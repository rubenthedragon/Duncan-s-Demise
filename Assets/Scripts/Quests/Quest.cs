using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quest : MonoBehaviour
{
    [field: HideInInspector] public List<Goal> Goals { get; private set; } = new List<Goal>();
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int GoldReward { get; private set; }
    [field: SerializeField] public int ExpReward { get; private set; }
    [field: SerializeField] public ItemObject ItemReward { get; private set; }
    [field: SerializeField] public int ItemRewardAmount { get; private set; }
    [field: SerializeField] public int currentGoalAmount { get; private set; }
    [field: SerializeField] public int requiredGoalAmount { get; private set; }
    [field: SerializeField] public string goalDescription { get; private set; }
    public Action OnStepChange { get; set; }
    [SerializeField] private AudioClip handInSound;

    private bool accepted = false;
    public bool Accepted
    {
        get
        {
            return accepted;
        }
        set
        {
            if (accepted != value)
            {
                accepted = value;
                CheckGoals();
            }
        }
    }

    private bool completed = false;
    public bool Completed 
    {
        get
        {
            return completed;
        }
        set
        {
            if (completed != value)
            {
                completed = value;
                CheckGoals();
            }
        } 
    }

    private bool isHandedIN = false;
    public bool IsHandedIN
    {
        get
        {
            return isHandedIN;
        }
        set
        {
            if (isHandedIN != value)
            {
                isHandedIN = value;
                CheckGoals();
            }
        }
    }

    private QuestSteps questStep = QuestSteps.NotAccepted;
    public QuestSteps QuestStep
    {
        get
        {
            return questStep;
        }
        set
        {
            if(value != questStep)
            {
                if (value == QuestSteps.HasCompleted && this.GetType() != typeof(GoToQuest))
                {
                    FindObjectOfType<Player>().QuestProgressionMade($"Quest completed: '{Name}'");
                }
                questStep = value;
                OnStepChange?.Invoke();
            }
        }
    }

    public enum QuestSteps
    {
        NotAccepted,
        InProgress,
        HasCompleted,
        HasHandedIn,
    }

    public void OnSceneEnter()
    {
        Goals.Clear();
        Accepted = false;
        Completed = false;
        IsHandedIN = false;
    }

    public virtual void Init()
    {
        Accepted = true;
        Completed = false;
        IsHandedIN = false;
        Goals.ForEach(g => g.Init());
    }

    public void ResetQuest()
    {
        Accepted = false; 
        Completed = false;
        IsHandedIN = false;
        Goals.ForEach(g => g.Init());
    }

    public void CheckGoals()
    {
        if(Goals.Count == 0)
        {
            if(requiredGoalAmount == 0)
            Completed = true;
        }
        else
        {
            //if goals count is 0 this will flag as true thats why the if above must be there
            Completed = Goals.All(g => g.Completed);
        }

        if (IsHandedIN == true)
        {
            QuestStep = QuestSteps.HasHandedIn;
            GoalEventHandler.QuestHandedIn(this);
            return;
        }

        if (Completed == true && IsHandedIN == false)
        {
            QuestStep = QuestSteps.HasCompleted;
            return;
        }

        if (Accepted)
        {
            QuestStep = QuestSteps.InProgress;
            return;
        }

        QuestStep = QuestSteps.NotAccepted;

    }

    public void GiveReward()
    {
        Player player = (Player)GameObject.Find("Player").GetComponent("Player");
        AudioPlayer.Audioplayer.PlaySFX(handInSound, 0.3f);
        IsHandedIN = true;
        player.AddExp(ExpReward);
        if (ItemReward != null) 
        {
            int leftover = player.hotbar.AddItem(ItemReward, ItemRewardAmount);
            leftover = player.inventory.AddItem(ItemReward, leftover);
            if(leftover > 0) {
                GroundItem drop = Instantiate(Resources.Load<GroundItem>("GroundItem"));
                drop.SetItem(ItemReward, leftover);
                drop.transform.position = player.transform.position + Vector3.down;
            }     
        }
        player.Gold += GoldReward;
        foreach (Goal goal in Goals)
        {
            goal.OnHandIn();
        }
    }
}
