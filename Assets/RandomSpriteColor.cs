using UnityEngine;

public class RandomSpriteColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color[] colors;

    private void Start()
    {
        if (spriteRenderer == null || colors.Length == 0) return;

        spriteRenderer.color = colors[Random.Range(0, colors.Length)];
    }

}