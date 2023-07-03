using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nubes : MonoBehaviour
{
    private SpriteRenderer nube;
    [SerializeField] private int Speed;

    void Start()
    {
        nube = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        nube.size += new Vector2(Speed * Time.deltaTime, 0);
    }
}
