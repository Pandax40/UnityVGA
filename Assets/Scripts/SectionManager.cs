using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{
    private Transform[][] sectionTransforms;
    private Transform[][] platformsColiders;
    private int matrixRow = 2;
    private int matrixColumn = 4;

    //TODO:
    // - Distancia maxima entre los caminos de la pared. Minimo tener un camino posible.

    // Start is called before the first frame update
    void Start()
    {
        FillMatrix();
        FillTransforms();
        FixPlatform(true);
       


        /*for (int i = 1; i < matrixRow; i++)
        {
            for (int j = 0; j < matrixColumn; j++)
            {
                Vector3 originPlatform = platformsColiders[i][j].transform.position;
                Vector3 targetPlatform = platformsColiders[0][j+1].transform.position;
            }
        }*/
            
    }

    private void FixPlatform(bool debugInfo)
    {
        int indexHightCheck = Random.Range(0, matrixColumn - 1);
        Vector3 posDownPlatform = platformsColiders[1][indexHightCheck].transform.position;
        Vector3 posUpPlatform = platformsColiders[0][indexHightCheck].transform.position;
        Vector3 distance = posDownPlatform - posUpPlatform;
        if (distance.magnitude > 5.8f && debugInfo) Debug.Log("Corrigiendo Plataforma: " + indexHightCheck);
        while (distance.magnitude > 5.8f)
        {
            posDownPlatform.y += 0.2f;
            posUpPlatform.y -= 0.2f;
            distance = posDownPlatform - posUpPlatform;
        }
        platformsColiders[1][indexHightCheck].transform.position = posDownPlatform;
        platformsColiders[0][indexHightCheck].transform.position = posUpPlatform;
    }

    private void FillTransforms()
    {
        platformsColiders = new Transform[matrixRow][];

        for (int i = 0; i < matrixRow; i++)
            platformsColiders[i] = new Transform[matrixColumn];

        for (int i = 0; i < matrixRow; i++)
            for (int j = 0; j < matrixColumn; j++)
                platformsColiders[i][j] = sectionTransforms[i][j].GetComponent<PlatformGenerator>().GeneratePlatform();
    }

    private void FillMatrix()
    {
        Transform[] auxVector = GetComponentsInChildren<Transform>();
        sectionTransforms = new Transform[matrixRow][];
        for (int i = 0; i < matrixRow; i++)
            sectionTransforms[i] = new Transform[matrixColumn];

        for (int i = 0; i < matrixRow; i++)
            for (int j = 0; j < matrixColumn; j++)
                sectionTransforms[i][j] = auxVector[2 + j + (matrixColumn+1) * i];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
