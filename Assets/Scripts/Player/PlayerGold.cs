using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGold : MonoBehaviour
{
    private Player player;

    private void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        GetComponent<Text>().text = player.Gold.ToString();
    }

    public void UpdateGoldText()
    {
        GetComponent<Text>().text = player.Gold.ToString();
    }
}
