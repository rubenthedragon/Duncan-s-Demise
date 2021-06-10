using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    [field: SerializeField] public DialogLayout[] dialog { get; set; }
    [field: SerializeField] public ResponseLayout[] Response { get; set; }
}