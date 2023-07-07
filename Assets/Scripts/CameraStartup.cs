using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Interfaz.gameObject.GetComponent<Canvas>().worldCamera = this.GetComponent<UnityEngine.Camera>();
        GameManager.Instance.Pause.GetComponent<Canvas>().worldCamera = this.GetComponent<UnityEngine.Camera>();
        GameManager.Instance.LoadingScreen.GetComponent<Canvas>().worldCamera = this.GetComponent<UnityEngine.Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
