﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    List<Fight> fightList = new List<Fight>();
    public GameObject FightPrefab;
    public int rows = 10;
    public int columns = 10;
    public int fightSteps = 10;
    public float cutoff = 0.3f;
    public float timePerTurn = 0.2f;
    private int fightCount;
    private int gen = 1;
    private bool populated;
    private bool paused;
    
    /// <summary>
    /// creates the 1st random population of fights
    /// </summary>
    public void Populate()
    {
        for(int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                fightList.Add(CreateFight(new Vector3(j * 10, 0, i * 5), new DNA(fightSteps)));

        StartCoroutine(HeroTurns());
        fightCount = rows * columns;
        populated = true;

    }

    /// <summary>
    /// gets the best 30% fights and creates the new generation with their combination
    /// </summary>
    void NextGeneration()
    {
        Interface.instance.CreateDataDisplay(fightList, gen);

        // Get survivors (the best fighters)
        int survivorCut = Mathf.RoundToInt(fightCount * cutoff);
        List<Fight> survivors = new List<Fight>();
        for(int i = 0; i < survivorCut; i++)
            survivors.Add(GetFittest());

        // Destroy every other fight
        for (int i = 0; i < fightList.Count; i++)
            Destroy(fightList[i].gameObject);

        // Clears old fightList
        fightList.Clear();

        // Create new fights
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                fightList.Add(CreateFight(new Vector3(j * 10, 0, i * 5), new DNA(survivors[i].hero.dna, survivors[Random.Range(0, 10)].hero.dna)));
         
        StartCoroutine(HeroTurns());

        // Destroy survivors list
        for (int i = 0; i < survivors.Count; i++)
            Destroy(survivors[i].gameObject);

    }


    private IEnumerator HeroTurns()
    {
        yield return new WaitForSeconds(timePerTurn);

        foreach (Fight fight in fightList)
            if (!fight.hasFinished)
                fight.HeroTurn();

        if (HasActive())
            StartCoroutine(BossTurns());
        else if (!paused)
            NextGeneration();
    }

    private IEnumerator BossTurns()
    {
        yield return new WaitForSeconds(timePerTurn);

        foreach (Fight fight in fightList)
            if (!fight.hasFinished)
                fight.BossTurn();

        if (HasActive())
            StartCoroutine(HeroTurns());
        else if (!paused)
            NextGeneration();
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

    public void TogglePause()
    {
        paused = !paused;
    }


}
