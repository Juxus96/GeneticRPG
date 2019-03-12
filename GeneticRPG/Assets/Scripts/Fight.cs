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
        //StartCoroutine(HeroTurn());
    }
    
    public float fightScore
    {
        get
        {
            float score = nextAction * 20 + (boss.maxHealth - boss.health);
            if (boss.IsDead) { score += 1000 / nextAction; }
            return score;
        }
    }

    public void HeroTurn()
    {
        // TODO OUTSIDE FOR HERE ON FIGHT CONTROLLER, DO NOT COROUTINE ON EVERY FIGHT
        //yield return new WaitForSeconds(0.03f);
        hero.TurnStart();

        if (nextAction < hero.dna.genes.Count && !boss.IsDead)
        {
            switch (hero.dna.genes[nextAction])
            {
                case DNA.Actions.ATTACK:
                    boss.Hit(hero.damage);
                    break;
                case DNA.Actions.DEFEND:
                    hero.Defend();
                    break;
                case DNA.Actions.BUFF:
                    hero.Buff();
                    break;
            }

            nextAction++;
            hero.TurnEnd();
        }
        else
        {
            hasFinished = true;
        }
    }


    public void BossTurn()
    {
        // TODO OUTSIDE FOR HERE ON FIGHT CONTROLLER, DO NOT COROUTINE ON EVERY FIGHT
        //yield return new WaitForSeconds(0.03f);
        if (nextAction % boss.attackTemp == 0)
            hero.Hit(boss.damage);
        if (hero.isDead)
            hasFinished = true;
        
    }

}
