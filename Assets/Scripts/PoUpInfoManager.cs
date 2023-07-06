using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoUpInfoManager : MonoBehaviour
{
    List<string> info = new List<string>()
    {
        "Coins may spawn as hearts. (1 Round)",
        "Get x2 coins. (1 Round)",
        "Heal 1 heart.",
        "+1 NON-permanent heart.",
        "Heal all your hearts.",
        "Jump higher. (1 Round)",
        "Extra velocity. (1 Round)",
        "Doble jump permanent.",
        "Doble wall jump permanent.",
        "+1 permanent heart.",    
        "Jump higher permanent.",
        "Extra velocity permanent."
    };

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Activate(int id)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = info[id];
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void NextScene()
    {
        GameManager.Instance.ShopStop();
    }
}
