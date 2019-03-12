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
        {
            GameObject go = Instantiate(FightPrefab , new Vector3(0, 0, i * 5), FightPrefab.transform.rotation);
            go.GetComponent<Fight>().StartFight(new DNA(fightSteps));
            go.name = "Fight " + i;
            fightList.Add(go.GetComponent<Fight>());
        }

        StartCoroutine(HeroTurns());
        
    }

    /// <summary>
    /// gets the best 30% fights and creates the new generation with their combination
    /// </summary>
    void NextGeneration()
    {
        print("Gen: " + gen++);
        int survivorCut = Mathf.RoundToInt(FightCount * cutoff);
        List<Fight> survivors = new List<Fight>();

        //gets the best scores of the last generation
        for(int i = 0; i < survivorCut; i++)
        {
            survivors.Add(GetFittest());
        }

        Debug.Log("Best fighter. Score: " + survivors[0].fightScore + " Boss Health: " + survivors[0].boss.health + " / " + survivors[0].boss.maxHealth + " Total turns: " + survivors[0].nextAction + " Hero Health: " + survivors[0].hero.health);// + survivors[0].hero);

        //kilss the rest of fights
        for (int i = 0; i < fightList.Count; i++)
        {
            Destroy(fightList[i].gameObject);
        }

        ////clears the new gen fightlists and completes it with the new gen
        fightList.Clear();

        while (fightList.Count < FightCount)
        {
            for (int i = 0; i < survivors.Count && fightList.Count < FightCount; i++)
            {
                GameObject go = Instantiate(FightPrefab, new Vector3(0, 0, i * 5), FightPrefab.transform.rotation);
                go.GetComponent<Fight>().StartFight(new DNA(survivors[i].hero.dna, survivors[Random.Range(0, 10)].hero.dna));
                fightList.Add(go.GetComponent<Fight>());
            }
        }

        StartCoroutine(HeroTurns());

        //clear survivors list
        for (int i = 0; i < survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }

    private void Start()
    {
        InitPopulation();
    }

    private void Update()
    {
       
        if (!HasActive())
        {
            NextGeneration();
        }
    }

    private IEnumerator HeroTurns()
    {
        yield return new WaitForSeconds(timePerTurn);

        foreach (Fight fight in fightList)
        {
            if (!fight.hasFinished)
                fight.HeroTurn();
        }

        if (HasActive())
            StartCoroutine(BossTurns());

    }

    private IEnumerator BossTurns()
    {
        yield return new WaitForSeconds(timePerTurn);

        foreach (Fight fight in fightList)
        {
            if (!fight.hasFinished)
                fight.BossTurn();
        }

        if (HasActive())
            StartCoroutine(HeroTurns());
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
