using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public float damage = 30;
    public float health = 300;
    public float maxHealth = 500;
    private bool defending = false;
    public bool IsDead { get; set; }

    public Animator anim;

    public BossBehaviour currentBehaviour;

    public void Defend()
    {
        defending = true;
    }

    public void Hit(float damage)
    {
        if (!defending)
        {
            health = Mathf.Clamp(health - damage, 0, maxHealth);
            Interface.instance.SpawnDamageText(transform.position + Vector3.up, damage);
        }

        if (health <= 0)
        {
            IsDead = true;
            anim.SetTrigger("die");
        }
    }
}


