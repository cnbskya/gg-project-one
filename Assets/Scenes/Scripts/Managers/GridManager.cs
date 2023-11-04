using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [Header("All Grid Parts")]
    public List<GridPart> allGridParts = new List<GridPart>();

    [SerializeField] private Camera mainCam;

    [Header("Grid Prop Values")]
    [SerializeField] private Transform gridHolder;
    [SerializeField] private GameObject gridPointPrefab;
    [SerializeField] private int gridSize = 5;
    [SerializeField] private float distanceBetweenPoints = 1f;

    private void Awake()
    {
        Instance = this;
        GenerateGrid();
    }
    
    void GenerateGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 spawnPosition = new Vector3(i * distanceBetweenPoints, 0f, j * distanceBetweenPoints);
                GameObject spawnedGridObj = Instantiate(gridPointPrefab, spawnPosition, Quaternion.identity);
                GridPart spawnedGridPart = spawnedGridObj.GetComponent<GridPart>();
                
                spawnedGridObj.transform.SetParent(gridHolder);
                ListHelper.AddToList(spawnedGridPart, allGridParts);
            }
        }

        SetOrthographicCameraPosAndSize();
    }

    public void CheckAllGridPartSelectionCondition()
    {
        foreach (var gridPart in allGridParts)
        {
            gridPart.CheckNeighboursSelectedCondition();
        }
    }
    
    private void SetOrthographicCameraPosAndSize()
    {
        Vector3 gridCenter = GetGridCenter();
        mainCam.transform.position = new Vector3(gridCenter.x, 20, gridCenter.z);
        mainCam.orthographicSize = (gridSize * distanceBetweenPoints) + 1;
    }

    #region Helper Funcs

    private Vector3 GetGridCenter()
    {
        float gridOffset = (gridSize - 1) * distanceBetweenPoints / 2f;
        Vector3 gridCenter = new Vector3(gridOffset, 0f, gridOffset);

        return gridCenter;
    }

    #endregion
}