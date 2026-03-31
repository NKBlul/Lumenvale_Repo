using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public CapsuleCollider hitboxCol;
    [SerializeField] LayerMask enemyLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            other.GetComponent<BaseEnemy>().TakeDamage(1);
        }
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
