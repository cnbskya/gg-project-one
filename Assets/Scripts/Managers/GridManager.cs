using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private Camera mainCam;

    [Header("UI Props")]
    [SerializeField] TMP_Text matchCounterText;
    [SerializeField] TMP_InputField gridSizeText;
    private int matchCount;

    [Header("Grid Generation Props")]
    [SerializeField] private Transform gridHolder;

    [SerializeField] private GameObject gridPointPrefab;
    [SerializeField] private int gridSize = 5;
    [SerializeField] private float distanceBetweenPoints = 1f;

    private List<GridPart> allGridParts = new List<GridPart>();

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

    public void Rebuild()
    {
        int size = int.Parse(gridSizeText.text);
        Debug.Log("SIZE: " + size);
        gridSize = size;
        
        DestroyAllGridPart();
        GenerateGrid();
    }

    private void DestroyAllGridPart()
    {
        for (int i = allGridParts.Count - 1; i >= 0; i--)
        {
            GridPart gridPart = allGridParts[i];
            allGridParts.Remove(gridPart);
            Destroy(gridPart.gameObject);
        }
    }
    
    public void CheckAllGridPartSelectionCondition()
    {
        List<GridPart> completedGridParts = GetCompletedGridParts();

        if (completedGridParts.Count <= 0) return;

        foreach (var gridPart in completedGridParts)
        {
            List<GridPart> gridPartSelectedNeighbours = gridPart.GetSelectedNeighbourList();

            gridPart.SelectedToggle(false);
            foreach (var selectedGridPart in gridPartSelectedNeighbours)
                selectedGridPart.SelectedToggle(false);
        }

        IncreaseMatchCounter(1);
    }

    private List<GridPart> GetCompletedGridParts()
    {
        List<GridPart> completedGridParts = new List<GridPart>();

        foreach (var gridPart in allGridParts)
        {
            bool isCompleted = gridPart.CheckNeighboursSelectedCondition();
            if (isCompleted) completedGridParts.Add(gridPart);
        }

        return completedGridParts;
    }

    private void SetOrthographicCameraPosAndSize()
    {
        Vector3 gridCenter = GetGridCenter();
        mainCam.transform.position = new Vector3(gridCenter.x, 20, gridCenter.z);
        mainCam.orthographicSize = (gridSize * distanceBetweenPoints) + 1;
    }

    #region Helper Funcs

    private void IncreaseMatchCounter(int increaseAmount)
    {
        matchCount += increaseAmount;

        UpdateMatchCounterText();
    }

    private void UpdateMatchCounterText()
    {
        matchCounterText.text = matchCount.ToString();
    }

    private Vector3 GetGridCenter()
    {
        float gridOffset = (gridSize - 1) * distanceBetweenPoints / 2f;
        Vector3 gridCenter = new Vector3(gridOffset, 0f, gridOffset);

        return gridCenter;
    }

    #endregion
}