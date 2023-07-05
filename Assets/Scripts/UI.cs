using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
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
}
