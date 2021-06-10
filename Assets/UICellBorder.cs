using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UICellBorder : MonoBehaviour
{
    [SerializeField] private Sprite normalBorderSprite;
    [SerializeField] private Sprite hightlightedBorderSprite;
    private Image borderImage;
    public bool IsHighlighted { get; private set; }

    private void Awake()
    {
        borderImage = GetComponent<Image>();
        borderImage.sprite = normalBorderSprite;
    }

    public void HighlightBorder(bool highlight) {
        IsHighlighted = highlight;
        if (highlight) {
            borderImage.sprite = hightlightedBorderSprite;
        }
        else {
            borderImage.sprite = normalBorderSprite;
        }
    }
}
