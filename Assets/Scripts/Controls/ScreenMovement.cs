using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    [SerializeField] private Transform center;

    private void Update()
    {
        Camera.main.transform.LookAt(center);
    }
}
