using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public float ShakeAmplitude;
    public float ShakeFrequency;
    private TextMeshProUGUI coinText;
    private GameObject[] hearts;
    private float shakeTimer;
    private float shakeCoins;
    private Vector3 auxCoinPos;
    private Vector3 auxHeartPos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        coinText = GetComponentInChildren<TextMeshProUGUI>();
        auxCoinPos = transform.GetChild(1).GetComponent<RectTransform>().localPosition;
        auxHeartPos = transform.GetChild(0).GetComponent<RectTransform>().localPosition;
        hearts = new GameObject[4];
        for(int i = 0; i < 4; ++i)
            hearts[i] = transform.GetChild(0).GetChild(i).gameObject;
        UpdateHearts();
    }

    // Update is called once per frame
    private void Update()
    {
        if(shakeTimer > 0f)
        {
            Vector3 pos = new Vector3(0, Mathf.Sin(Time.time * ShakeFrequency) * ShakeAmplitude, 0); //shake en vertical
            transform.GetChild(0).GetComponent<RectTransform>().localPosition = pos + auxHeartPos;
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
                transform.GetChild(0).GetComponent<RectTransform>().localPosition = auxHeartPos;
                
        }
        if(shakeCoins > 0f)
        {
            Vector3 pos = new Vector3(0, Mathf.Sin(Time.time * ShakeFrequency) * ShakeAmplitude, 0);
            transform.GetChild(1).GetComponent<RectTransform>().localPosition = pos + auxCoinPos; //shake en vertical
            shakeCoins -= Time.deltaTime;
            if (shakeCoins <= 0f)
            {
                transform.GetChild(1).GetComponent<RectTransform>().localPosition = auxCoinPos;
            }
                
        }
    }
    public void UpdateCoins()
    {
        coinText.text = GameManager.Instance.Monedas.ToString();
    }

    public void UpdateHearts()
    {
        bool extraHeart = GameManager.Instance.GetPropertys.extraHeart;
        int numHearts = GameManager.Instance.GetPropertys.hearts;
        hearts[3].SetActive(extraHeart);
        for (int i = 0; i < (3 + (extraHeart ? 1 : 0)); ++i)
        {
            if (numHearts > 0)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(true);
                --numHearts;
            }
            else
                hearts[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ShakeHearts(float timer)
    {
        shakeTimer = timer;
    }

    public void ShakeCoins(float timer) //same pero para monedas (se llama cuando compres)
    {
        shakeCoins = timer;
    }
}
