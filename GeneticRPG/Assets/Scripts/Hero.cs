using UnityEngine;

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
    public bool IsDead { get; set; }

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

    public void Buff()
    {
        turnsBuffed = buffedTurns;
        Damage = buffedDamage;
    }

    public void TurnEnd()
    {
        if (turnsBuffed-- <= 0)
            Damage = baseDamage;
    }

    public void Hit(float damage)
    {
        if (!defending)
            Health = Mathf.Clamp(Health - damage, 0, maxHealth);
        if (Health <= 0)
            IsDead = true;
    }

    public void TurnStart()
    {
        defending = false;
    }
}
