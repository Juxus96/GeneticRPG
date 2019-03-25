using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hero Genetic Data that stores the actions that he will be doing during it's generation
/// </summary>
[System.Serializable]
public class DNA
{
    public enum Actions
    {
        ATTACK,
        DEFEND,
        BUFF
    }
    private int actionEnumLength = System.Enum.GetNames(typeof(Actions)).Length;
    // Stores every action gene
    public List<Actions> genes = new List<Actions>();

    // Build the starting DNA (Random)
    public DNA(int genomeLenght = 50)
    {
        for(int i = 0; i < genomeLenght; i++)
            genes.Add((Actions)Random.Range(0, actionEnumLength));
    }

    // Builds the next generation with the given DNA data and mutation rate
    public DNA(DNA parent, DNA partner, float mutationRate = 0.01f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            // Roll mutation
            float mutationChance = Random.Range(0.0f, 1.0f);
            if(mutationChance <= mutationRate)
                genes.Add((Actions)Random.Range(0, actionEnumLength));
            else
            {
                // Otherwise take the parent or partner genes
                int chance = Random.Range(0, 2);
                if(chance == 0)
                    genes.Add(parent.genes[i]);
                else
                    genes.Add(partner.genes[i]);
            }
        }
    }  
}
