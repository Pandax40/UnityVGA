using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInfo : MonoBehaviour
{
    [SerializeField] private int id;

    private PoUpInfoManager manager;

    private void Start()
    {
        manager = transform.parent.GetChild(1).GetComponent<PoUpInfoManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //llamar a funcion activate (num)
        manager.Activate(id);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //llamar a funcion stop
        manager.Deactivate();
    }
}
