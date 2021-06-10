using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private Text questNameText;
    [SerializeField] private Text marker;
    [SerializeField] private string description;
    [HideInInspector] public Quest quest;
    private bool isQuestSelected;

    private void Start()
    {
        questNameText.text = quest.Name;
        description = quest.Description;
        UpdateItem(quest);
    }

    public void UpdateItem(Quest _quest)
    {
        quest = _quest;
        if (quest.IsHandedIN)
        {
            marker.text = "(Handed in)";
            marker.color = Color.gray;
            questNameText.color = Color.gray;
        }
        else if (quest.Completed)
        {
            marker.text = "(Completed)";
        }
        else
        {
            marker.text = "(Not completed)";
        }
    }

    public void OnMouseClick()
    {
        transform.GetComponentInParent<QuestBook>()?.SetSelectedQuest(quest);
    }

}
