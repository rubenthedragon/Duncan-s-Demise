using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour, IOnLoadAndSave
{
    [SerializeField] private Entity entity;

    [SerializeField] private Text hpText;

    private Slider healthSlider;

    private void Start()
    {
        healthSlider = this.gameObject.GetComponent<Slider>();
        healthSlider.maxValue = entity.MaxHP;
        healthSlider.value = entity.HP;
        hpText.text = string.Format("HP {0}/{1}", entity.HP, entity.MaxHP);
        entity.HealthChanged += ChangeHealth;
        if (entity.GetComponent<Player>() != null)
        {
            entity.GetComponent<Player>().LevelChanged += ChangeLevelHP;
        }
    }

    /// <summary>
    /// Displays the current health on the hpbar
    /// </summary>
    /// <param name="healthChange"> The change in health</param>
    private void ChangeHealth(int healthChange)
    {
        healthSlider.maxValue = entity.MaxHP;
        healthSlider.value += healthChange;
        hpText.text = string.Format("HP {0}/{1}", entity.HP, entity.MaxHP);
    }

    private void ChangeLevelHP(int level)
    {
        healthSlider.maxValue = entity.MaxHP;
        healthSlider.value = entity.HP;
        hpText.text = string.Format("HP {0}/{1}", entity.HP, entity.MaxHP);
    }

    private void ResetHealth(int health, int maxHealth)
    {
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            hpText.text = string.Format("HP {0}/{1}", health, maxHealth);
        }
    }
    public void Save()
    {
        
    }

    public void Load()
    {
        if (entity != null)
        {
            ResetHealth(DataControl.control.Health, DataControl.control.MaxHealth);
        }
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