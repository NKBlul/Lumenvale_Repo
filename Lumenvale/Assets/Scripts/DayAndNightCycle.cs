using UnityEngine;

public class DayAndNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Length of a full day in minutes")]
    public float dayLengthInMinutes = 0.1f;

    [Range(0f, 24f)]
    public float startTimeOfDay = 12f; // Noon by default

    [Header("References")]
    public Light sunLight;

    [SerializeField] private float timeOfDay; // 0–24 hours

    void Start()
    {
        timeOfDay = startTimeOfDay;
    }

    void Update()
    {
        UpdateTime();
        UpdateSun();
    }

    void UpdateTime()
    {
        float dayLengthInSeconds = dayLengthInMinutes * 60f;

        // Advance time (24 hours cycle)
        timeOfDay += (24f / dayLengthInSeconds) * Time.deltaTime;

        if (timeOfDay >= 24f)
            timeOfDay = 0f;
    }

    void UpdateSun()
    {
        // Convert time (0–24) to rotation (0–360)
        float sunRotation = (timeOfDay / 24f) * 360f;

        // Rotate around X axis (like sunrise to sunset)
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0f);
    }
}
