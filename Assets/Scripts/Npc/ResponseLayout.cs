using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseLayout
{
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public Dialog Dialog { get; set; }
    [field: SerializeField] public Quest Quest { get; set; }
    [field: SerializeField] public bool OpenShop { get; set; } = false;
    [field: SerializeField] public bool KillPlayer { get; set; } = false;
    [field: SerializeField] public string GoToScene { get; set; }

}
