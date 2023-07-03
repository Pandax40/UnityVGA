using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nubes2Anim : MonoBehaviour
{
    private SpriteRenderer nube;

    void Start()
    {
        nube = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        nube.size += new Vector2(2 * Time.deltaTime, 0);
    }
}
