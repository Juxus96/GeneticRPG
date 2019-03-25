using UnityEngine;

/// <summary>
/// Main Character that has to kill the Boss
/// </summary>
public class Hero : MonoBehaviour
{
    [Header("Stats")]
    public float buffedDamage = 30;
    public float baseDamage = 10;
    public float Damage { get; set; }
    public float Health { get; set; }
    public float maxHealth = 50;
    private bool defending = false;
    private int turnsBuffed = 0;
    private int buffedTurns = 1;
    public bool IsDead;

    [Header("DNA")]
    public DNA dna;

    public Animator anim;

    private void Awake()
    {
        Damage = baseDamage;
        Health = maxHealth;
    }

    public void InitHero(DNA newDna)
    {
        dna = newDna;
    }

    public void Defend()
    {
        defending = true;
    }

    /// <summary>
    /// Buff the current unit giving it more damage
    /// </summary>
    public void Buff()
    {
        turnsBuffed = buffedTurns;
        Damage = buffedDamage;
    }

    /// <summary>
    /// At the end of the turn we remove the buffed effect, only if it has run out of time
    /// </summary>
    public void TurnEnd()
    {
        if (turnsBuffed-- <= 0)
            Damage = baseDamage;
    }

    /// <summary>
    /// Deals damage to this unit with the given value, unless it's defending
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        if (!defending)
        {
            Health = Mathf.Clamp(Health - damage, 0, maxHealth);
            Interface.instance.SpawnDamageText(transform.position + Vector3.up, damage);
        }
        if (Health <= 0)
        {
            IsDead = true;
            anim.SetTrigger("Die");
        }
    }

    public void TurnStart()
    {
        defending = false;
    }
}
