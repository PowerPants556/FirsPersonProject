using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    [SerializeField] private AIPathFinder findPath;

    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Player":
                findPath.Explodion();
                break;
            case "Block":
                findPath.Jump();
                break;
        }
    }

}
