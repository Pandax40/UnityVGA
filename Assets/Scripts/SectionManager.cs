using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    private Transform[] sectionTransforms;
    // Start is called before the first frame update
    void Start()
    {
        sectionTransforms = GetComponentsInChildren<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
