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

        for (int i = 1; i < matrixRow; i++)
        {
            for (int j = 0; j < matrixColumn; j++)
            {
                Vector3 originPlatform = platformsColiders[i][j].transform.position;
                Vector3 targetPlatform = platformsColiders[0][j+1].transform.position;
            }
        }
            
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
