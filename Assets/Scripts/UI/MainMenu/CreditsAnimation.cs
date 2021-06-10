using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAnimation : MonoBehaviour
{
    public event Action ReachedEnd;

    [SerializeField] private GameObject creditScreen;
    [SerializeField] private GameObject credits;
    [SerializeField] private Vector2 startEnd;
    [SerializeField] private float speed;

    private void Update()
    {
        Vector2 oPosition = credits.transform.localPosition;

        oPosition.y += speed * Time.deltaTime;
        credits.transform.localPosition = oPosition;

        if (credits.transform.localPosition.y >= startEnd.y)
        {
            Vector2 position = credits.transform.localPosition;
            position.y = startEnd.x;

            credits.transform.localPosition = position;

            ReachedEnd?.Invoke();
            creditScreen.SetActive(false);
        }
    }
}
