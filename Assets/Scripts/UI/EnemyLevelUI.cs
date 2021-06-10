using UnityEngine;
using UnityEngine.UI;

public class EnemyLevelUI : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    void Start()
    {
        GetComponent<Text>().text = enemy.Level.ToString();
    }
}
