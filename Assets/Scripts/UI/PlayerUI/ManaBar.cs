using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ManaBar : MonoBehaviour, IOnLoadAndSave
{
    [SerializeField] private Player player;

    [SerializeField] private Text manaText;

    [SerializeField] private Slider manaSlider;

    private void Start()
    {
        manaSlider = GetComponent<Slider>();
        manaSlider.maxValue = player.MaxMana;
        manaSlider.value = player.Mana;
        manaText.text = string.Format("MP {0}/{1}", player.Mana, player.MaxMana);
        player.ManaChanged += ChangeMana;
        player.LevelChanged += ChangeLevelMana;
    }

    /// <summary>
    /// Displays the current mana on the manabar
    /// </summary>
    /// <param name="mana"> The current mana of the entity</param>
    private void ChangeMana(int mana)
    {
        manaSlider.maxValue = player.MaxMana;
        manaSlider.value += mana;
        manaText.text = string.Format("MP {0}/{1}", player.Mana, player.MaxMana);
    }

    private void ChangeLevelMana(int level)
    {   
        manaSlider.maxValue = player.MaxMana;
        manaSlider.value = player.Mana;
        manaText.text = string.Format("MP {0}/{1}", player.Mana, player.MaxMana);
    }

    private void ResetMana(int mana, int maxMana)
    {
        if (manaSlider)
        {
            manaSlider.maxValue = maxMana;
            manaSlider.value = mana;
            manaText.text = string.Format("MP {0}/{1}", mana, maxMana);
        }
    }

    public void Load()
    {
        ResetMana(DataControl.control.Mana, DataControl.control.MaxMana);
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
