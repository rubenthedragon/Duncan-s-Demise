using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTab : MonoBehaviour
{
    [field: SerializeField] public GameObject tab;
    [field: SerializeField] public string Name { get; set; }
}