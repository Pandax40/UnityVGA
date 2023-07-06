using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class SectionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Prefabs;

    public static AudioSource AudioSource;

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
        Queue<GameObject> prefabs = GenerateQueue();
        GenerateTransforms(prefabs);
        FixPlatform(true);
        Platform.Probability = GameManager.Instance.Probability;
        Debug.Log("Actual Coin Probability Spawn: " + GameManager.Instance.Probability);
        GameManager.SpawnPos = GetSpawnPos();
    }

    private Queue<GameObject> GenerateQueue() 
    {
        Queue<GameObject> queuePrefabsRandom = new Queue<GameObject>();
        int maxIndex = Prefabs.Length;
        for (int i = 0; i < matrixColumn*matrixRow; i++)
        {
            int index = Random.Range(0, maxIndex);
            queuePrefabsRandom.Enqueue(Prefabs[index]);
        }
        return queuePrefabsRandom;
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

    private void GenerateTransforms(Queue<GameObject> gameObjects)
    {
        platformsColiders = new Transform[matrixRow][];

        for (int i = 0; i < matrixRow; i++)
            platformsColiders[i] = new Transform[matrixColumn];

        for (int i = 0; i < matrixRow; i++)
            for (int j = 0; j < matrixColumn; j++)
            {
                PlatformGenerator platform = sectionTransforms[i][j].GetComponent<PlatformGenerator>();
                platformsColiders[i][j] = platform.GeneratePlatform(gameObjects.Dequeue());
            }   
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

    public Vector3 GetSpawnPos()
    {
        int index = Random.Range(0, matrixColumn);
        return platformsColiders[0][index].position + new Vector3(0, 1.8f, 0);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
