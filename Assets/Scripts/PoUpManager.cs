using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PoUpManager : MonoBehaviour
{
    public GameObject[] PU = new GameObject[12];

    private GameObject[] cajas;
    public bool forest;

    // Start is called before the first frame update
    void Start()
    {
        cajas = new GameObject[3];
        int[] ids = new int[3];
        ids[0] = UnityEngine.Random.Range(0, 2);
        ids[1] = UnityEngine.Random.Range(3, 6);
        ids[2] = UnityEngine.Random.Range(7, 11);
        while (GameManager.Instance.BuyBuffer[ids[2] - 7])
            ids[2] = UnityEngine.Random.Range(7, 11);
        for (int i = 0; i < 2 + (!forest ? 1 : 0); ++i)
            cajas[i] = Instantiate(PU[ids[i]], transform.GetChild(0).GetChild(i).transform.position, quaternion.identity, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(cajas[0] != null) 
            cajas[0].GetComponent<PowerUp>().comprable = GameManager.Instance.Monedas >= 15;
        if (cajas[1] != null) 
            cajas[1].GetComponent<PowerUp>().comprable = GameManager.Instance.Monedas >= 25;
        if(!forest && cajas[2] != null) 
            cajas[2].GetComponent<PowerUp>().comprable = GameManager.Instance.Monedas >= 50;
    }
}
