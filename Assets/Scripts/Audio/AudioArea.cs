using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioArea : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volume;
    [SerializeField] private string areaName;

    private void Awake()
    {
        if (this.gameObject.GetComponent<BoxCollider2D>())
        {
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(((RectTransform)transform).rect.width, ((RectTransform)transform).rect.height);
            this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            if (this.gameObject.GetComponent<BoxCollider2D>() && this.gameObject.GetComponent<BoxCollider2D>().bounds.Contains(GameObject.Find("Player").transform.position))
            {
                if (clip != null)
                {
                    AudioPlayer.Audioplayer.PlayMusic(clip, volume);
                }
                else
                {
                    AudioPlayer.Audioplayer.StopMusic();
                }
            }
        }
        else
        {
            if (this.gameObject.GetComponent<PolygonCollider2D>() && this.gameObject.GetComponent<PolygonCollider2D>().bounds.Contains(GameObject.Find("Player").transform.position))
            {
                if (clip != null)
                {
                    AudioPlayer.Audioplayer.PlayMusic(clip, volume);
                }
                else
                {
                    AudioPlayer.Audioplayer.StopMusic();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == GameObject.Find("Player").GetComponent<CircleCollider2D>())
        {
            GameObject.FindObjectOfType<Player>().QuestProgressionMade(areaName);
            
            Discord.Activity updatedActivity = DiscordRPCScript.Instance.MainActivity;
            updatedActivity.State = $"Now in {areaName}";
            DiscordRPCScript.Instance.UpdateMainActivity(updatedActivity);
            
            if(clip != null)
            {
                AudioPlayer.Audioplayer.PlayMusic(clip, volume);
            }
            else
            {
                AudioPlayer.Audioplayer.StopMusic();
            }
        }
    } 
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == GameObject.Find("Player").GetComponent<CircleCollider2D>())
        {
            AudioPlayer.Audioplayer.StopMusic();
        }
    }
}
