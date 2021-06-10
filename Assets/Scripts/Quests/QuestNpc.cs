using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpc : NpcObject, IOnLoadAndSave
{
    [SerializeField] private Quest quest;
    [SerializeField] private Sprite explenationsMarker;
    [SerializeField] private Sprite questionMarker;
    [SerializeField] private Sprite questionMarkerGrey;
    [SerializeField] private bool sender;

    [Header("Optional: on target quest handed in be enabled")]
    [SerializeField] private Quest onQuestHandedIn2;

    [Header("Optional: on target quest handed in be disabled")]
    [SerializeField] private Quest onQuestHandedIn;

    protected override void Start()
    {
        base.Start();
        if (!DataControl.control.Quests.Find(q => q.Name == quest.Name))
            quest.OnSceneEnter();

        if (quest)
        {
            quest.OnStepChange += SetMarker;
        }

        if(onQuestHandedIn && onQuestHandedIn2)
        {
            GoalEventHandler.OnQuestHandedIn += OnTargetQuestHandedIn;
            OnTargetQuestHandedIn(onQuestHandedIn);
            OnTargetQuestHandedIn(onQuestHandedIn2);
        }
        else
        {
            if (onQuestHandedIn2)
            {
                GoalEventHandler.OnQuestHandedIn += OnTargetQuestHandedIn;
                if (onQuestHandedIn2.QuestStep == Quest.QuestSteps.HasHandedIn)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }

            if (onQuestHandedIn)
            {
                GoalEventHandler.OnQuestHandedIn += OnTargetQuestHandedIn;
                if (onQuestHandedIn.QuestStep == Quest.QuestSteps.HasHandedIn)
                {
                    Disable();
                }
                else
                {
                    Enable();
                }
            }
        }

        SetMarker();
    }

    private void Enable()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;

        foreach (SpriteRenderer renderer in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = true;
        }
    }

    private void Disable()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;

        foreach (SpriteRenderer renderer in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = false;
        }
    }

    protected override void SetMarker()
    {
        if (playerInRange)
        {
            base.SetMarker();
            return;
        }
        if (quest.QuestStep == Quest.QuestSteps.NotAccepted)
        {
            if (quest is GoToQuest && !sender)
            {
                base.SetMarker();
            }
            else
            {
                mover.StartRoutine(explenationsMarker);
            }
        }
        else if (quest.QuestStep == Quest.QuestSteps.InProgress && !sender)
        {
            mover.StartRoutine(questionMarkerGrey); 
        }
        else if (quest.QuestStep == Quest.QuestSteps.HasCompleted && !sender)
        {
            mover.StartRoutine(questionMarker);
        }
        else if (quest.QuestStep == Quest.QuestSteps.HasHandedIn || sender)
        {
            mover.StopRoutine();
        }
    }

    protected override void Check()
    {
        Player player = (GameObject.Find("Player").GetComponent<Player>());

        if (player.QuestList.Contains(quest))
        {
            quest = player.QuestList.Find(w => w.name == quest.name);

            if (quest is GoToQuest)
            {
                if (!sender)
                {
                    quest.Completed = true;
                }
            }

            if (currentDialogNr != (int)quest.QuestStep)
            {
                NextDialog((int)quest.QuestStep);
            }
        }
    }

    private void OnTargetQuestHandedIn(Quest _quest)
    {
        if (this != null)
        {
            if (onQuestHandedIn && onQuestHandedIn2)
            {
                if (onQuestHandedIn2 && onQuestHandedIn2.name == _quest.name)
                {
                    if (onQuestHandedIn2.QuestStep == Quest.QuestSteps.HasHandedIn)
                    {
                        if (onQuestHandedIn.QuestStep == Quest.QuestSteps.HasHandedIn)
                        {
                            Disable();
                        }
                        else
                        {
                            Enable();
                        }
                    }
                }
                else if(onQuestHandedIn && onQuestHandedIn.name == _quest.name)
                {
                    if (onQuestHandedIn.QuestStep == Quest.QuestSteps.HasHandedIn)
                    {
                        if (this.gameObject.GetComponent<SpriteRenderer>().enabled)
                        {
                            Disable();
                        }
                    }
                    else
                    {
                        Disable();
                    }
                }
            }
            else
            {
                if (onQuestHandedIn2 && onQuestHandedIn2.name == _quest.name)
                {
                    if (onQuestHandedIn2.QuestStep == Quest.QuestSteps.HasHandedIn)
                    {
                        if (!this.gameObject.GetComponent<SpriteRenderer>().enabled)
                        {
                            Enable();
                        }
                    }
                }

                if (onQuestHandedIn && onQuestHandedIn.name == _quest.name)
                {
                    if (onQuestHandedIn.QuestStep == Quest.QuestSteps.HasHandedIn)
                    {
                        if (this.gameObject.GetComponent<SpriteRenderer>().enabled)
                        {
                            Disable();
                        }
                    }
                }
            }


        }
    }

    public void Save()
    {
        
    }

    public void Load()
    {
        if(quest != null)
        {
            if(DataControl.control.Quests.Find(w => w.Name == quest.Name))
            {
                quest = DataControl.control.Quests.Find(w => w.Name == quest.Name);

                if(onQuestHandedIn2 != null && onQuestHandedIn != null && DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn2.Name) && DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn.Name))
                {
                    onQuestHandedIn2 = DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn2.Name);
                    onQuestHandedIn = DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn.Name);
                    OnTargetQuestHandedIn(onQuestHandedIn2);
                    OnTargetQuestHandedIn(onQuestHandedIn);
                }
                else
                {
                    if (onQuestHandedIn2 != null)
                    {
                        if (DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn2.Name))
                        {
                            onQuestHandedIn2 = DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn2.Name);
                            OnTargetQuestHandedIn(onQuestHandedIn2);
                        }
                    }

                    if (onQuestHandedIn != null)
                    {
                        if (DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn.Name))
                        {
                            onQuestHandedIn = DataControl.control.Quests.Find(w => w.Name == onQuestHandedIn.Name);
                            OnTargetQuestHandedIn(onQuestHandedIn);
                        }
                    }
                }
            }
        }
    }

    public void OnEnable()
    {
        DataControl.control.OnLoad += Load;
    }

    public void OnDisable()
    {
        DataControl.control.OnLoad -= Load;
    }
}
