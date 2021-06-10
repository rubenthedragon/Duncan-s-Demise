using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Text xpText;

    private Slider EXPSlider;

    void Start()
    {
        EXPSlider = GetComponent<Slider>();
        EXPSlider.maxValue = player.ExpToNext(player.Level);
        EXPSlider.value = player.CurrentExp;
        xpText.text = string.Format("XP {0}/{1}", player.CurrentExp, player.ExpToNext(player.Level));
        player.EXPChanged += ChangeEXP;
        player.LevelChanged += ChangeLevelEXP;
        DataControl.control.OnLoad += Load;
    }

    private void ChangeEXP(int expChange)
    {
        EXPSlider.value = expChange;
        xpText.text = string.Format("XP {0}/{1}", player.CurrentExp, player.ExpToNext(player.Level));
    }

    private void ChangeLevelEXP(int levelChange)
    {
        EXPSlider.value = player.CurrentExp;
        EXPSlider.maxValue = player.ExpToNext(levelChange);
        xpText.text = string.Format("XP {0}/{1}", player.CurrentExp, player.ExpToNext(player.Level));
    }

    private void ResetEXP(int currentEXP, int level)
    {
        EXPSlider.value = currentEXP;
        EXPSlider.maxValue = player.ExpToNext(level);
        xpText.text = string.Format("XP {0}/{1}", player.CurrentExp, player.ExpToNext(player.Level));
    }

    public void Load()
    {
        ResetEXP(DataControl.control.CurrentEXP, DataControl.control.Level);
    }
}
