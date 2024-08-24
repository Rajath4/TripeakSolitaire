using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardGridGenerator : MonoBehaviour
{
    public GameObject cardPrefab; // Assign in Unity Inspector
    public Transform[] cardPositions; // Assign each card position in Unity Inspector

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        foreach (Transform position in cardPositions)
        {
            // Instantiate(cardPrefab, position.position, Quaternion.identity, position);
                   Instantiate(cardPrefab);

        }
    }
}
