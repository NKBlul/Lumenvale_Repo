using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public Collider hitboxCol;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
    }
}
