using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private const int visibilitiDistance = 30;
    private Transform playerT;
    private bool isVisible;
    private Vector3 position;
    void Start()
    {
        playerT = GameObject.Find("Player").transform;
        position = transform.position;
        isVisible = true;
    }

    void Update()
    {
        
    }
}
