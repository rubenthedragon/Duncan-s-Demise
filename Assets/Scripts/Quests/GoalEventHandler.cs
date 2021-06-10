using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEventHandler : MonoBehaviour
{
    public delegate void EnemyEventHandler(EnemyID enemyId);
    public static event EnemyEventHandler OnEnemyDeath;

    public static void EnemyDied(EnemyID enemyId)
    {
        if(OnEnemyDeath != null)
        {
            OnEnemyDeath?.Invoke(enemyId);
        }
    }


    public delegate void ItemPickupEventHandler(ItemObject item, int amount);
    public static event ItemPickupEventHandler OnItemPickup;

    public static void ItemPickedUp(ItemObject item, int amount)
    {
        OnItemPickup?.Invoke(item, amount);
    }

    public delegate void ItemDropEventHandler(ItemObject item, int amount);
    public static event ItemDropEventHandler OnItemDropped;

    public static void ItemDropped(ItemObject item, int amount)
    {
        OnItemDropped?.Invoke(item, amount);
    }

    public delegate void QuestHandedInEventHandler(Quest quest);
    public static event QuestHandedInEventHandler OnQuestHandedIn;

    public static void QuestHandedIn(Quest quest)
    {
        OnQuestHandedIn?.Invoke(quest);
    }
}
