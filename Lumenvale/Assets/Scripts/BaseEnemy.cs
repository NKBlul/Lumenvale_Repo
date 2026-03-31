using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider col;
    [SerializeField] int health = 3;

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("hurt");
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("DEAD");
        //Destroy(gameObject);
    }
}
