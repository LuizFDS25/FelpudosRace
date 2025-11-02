using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffect = 1f;
    public float scrollSpeed = 2f;
    public bool moveRight = true;

    private float length;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Movimento automático do fundo
        float direction = moveRight ? 1f : -1f;
        float temp = (Time.time * scrollSpeed * parallaxEffect * direction) % length;

        // Muda a posição suavemente, mantendo o loop
        transform.position = new Vector3(startPos.x + temp, startPos.y, startPos.z);
    }
}
