using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public Quest Quest { get; set; }
    public int RequiredAmount { get; set; }
    public int CurrentAmount { get; set; }
    public bool Completed { get; set; }
    public string Description { get; set; }
    public GoalType GoalType { get; set; }

    public virtual void Init()
    {

    }

    public void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
        else
        {
            InComplete();
        }
    }

    public virtual void OnHandIn()
    {

    }

    public void Complete()
    {
        Completed = true;
        Quest.CheckGoals();
    }

    public void InComplete()
    {
        Completed = false;
        Quest.CheckGoals();
    }
}
