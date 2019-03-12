using UnityEngine;

public class Hero : MonoBehaviour
{

    public float damage = 10;
    public float buffedDamage = 30;
    public float baseDamage = 10;
    public float health = 50;
    private bool defending = false;
    private int turnsBuffed = 0;
    private int buffedTurns = 1;
    public DNA dna;

    public bool isDead = false;

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
        damage = buffedDamage;
    }

    public void TurnEnd()
    {
        if (turnsBuffed-- <= 0)
            damage = baseDamage;
    }

    public void Hit(float damage)
    {
        if (!defending) health -= damage;
        isDead = health <= 0;
    }

    public void TurnStart()
    {
        defending = false;
    }
}
