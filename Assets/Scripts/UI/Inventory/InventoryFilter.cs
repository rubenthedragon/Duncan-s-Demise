using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class InventoryFilter : MonoBehaviour
{
    [SerializeField] private ItemType filterItemType;
    [SerializeField] private InventoryObject inventory;
    
    private void Awake(){
        Toggle toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate{Filter(toggle.isOn);});
    }

    private void Filter(bool filterEnabled){
        foreach (InventorySlot slot in inventory.Container.items)
        {
            inventory.activeFilterType =    (filterEnabled) ? 
                                                filterItemType : 
                                                (inventory.activeFilterType == filterItemType) ? 
                                                    null : 
                                                    inventory.activeFilterType;
            if(filterEnabled && slot.item?.type == filterItemType) continue;
            slot.filteredOut = filterEnabled;
        }
    }
}
