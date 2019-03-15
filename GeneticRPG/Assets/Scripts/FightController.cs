using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    List<Fight> fightList = new List<Fight>();
    public GameObject FightPrefab;
    public int FightCount = 10;
    public int fightSteps = 10;
    public float cutoff = 0.3f;
    public float timePerTurn = 0.2f;
    private float gen = 0;
    
    /// <summary>
    /// creates the 1st random population of fights
    /// </summary>
    void InitPopulation()
    {
        for(int i = 0; i < FightCount; i++)
            fightList.Add(CreateFight(new Vector3(0, 0, i * 5), new DNA(fightSteps)));

        StartCoroutine(HeroTurns());
        
    }

    /// <summary>
    /// gets the best 30% fights and creates the new generation with their combination
    /// </summary>
    void NextGeneration()
    {
        // Get survivors (the best fighters)
        int survivorCut = Mathf.RoundToInt(FightCount * cutoff);
        List<Fight> survivors = new List<Fight>();
        for(int i = 0; i < survivorCut; i++)
            survivors.Add(GetFittest());

        Debug.Log("Gen: " + gen++ + " Survivors: " + survivors.Count);
        Fight bestFight = survivors[0];
        Debug.Log("Best fighter. Score: " + bestFight.fightScore + " Boss Health: " + bestFight.boss.health + " / " + bestFight.boss.maxHealth + " Total turns: " + bestFight.turnCount + " Hero Health: " + bestFight.hero.Health);

        // Destroy every other fight
        for (int i = 0; i < fightList.Count; i++)
            Destroy(fightList[i].gameObject);

        // Clears old fightList
        fightList.Clear();

        // Create new fights
        while (fightList.Count < FightCount)
            for (int i = 0; i < survivors.Count && fightList.Count < FightCount; i++)
                fightList.Add(CreateFight(new Vector3(0, 0, i * 5), new DNA(survivors[i].hero.dna, survivors[Random.Range(0, 10)].hero.dna)));

        StartCoroutine(HeroTurns());

        // Destroy survivors list
        for (int i = 0; i < survivors.Count; i++)
            Destroy(survivors[i].gameObject);
    }

    private void Start()
    {
        InitPopulation();
    }

    private void Update()
    {
        if (!HasActive())
            NextGeneration();
    }

    private IEnumerator HeroTurns()
    {
        yield return new WaitForSeconds(timePerTurn);

        foreach (Fight fight in fightList)
            if (!fight.hasFinished)
                fight.HeroTurn();

        if (HasActive())
            StartCoroutine(BossTurns());
    }

    private IEnumerator BossTurns()
    {
        yield return new WaitForSeconds(timePerTurn);

        foreach (Fight fight in fightList)
            if (!fight.hasFinished)
                fight.BossTurn();

        if (HasActive())
            StartCoroutine(HeroTurns());
    }
   
    private Fight CreateFight(Vector3 position, DNA dna)
    {
        Fight newFight = Instantiate(FightPrefab, position, FightPrefab.transform.rotation).GetComponent<Fight>();
        newFight.StartFight(dna);
        newFight.name = "Fight " + (fightList.Count + 1);
        return newFight;
    }

    Fight GetFittest()
    {
        float maxFightScore = float.MinValue;
        int index = 0;
        for(int i = 0; i < fightList.Count; i++)
        {
            if(fightList[i].fightScore > maxFightScore)
            {
                maxFightScore = fightList[i].fightScore;
                index = i;
            }
        }
        Fight bestFight = fightList[index];
        fightList.Remove(bestFight);
        return bestFight;
    }

    bool HasActive()
    {
        
        for (int i = 0; i < fightList.Count; i++)
        {
            if (!fightList[i].hasFinished)
                return true;
        }
        return false;
    }
}
