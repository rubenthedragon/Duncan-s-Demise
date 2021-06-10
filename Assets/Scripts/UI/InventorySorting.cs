using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using TMPro;

public class InventorySorting : MonoBehaviour
{

    TMP_Dropdown dropdown = null;
    private enum SortingOption
    {
        Weight,
        Value,
        Type
    }

    private void Awake(){
        dropdown = GetComponentInChildren<TMP_Dropdown>();
    }

    private void OnDisable(){
        dropdown.SetValueWithoutNotify(-1);
    }
    [field: SerializeField] public InventoryObject inventory {get;set;}
    public void SetSorting(int value)
    {
        switch ((SortingOption)value)
        {
            case (SortingOption.Weight):
                inventory.Sort((slot) => slot.item == null ? int.MinValue : slot.weight * slot.amount, SortingOrder.Desc);
                break;

            case (SortingOption.Value):
                inventory.Sort((slot) => slot.item == null ? int.MinValue : slot.value * slot.amount, SortingOrder.Desc);
                break;

            case (SortingOption.Type):
                inventory.Sort((slot) => slot.item == null ? int.MaxValue : (int)slot.item.type, SortingOrder.Asc);
                break;

        }
    }

}