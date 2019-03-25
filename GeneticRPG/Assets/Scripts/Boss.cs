using UnityEngine;

/// <summary>
/// Main Enemy that the Hero has to kill
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public float    damage = 30;
    public float    health = 300;
    public float    maxHealth = 500;
    public bool     IsDead;
    private bool    defending = false;

    public Animator anim;

    public BossBehaviour currentBehaviour;

    public void Defend()
    {
        defending = true;
    }

    /// <summary>
    /// Deals damage to this unit with the given value, unless it's defending
    /// </summary>
    /// <param name="damage"></param>
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


