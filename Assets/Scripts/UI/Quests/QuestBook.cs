using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBook : MonoBehaviour
{
    [SerializeField] private Text questName;
    [SerializeField] private GameObject questGoals;
    [SerializeField] private Text questDescription;
    [SerializeField] private GameObject questRewards;
    [SerializeField] private GameObject goalPrefab;

    [Header("Reward Components")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text goldRewardText;
    [SerializeField] private Text xpRewardText;
    [SerializeField] private GameObject selectedQuestInfo;

    private GameObject goGoal;

    public void SetSelectedQuest(Quest quest) 
    {
        ClearSelectedQuest();
        questName.text = quest.Name;
        questDescription.text = quest.Description;
        foreach (Goal goal in quest.Goals) 
        {
            if(goal.Quest != null)
            {
                goGoal = Instantiate(goalPrefab, questGoals.transform);
                goGoal.GetComponent<Text>().text = $"{goal.Description}: {goal.CurrentAmount} / {goal.RequiredAmount}";
            }
        }
        if (quest.ItemReward != null)
        {
            itemIcon.sprite = quest.ItemReward.uiDisplay;
        }
        else
        {
            itemIcon.enabled = false;
        }
        selectedQuestInfo.SetActive(true);
        goldRewardText.text = quest.GoldReward.ToString();
        xpRewardText.text = $"{quest.ExpReward} XP";
    }

    public void ClearSelectedQuest()
    {
        questName.text = "";
        questDescription.text = "";
        itemIcon.sprite = null;
        itemIcon.enabled = true;
        goldRewardText.text = "";
        xpRewardText.text = "";
        selectedQuestInfo.SetActive(false);

        Destroy(goGoal);
    }


}
