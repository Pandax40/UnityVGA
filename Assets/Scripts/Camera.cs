using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Interfaz.GetComponent<Canvas>().worldCamera = this.GetComponent<UnityEngine.Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
