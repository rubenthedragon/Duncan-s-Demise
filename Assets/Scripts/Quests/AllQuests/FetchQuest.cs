using UnityEngine;

public class FetchQuest : Quest
{
    [SerializeField] private ItemObject itemRequired;

    public override void Init()
    {
        if (Goals.Count == 0)
            Goals.Add(new ItemGoal(this, itemRequired, goalDescription, false, currentGoalAmount, requiredGoalAmount, GoalType.ItemGoal));
        base.Init();
    }
}
