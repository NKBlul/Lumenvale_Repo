using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public CapsuleCollider hitboxCol;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
    }

    public void EnableHitbox()
    {
        hitboxCol.enabled = true;
    }

    public void DisableHitbox()
    {
        hitboxCol.enabled = false;
    }
}
