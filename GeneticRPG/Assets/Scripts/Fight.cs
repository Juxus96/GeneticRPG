using UnityEngine;

[System.Serializable]
public class Fight : MonoBehaviour
{
    [HideInInspector] public string nameGo; 
    public Hero hero;
    public Transform heroSpawn;
    public Boss boss;
    public Transform bossSpawn;

    public bool hasFinished;
    public int turnCount;


    public void StartFight(DNA dna, BossBehaviour newBehaviour)
    {
        hero = Instantiate(hero,heroSpawn);
        boss = Instantiate(boss,bossSpawn);
        hero.InitHero(dna);
        if (newBehaviour) boss.currentBehaviour = newBehaviour;
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
                    hero.anim.SetTrigger("Attack 01");
                    boss.Hit(hero.Damage);
                    break;
                case DNA.Actions.DEFEND:
                    hero.anim.SetTrigger("Defend");
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
        int turnRate = turnCount % boss.currentBehaviour.bossBehaviour.Length;

        switch (boss.currentBehaviour.bossBehaviour[turnRate])
        {
            case BossBehaviour.actionType.ATTACK:
                hero.Hit(boss.damage);
                if (hero.IsDead)
                    hasFinished = true;
                break;
            case BossBehaviour.actionType.DEFEND:
                boss.Defend();
                break;
            case BossBehaviour.actionType.NONE:
                break;
            default:
                break;
        }

    }

}

