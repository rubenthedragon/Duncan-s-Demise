using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelTag : MonoBehaviour
{
    [SerializeField] private Player player;

    private Text levelUpTag;

    void Start()
    {
        levelUpTag = GetComponent<Text>();
        Color c = levelUpTag.color;
        c.a = 0;
        levelUpTag.color = c;
        player.LevelChanged += LevelUp;
    }

    public void LevelUp(int level)
    {
        StartCoroutine(FlashingText());
    }

    private IEnumerator FlashingText()
    {
        yield return ChangeAlpha(1);
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                yield return ChangeColor(Color.white);
            }
            else
            {
                yield return ChangeColor(Color.yellow);
            }
        }
        yield return ChangeAlpha(0);
    }

    private IEnumerator ChangeColor(Color c)
    {
        levelUpTag.color = c;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator ChangeAlpha(float a)
    {
        Color c = levelUpTag.color;
        c.a = a;
        levelUpTag.color = c;
        yield return null;
    }
}
