using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public float ShakeSpeed; //velocidad a la que shakea
    public float ShakeFrequency; //frequencia 
    private TextMeshProUGUI coinText;
    private GameObject[] hearths;
    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponentInChildren<TextMeshProUGUI>();
        hearths = new GameObject[4];
        for(int i = 0; i < 4; ++i)
        hearths[i] = transform.GetChild(i).gameObject;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = GameManager.Instance.Monedas.ToString();
    }

    public void ShakeHearts() //funcion para agitar corazones, hace falta que se llame varias veces cuando el player reciba daño, antes de llamarla habria que almacenar la 
                              //posición original para que solo vibre y al acabar no se modifique su posición
    {
        transform.GetChild(0).position += new Vector3(0, Mathf.Sin(Time.time * ShakeSpeed) * ShakeFrequency, 0); //shake en vertical
    }

    public void ShakeCoins() //same pero para monedas (se llama cuando compres)
    {   
        float shaketime = Time.time; //para que shakeen lo mismo, no vaya a ser que cambie el time entre la ejecución
        transform.GetChild(1).position += new Vector3(0, Mathf.Sin(shaketime * ShakeSpeed) * ShakeFrequency, 0);//sprite moneda
        transform.GetChild(2).position += new Vector3(0, Mathf.Sin(shaketime * ShakeSpeed) * ShakeFrequency, 0);//coincounter
    }
}
