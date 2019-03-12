using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DNA
{
    public enum Actions
    {
        ATTACK,
        DEFEND,
        BUFF
    }
    public List<Actions> genes = new List<Actions>();

    public DNA(int genomeLenght = 50)
    {
        for(int i = 0; i < genomeLenght; i++)
        {
            genes.Add((Actions)Random.Range(0, 3));
        }
    }
    public DNA(DNA parent, DNA partner, float mutationRate=0.01f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            float mutationChance = Random.Range(0.0f, 1.0f);
            if(mutationChance <= mutationRate)
            {
                genes.Add((Actions)Random.Range(0, 3));
            }
            else
            {
                int chance = Random.Range(0, 2);
                if(chance == 0)
                {
                    genes.Add(parent.genes[i]);
                }
                else
                {
                    genes.Add(partner.genes[i]);
                }
                
            }
        }
    }  
}
