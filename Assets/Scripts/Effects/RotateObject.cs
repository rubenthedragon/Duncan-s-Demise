using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float _speed;
    private void Update()
    {
        transform.Rotate(Vector3.back * _speed);
    }
}
