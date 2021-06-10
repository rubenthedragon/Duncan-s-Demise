using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class MapUI : MonoBehaviour
{
    [SerializeField] Image MapImage;

    private void Start()
    {
        Text mapTitle = this.GetComponentInChildren<Text>();
        mapTitle.text = MapImage.sprite.name;
    }

    private void OnEnable()
    {
        PauseGame(true);
    }

    private void OnDisable() => PauseGame(false);

    public void PauseGame(bool pause) => Time.timeScale = pause ? 0f : 1f;
}
