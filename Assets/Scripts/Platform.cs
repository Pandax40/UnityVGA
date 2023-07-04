using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Player.transform.position.y <= transform.position.y + 1.675f) collider.enabled = false;
        else collider.enabled = true;
    }
}
