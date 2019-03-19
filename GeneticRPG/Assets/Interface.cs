using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public static Interface instance;

    [SerializeField] private GameObject fightUI_Prefab;
    [SerializeField] private GameObject actionUI_Prefab;
    [SerializeField] private Transform fightScrollDisplay;
    [SerializeField] private Transform actionScrollDisplay;
    [SerializeField] private TextMeshProUGUI genCountText;
    [SerializeField] private TextMeshProUGUI bestFightNameText;
    [SerializeField] private Color bestFightColor;
    [SerializeField] private Color survivedFightColor;
    [SerializeField] private Color deadFightColor;

    private void Awake()
    {
        instance = this;
    }

    public void ClearDataDisplay()
    {
        foreach (Transform fight in fightScrollDisplay)
            fight.gameObject.SetActive(false);
    }

    public void CreateDataDisplay(List<Fight> fightList, int genCount)
    {
        ClearDataDisplay();

        genCountText.text = "Generation " + genCount;

        fightList.Sort((f1, f2) => f2.fightScore.CompareTo(f1.fightScore));

        for (int i = 0; i < fightList.Count; i++)
        {
            Fight currentFight = fightList[i];
            Transform newFight = Instantiate(fightUI_Prefab, fightScrollDisplay).transform;
            newFight.SetSiblingIndex(i);
            newFight.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1) + "";
            newFight.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentFight.name;
            newFight.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentFight.hero.Health + " / " + currentFight.hero.maxHealth;
            if (currentFight.hero.Health > 0)   newFight.GetComponent<Image>().color = survivedFightColor;
            else                                newFight.GetComponent<Image>().color = deadFightColor;
            newFight.GetChild(3).GetComponent<TextMeshProUGUI>().text = currentFight.boss.health + " / " + currentFight.boss.maxHealth;
            newFight.GetChild(4).GetComponent<TextMeshProUGUI>().text = currentFight.turnCount + "";
            newFight.GetChild(5).GetComponent<TextMeshProUGUI>().text = currentFight.fightScore + "";

            EventTrigger eventTrigger = newFight.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { SelectFight(currentFight); });
            eventTrigger.triggers.Add(entry);
        }

        fightScrollDisplay.GetChild(0).GetComponent<Image>().color = bestFightColor;

        for (int i = fightList.Count; i < fightScrollDisplay.childCount; i++)
            Destroy(fightScrollDisplay.GetChild(i).gameObject);

    }

    public void Toggle(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SelectFight(Fight fight)
    {
        List<DNA.Actions> genes = fight.hero.dna.genes;
        for (int i = 0; i < genes.Count; i++)
        {
            GameObject newAction = Instantiate(actionUI_Prefab, actionScrollDisplay);
            newAction.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = genes[i].ToString();
        }
    }

    
}
