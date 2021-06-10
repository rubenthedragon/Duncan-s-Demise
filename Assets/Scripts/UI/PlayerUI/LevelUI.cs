using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour, IOnLoadAndSave
{
    [SerializeField] private Player player;
    [SerializeField] private Text levelText;

    void Start()
    {
        levelText.text = string.Format("Lv {0}", player.Level);
        player.LevelChanged += ChangeLevel;
    }

    /// <summary>
    /// Change the level number on the card
    /// </summary>
    /// <param name="levelChange">the new level</param>
    private void ChangeLevel(int levelChange)
    {
        levelText.text = string.Format("Lv {0}", levelChange);
    }

    public void Load()
    {
        ChangeLevel(DataControl.control.Level);
    }

    public void Save()
    {
        
    }

    public void OnEnable()
    {
        DataControl.control.OnLoad += Load;
    }

    public void OnDisable()
    {
        DataControl.control.OnLoad -= Load;
    }
}
