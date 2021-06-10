using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stats {
    Vitality ,      //Health    Vitality: 5 = 100 HP
    Wisdom ,        //Mana      Wisdom:	5 = 100 mana
    Strength ,      //Damage
    Dexterity ,     //MovementSpeed
    Intelligence,   //
    Defense         //Damage Reduction
}

[System.Serializable]
public class ItemBuff
{
   public Stats stat;
   public int Value;
   public int min;
   public int max;

   public ItemBuff(int _min, int _max)
   {
       min = _min;
       max = _max;
       generateValue();
   }

   public void generateValue()
   {
       Value = UnityEngine.Random.Range(min,max);
   }
}