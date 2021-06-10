using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit this boi if you need to do something onload
/// </summary>
public interface IOnLoadAndSave
{
    void Save();
    void Load();
    void OnEnable();
    void OnDisable();
}
