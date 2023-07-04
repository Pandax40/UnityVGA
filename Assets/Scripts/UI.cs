using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    private TextMeshPro coinText;
    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponent<TextMeshPro>();
        UpdateScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScreen()
    {
        coinText.text = GameManager.Instance.Monedas.ToString();
    }
}
