using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScreen()
    {
        coinText.text = GameManager.Instance.Health.ToString();
    }
}
