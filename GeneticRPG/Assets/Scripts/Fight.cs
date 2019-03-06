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
    public int nextAction;

    Vector2 target;
    Vector2 nextPoint;

    public void StartFight(DNA dna)
    {
        hero = Instantiate(hero,heroSpawn);
        boss = Instantiate(boss,bossSpawn);
        hero.InitHero(dna);
        StartCoroutine(HeroTurn());
    }
    

    public float fightScore
    {
        get
        {
            float score = boss.maxHealth - boss.health + nextAction*7;
            if (boss.health == 0) { score += 1000 / nextAction; print("boss died"); }
            return score;
        }
    }

    private IEnumerator HeroTurn()
    {
        yield return new WaitForSeconds(0.1f);
        if (!hasFinished)
        {
            hero.TurnStart();
            switch(hero.dna.genes[nextAction])
            {
                case DNA.Actions.ATTACK:
                    boss.RecieveDamage(hero.damage);
                    break;
                case DNA.Actions.DEFEND:
                    hero.Defend();
                    break;
                case DNA.Actions.BUFF:
                    hero.Buff();
                    break;
            }
            if (nextAction == hero.dna.genes.Count || boss.health == 0)
            {
                hasFinished = true;
            }
            else
            {
                nextAction++;
                hero.TurnEnd();
                StartCoroutine(BossTurn());
            }
        }
    }


    private IEnumerator BossTurn()
    {
        yield return new WaitForSeconds(0.1f);
        if (!hasFinished)
        {
            if (nextAction % boss.attackTemp == 0) hero.RecieveDamage(boss.damage);
            if (hero.isDead)
            {
                hasFinished = true;
            }
            else
            {
                StartCoroutine(HeroTurn());
            }
        }
    }

}
