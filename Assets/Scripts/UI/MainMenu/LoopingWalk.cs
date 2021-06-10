using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingWalk : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;
    [SerializeField] private Vector2 startEnd;
    [SerializeField] private float speed;

    [SerializeField] private Animator playerAnimator;

    private void Update()
    {
        Vector2 oPosition = objectTransform.position;

        oPosition.x -= speed * Time.deltaTime;
        objectTransform.position = oPosition;

        playerAnimator.SetFloat("Horizontal", 1);
        playerAnimator.SetFloat("Speed", 1);

        if (objectTransform.position.x <= startEnd.y)
        {
            Vector2 position = objectTransform.position;
            position.x = startEnd.x;

            objectTransform.position = position;
        }
    }
}
