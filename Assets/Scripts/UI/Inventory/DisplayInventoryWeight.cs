using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayInventoryWeight : MonoBehaviour
{
    public GameObject player;
    public InventoryObject[] inventories;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = Weight();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Text>().text = Weight();
    }

    private string Weight()
    {
        Player p = player.GetComponent<Player>();

        int weight = inventories.Sum(i => i.weight);
        return weight + " / " + p.CarryStrength + " KG";
    }
}
