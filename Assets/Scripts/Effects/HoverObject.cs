using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverObject : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float height;

    private float startY;

    private void Start() => startY = transform.position.y;

    private void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * height + startY;

        transform.position = new Vector2(transform.position.x, y);
    }
}
