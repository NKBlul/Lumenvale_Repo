Shader "Custom/HD2D_SpriteLitShadow_Balanced"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5

        _MinLight("Minimum Light", Range(0,1)) = 0.15
        _AmbientStrength("Ambient Strength", Range(0,2)) = 1.0
        _RimStrength("Rim Light Strength", Range(0,1)) = 0.2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        // =========================
        // Forward Lit Pass
        // =========================
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float4 shadowCoord : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half _Cutoff;

                half _MinLight;
                half _AmbientStrength;
                half _RimStrength;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.shadowCoord = TransformWorldToShadowCoord(OUT.positionWS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // Alpha cutoff
                clip(tex.a - _Cutoff);

                // Camera-facing normal (good for sprites)
                float3 normal = normalize(_WorldSpaceCameraPos - IN.positionWS);

                // Main light
                Light mainLight = GetMainLight(IN.shadowCoord);
                float NdotL = saturate(dot(normal, mainLight.direction));
                float shadowAtten = MainLightRealtimeShadow(IN.shadowCoord);

                float3 directLight = mainLight.color * NdotL * shadowAtten;

                // Ambient fades when sun is strong
                float3 ambient = SampleSH(normal) * _AmbientStrength * (1.0 - NdotL);

                // Minimum light only at night
                float minFactor = 1.0 - NdotL;
                float3 minLight = _MinLight * minFactor;

                // Rim light (reduced at noon)
                float rim = 1.0 - saturate(dot(normal, mainLight.direction));
                float3 rimLight = rim * _RimStrength * (1.0 - NdotL) * mainLight.color;

                // Combine lighting
                float3 lighting = tex.rgb * (directLight + ambient + minLight) + rimLight;

                // Safety clamp (prevents overexposure)
                lighting = saturate(lighting);

                return half4(lighting, tex.a);
            }
            ENDHLSL
        }

        // =========================
        // Shadow Caster Pass
        // =========================
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half _Cutoff;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(positionWS);
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                clip(tex.a - _Cutoff);

                return 1;
            }

            ENDHLSL
        }
    }
}