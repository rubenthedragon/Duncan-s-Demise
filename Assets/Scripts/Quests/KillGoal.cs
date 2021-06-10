using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public EnemyID EnemyId { get; set; }

    public KillGoal(Quest quest, EnemyID enemyId, string description, bool completed, int currentAmount, int requiredAmount, GoalType goalType)
    {
        this.Quest = quest;
        this.EnemyId = enemyId;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
        this.GoalType = goalType;
    }

    public override void Init()
    {
        base.Init();
        GoalEventHandler.OnEnemyDeath += EnemyDied;
    }

    private void EnemyDied(EnemyID enemyId)
    {
        if (EnemyId == enemyId && !Completed)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
}
