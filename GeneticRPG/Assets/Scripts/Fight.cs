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
            float score = 0;
            if (boss.IsDead) score = 10000 + 300000 / turnCount;
            else score = turnCount * 20 + (boss.maxHealth - boss.health);
            return score;
        }
    }

    public void HeroTurn()
    {
        hero.TurnStart();

        if (turnCount < hero.dna.genes.Count && !hero.IsDead)
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
        if(boss.IsDead || turnCount >= hero.dna.genes.Count)
            hasFinished = true;
    }

    public void BossTurn()
    {
        int turnRate = turnCount % boss.currentBehaviour.bossBehaviour.Length;

        switch (boss.currentBehaviour.bossBehaviour[turnRate])
        {
            case BossBehaviour.actionType.ATTACK:
                boss.anim.SetTrigger("Attack 01");
                hero.Hit(boss.damage);
                break;
            case BossBehaviour.actionType.DEFEND:
                // defense animation here
                boss.Defend();
                break;
            case BossBehaviour.actionType.NONE:
                break;
            default:
                break;
        }

        if (hero.IsDead)
            hasFinished = true;
    }

}

