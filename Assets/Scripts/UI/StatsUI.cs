using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Text VitalityNumber;
    [SerializeField] private Text MaxHPNumber;
    [SerializeField] private Text WisdomNumber;
    [SerializeField] private Text MaxManaNumber;
    [SerializeField] private Text StrengthNumber;
    [SerializeField] private Text DexterityNumber;
    [SerializeField] private Text IntelligenceNumber;
    [SerializeField] private Text StatPointsNumber;
    [SerializeField] private Button VitalityButton;
    [SerializeField] private Button WisdomButton;
    [SerializeField] private Button StrengthButton;
    [SerializeField] private Button DexterityButton;
    [SerializeField] private Button IntelligenceButton;

    private void Start()
    {
        UpdateStats();
    }

    private void Update()
    {
        UpdateStats();
        UpdateButtons();
    }

    private void UpdateStats()
    {
        VitalityNumber.text = player.Vitality.ToString();
        MaxHPNumber.text = player.MaxHP.ToString();
        WisdomNumber.text = player.Wisdom.ToString();
        MaxManaNumber.text = player.MaxMana.ToString();
        StrengthNumber.text = player.Strength.ToString();
        DexterityNumber.text = player.Dexterity.ToString();
        IntelligenceNumber.text = player.Intelligence.ToString();
        StatPointsNumber.text = player.StatPoints.ToString();
    }

    private void UpdateButtons()
    {
        if (player.StatPoints > 0)
        {
            VitalityButton.interactable = true;
            WisdomButton.interactable = true;
            StrengthButton.interactable = true;
            DexterityButton.interactable = true;
            IntelligenceButton.interactable = true;
        }
        else
        {
            VitalityButton.interactable = false;
            WisdomButton.interactable = false;
            StrengthButton.interactable = false;
            DexterityButton.interactable = false;
            IntelligenceButton.interactable = false;
        }
    }

    public void IncreaseVitality()
    {
        player.IncreaseVitality();
    }

    public void IncreaseWisdom()
    {
        player.IncreaseWisdom();
    }

    public void IncreaseStrength()
    {
        player.IncreaseStrength();
    }

    public void IncreaseDexterity()
    {
        player.IncreaseDexterity();
    }

    public void IncreaseIntelligence()
    {
        player.IncreaseIntelligence();
    }
}
