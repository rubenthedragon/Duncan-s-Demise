using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questBook;
    [SerializeField] private QuestItem Quest;
    [SerializeField] private GameObject QuestListPanel;
    private Player Player;
    private List<Quest> QuestList;
    private List<QuestItem> QuestItems = new List<QuestItem>();

    private void Start()
    {
        questBook.SetActive(false);
    }

    public void OpenCloseQuestlog()
    {
        if (questBook.gameObject.activeSelf == true)
        {
            questBook.gameObject.SetActive(false);
            questBook.GetComponent<QuestBook>().ClearSelectedQuest();
        }
        else
        {
            Player = GameObject.Find("Player").GetComponent<Player>();
            QuestList = Player.QuestList;
            questBook.gameObject.SetActive(true);
            foreach (Quest quest in QuestList)
            {
                for (int i = 0; i < QuestItems.Count; i++)
                {
                    if (QuestItems[i].quest == quest)
                    {
                        QuestItems[i].UpdateItem(quest);
                        break;
                    }
                }
                if (QuestItems.Find(x => x.quest.Name.Equals(quest.Name)))
                {
                    continue;
                }
                QuestItem q = Instantiate(Quest, QuestListPanel.transform);
                q.quest = quest;
                QuestItems.Add(q);
            }
        }
    }
}
