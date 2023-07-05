using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void Button()
    {
        gameObject.SetActive(false);
        GameManager.Instance.LoadScene(0);
    }
}
