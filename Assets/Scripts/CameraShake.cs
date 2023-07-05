using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeFrequency;
    public float ShakeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera() 
    {
        transform.position += new Vector3(0, Mathf.Sin(Time.time * ShakeSpeed) * ShakeFrequency, 0);
    }
}
