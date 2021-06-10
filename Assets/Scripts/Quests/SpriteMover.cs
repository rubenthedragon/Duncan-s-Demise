using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMover : MonoBehaviour
{
    [field: SerializeField] private Vector2 startPosition;
    [field: SerializeField] private SpriteRenderer renderer;

    private void Start()
    {
        startPosition = transform.localPosition;
        startPosition.y += 0.9f;
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * 1) * 0.08f + startPosition.y;

        transform.localPosition = new Vector2(transform.localPosition.x, y);
    }

    public void StartRoutine(Sprite sprite)
    {
        if(this != null)
        {
            renderer.sprite = sprite;
            transform.localPosition = startPosition;
        }
    }
    
    public void StopRoutine()
    {
        if (this != null)
        renderer.sprite = null;
    }
}
