using UnityEngine;

public class KillQuest : Quest
{
    [SerializeField] private EnemyID enemyID;

    public override void Init()
    {
        if (Goals.Count == 0)
            Goals.Add(new KillGoal(this, enemyID, goalDescription, false, currentGoalAmount, requiredGoalAmount, GoalType.KillGoal));
        base.Init();
    }
}
