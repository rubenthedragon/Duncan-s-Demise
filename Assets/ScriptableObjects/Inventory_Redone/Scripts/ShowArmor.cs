using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowArmor : MonoBehaviour
{
    [SerializeField] Text ArmorText;
    [SerializeField] Text DmgReductionText;
    private Player player;
    private float defense;

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.LevelChanged += ShowArmorInText;
    }

    void Update()
    {
        if (defense != player.Defense)
        {
            ShowArmorInText(player.Level);
        }
    }

    public void ShowArmorInText(int level)
    {
        defense = player.Defense;
        ArmorText.text = defense.ToString();
        DmgReductionText.text = Mathf.RoundToInt((defense / (defense + 700f + (85f * level))) * 100).ToString() + "%";
    }

}
