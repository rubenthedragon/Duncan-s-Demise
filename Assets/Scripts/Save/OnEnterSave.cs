using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnterSave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == GameObject.Find("Player").GetComponent<CircleCollider2D>())
        {
            DataControl.control.Save();
            ShowSavingText();
        }
    }
    public void ShowSavingText()
    {
        if (this != null)
        {
            GameObject.Find("Saving text").GetComponent<Text>().text = "Checkpoint reached, saving progress...";
            Invoke("DisableSavingText", 1);
        }
    }

    public void DisableSavingText()
    {
        GameObject.Find("Saving text").GetComponent<Text>().text = "";
    }
}