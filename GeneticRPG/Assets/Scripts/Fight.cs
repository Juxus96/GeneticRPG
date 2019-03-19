using System.Collections;
using UnityEngine;

[System.Serializable]
public class Fight : MonoBehaviour
{

    public Hero hero;
    public Transform heroSpawn;
    public Boss boss;
    public Transform bossSpawn;

    public bool hasFinished;
    public int turnCount;

    public void StartFight(DNA dna)
    {
        hero = Instantiate(hero,heroSpawn);
        boss = Instantiate(boss,bossSpawn);
        hero.InitHero(dna);
    }
    
    public float fightScore
    {
        get
        {
            float score = turnCount * 20 + (boss.maxHealth - boss.health);
            if (boss.IsDead) { score += 1000 / turnCount; }
            return score;
        }
    }

    public void HeroTurn()
    {
        hero.TurnStart();

        if (turnCount < hero.dna.genes.Count && !boss.IsDead)
        {
            switch (hero.dna.genes[turnCount])
            {
                case DNA.Actions.ATTACK:
                    hero.anim.SetTrigger("Attack");
                    boss.Hit(hero.Damage);
                    break;
                case DNA.Actions.DEFEND:
                    hero.Defend();
                    break;
                case DNA.Actions.BUFF:
                    hero.Buff();
                    break;
            }

            turnCount++;
            hero.TurnEnd();
        }
        else
            hasFinished = true;
    }


    public void BossTurn()
    {
        if (turnCount % boss.attackTemp == 0)
            hero.Hit(boss.damage);
        if (hero.IsDead)
            hasFinished = true;
    }

}
