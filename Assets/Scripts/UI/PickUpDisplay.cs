using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpDisplay : MonoBehaviour
{
    [SerializeField] private Text displayText;

    public void Display(string text, ItemRarity itemRarity)
    {
        displayText.text = text;

        switch (itemRarity)
        {
            case ItemRarity.Common:
                displayText.color = Color.white;
                break;
            case ItemRarity.Uncommon:
                displayText.color = Color.green;
                break;
            case ItemRarity.Rare:
                displayText.color = Color.blue;
                break;
            case ItemRarity.Epic:
                displayText.color = Color.magenta;
                break;
            case ItemRarity.Legendary:
                displayText.color = new Color32(252, 161, 3, 255);
                break;
            default:
                break;
        }

    }
}
