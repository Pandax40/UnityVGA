using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int id;
    public GameObject BuyPlayer;
    public bool comprable;
    private bool onStay;
    private PoUpInfoManager manager;

    private void Start()
    {   
        manager = transform.parent.GetChild(1).GetComponent<PoUpInfoManager>();
        comprable = false;
    }
    private void Update()
    {
        if (!comprable)
        {
            transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetChild(2).GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (onStay && GameManager.Instance.BuyPerformed)
        {
            if(comprable)
            {
                GameManager.Instance.BuyItem(id);
                GameObject Sonido = Instantiate(BuyPlayer);
                Destroy(Sonido, 1f);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //llamar a funcion activate (num)
        manager.Activate(id);
        onStay = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //llamar a funcion stop
        manager.Deactivate();
        onStay = false;
    }
}
