using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossMeter : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private GameObject bossMeterContainer;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Text nameText;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private float bossMusicVolume;
    [SerializeField] private AudioClip currentAreaMusic;
    [SerializeField] private float AreaMusicVolume;

    private void Start()
    {
        nameText.text = enemy.name.Replace("(Clone)", ""); 
        bossMeterContainer.SetActive(false);
    }

    private void Update() => CheckIfPlayerIsInRange();

    private void CheckIfPlayerIsInRange()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(enemy.transform.position, range).Where(c => c.gameObject.CompareTag("Player")).ToArray();
        bossMeterContainer.SetActive(cols.Length > 0);
    }

    /// <summary>
    /// Draws debug wires to display the range of certain attributes
    /// NOTE: This is only visible in the editor when selecting the object
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(enemy.transform.position, range);
    }

    private void OnEnable()
    {
        if (bossMusic != null)
            AudioPlayer.Audioplayer.PlayMusic(bossMusic, bossMusicVolume);
    }

    private void OnDisable()
    {
        if (currentAreaMusic != null)
        {
            AudioPlayer.Audioplayer.PlayMusic(currentAreaMusic);
        }
        else
        {
            AudioPlayer.Audioplayer.StopMusic();
        }
    }
}