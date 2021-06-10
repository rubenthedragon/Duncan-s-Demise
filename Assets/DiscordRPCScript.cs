using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class DiscordRPCScript : MonoBehaviour
{
    public static DiscordRPCScript Instance { get; set; }

    private Discord.Discord discord;

    private Discord.ActivityManager actManager;

    public Discord.Activity MainActivity {get;set;}

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start(){
        discord = new Discord.Discord(806896004980539442, (ulong)Discord.CreateFlags.NoRequireDiscord);
        actManager = discord.GetActivityManager();
        
        MainActivity = new Discord.Activity{
            State = "Playing Duncan's demise",
            Assets = new Discord.ActivityAssets{
                LargeImage = "duncanhead",
                LargeText = "Duncan's demise"
            },
            Timestamps = new Discord.ActivityTimestamps{
                Start = DateTimeOffset.Now.ToUnixTimeSeconds()
            }
        };

        UpdateMainActivity(MainActivity);
    }

    public void UpdateMainActivity(Discord.Activity updatedActivity){
        MainActivity = updatedActivity;
        actManager.UpdateActivity(MainActivity, (res) => {});
    }

    private void Update(){
        discord.RunCallbacks();
    }

    private void OnApplicationQuit(){
        discord.Dispose();
        Destroy(this);
    }
}