using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatIncreaseTag : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private Text statIncreaseTag;

    [SerializeField]
    private Vector2 startPosition;

    [SerializeField]
    private Vector2 target;

    void Start()
    {
        statIncreaseTag = GetComponent<Text>();
        player.StatsChanged += StatChange;
    }

    public void StatChange(string statsIncrease)
    {
        StartCoroutine(MoveText(statsIncrease));
    }

    private IEnumerator MoveText(string statsIncrease)
    {
        statIncreaseTag.text = statsIncrease;
        transform.localPosition = startPosition;
        yield return ChangeAlpha(1);

        while (Vector2.Distance(transform.localPosition, target) > 0.1f)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, 1 * Time.deltaTime);
            yield return null;
        }

        yield return ChangeAlpha(0);
    }

    private IEnumerator ChangeAlpha(float a)
    {
        Color c = statIncreaseTag.color;
        c.a = a;
        statIncreaseTag.color = c;
        yield return null;
    }
}