using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_Renderer;
    private int health = 200;

    public void TakeDamage(int value)
    {
        health -= value;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        m_Renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        m_Renderer.material.color = Color.white;
    }
}
