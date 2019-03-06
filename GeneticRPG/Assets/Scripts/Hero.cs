using UnityEngine;

public class Hero : MonoBehaviour
{

    public float damage = 10;
    public float buffedDamage = 30;
    public float baseDamage = 10;
    public float health = 50;
    private bool defending = false;
    private bool buffed = false;
    private int turnsBuffed = 0;
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
        if (buffed) turnsBuffed--;
        damage = buffedDamage;
        buffed = true;
    }

    public void TurnEnd()
    {
        if (turnsBuffed >= 1 && buffed)
        {
            turnsBuffed--;
            damage = baseDamage;
            buffed = true;
        }
        else
        {
            turnsBuffed++;
        }
    }

    public void RecieveDamage(float damage)
    {
        if (!defending) health -= damage;
        isDead = health <= 0;
    }

    public void TurnStart()
    {
        defending = false;
    }
}
