using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Sprite profileNormal;
    [SerializeField] private Sprite profileDamage;

    private Image profile;

    void Start()
    {
        profile = GetComponent<Image>();
        profile.sprite = profileNormal;
        player.HealthChanged += ChangeProfile;
    }

    /// <summary>
    /// Change player profile based on the hp
    /// </summary>
    /// <param name="hp"></param>
    private void ChangeProfile(int hp)
    {
        if (player.HP <= player.MaxHP * 0.3)
        {
            profile.sprite = profileDamage;
        }
        else
        {
            profile.sprite = profileNormal;
        }
    }
}
