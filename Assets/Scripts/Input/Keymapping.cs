using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keymappings", menuName = "Keymappings")]
public class Keymapping : ScriptableObject{
    public List<KeyCode> 
        Inventory,
        Stats,
        Questlog,
        UseItem, 
        Exitbutton,
        Map,
        Hotbar1,
        Hotbar2,
        Hotbar3,
        Hotbar4,
        Hotbar5,
        Hotbar6,
        Hotbar7,
        Splitting,
        LeftClick,
        RightClick
    ;
}