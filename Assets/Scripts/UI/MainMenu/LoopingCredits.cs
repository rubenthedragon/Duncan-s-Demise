using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingCredits : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;
    [SerializeField] private Vector2 startEnd;
    [SerializeField] private float speed;

    private void Update()
    {
        Vector2 oPosition = objectTransform.position;

        oPosition.y += speed * Time.deltaTime;
        objectTransform.position = oPosition;

        if (objectTransform.position.y >= startEnd.y)
        {
            Vector2 position = objectTransform.position;
            position.y = startEnd.x;

            objectTransform.position = position;
        }
    }
}
