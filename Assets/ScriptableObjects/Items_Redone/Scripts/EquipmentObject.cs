using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory_System_Redone/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public ItemArmorSubType armorSubType;
    public void Awake() 
    {
        type = ItemType.Armor;
    }

    public override void Use(Player player)
    {
        
    }

    public override string GetAdditionalInfo()
    {
        if (buffs.Length > 0)
        {
            return $"Armor: {this.buffs[0].Value}";
        }
        return "Armor: 0";
    }
}
