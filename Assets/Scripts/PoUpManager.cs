using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class PoUpManager : MonoBehaviour
{
    public GameObject[] PU = new GameObject[12];

    private int r;

    public bool forest;

    // Start is called before the first frame update
    void Start()
    {
        r = UnityEngine.Random.Range(0, 3);
        Instantiate(PU[r], transform.GetChild(0).GetChild(0).transform.position, quaternion.identity, this.transform);

        r = UnityEngine.Random.Range(3, 7);
        Instantiate(PU[r], transform.GetChild(0).GetChild(1).transform.position, quaternion.identity, this.transform);

        if (!forest)
        {
            r = UnityEngine.Random.Range(7, 12); 
            Instantiate(PU[r], transform.GetChild(0).GetChild(2).transform.position, quaternion.identity, this.transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
