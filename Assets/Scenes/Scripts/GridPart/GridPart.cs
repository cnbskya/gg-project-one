using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPart : MonoBehaviour, IClickable
{
    public bool isSelected;

    [SerializeField] private GameObject selectedCanvas;
    public List<GridPart> myNeighbours = new List<GridPart>();

    private void Start()
    {
        SetSnapFieldNeighbours();
    }

    public void Click()
    {
        if (isSelected) return;

        SelectedToggle(true);
        GridManager.Instance.CheckAllGridPartSelectionCondition();
    }

    public void SelectedToggle(bool status)
    {
        isSelected = status;
        SelectedCanvasEnabledToggle(status);
    }

    private void SelectedCanvasEnabledToggle(bool isEnabled)
    {
        selectedCanvas.SetActive(isEnabled);
    }

    public void CheckNeighboursSelectedCondition()
    {
        List<GridPart> selectedNeighbourList = GetSelectedNeighbourList();

        if (isSelected && selectedNeighbourList.Count >= 2)
        {
            SelectedToggle(false);
            foreach (var gridPart in selectedNeighbourList)
            {
                gridPart.SelectedToggle(false);
            }
        }
    }

    #region Helper

    private List<GridPart> GetSelectedNeighbourList()
    {
        List<GridPart> returnedSelectedNeighbourList = new List<GridPart>();

        foreach (GridPart neighbour in myNeighbours.Where(neighbour => neighbour.isSelected))
        {
            ListHelper.AddToList(neighbour, returnedSelectedNeighbourList);
        }
        
        return returnedSelectedNeighbourList;
    }

    private void SetSnapFieldNeighbours()
    {
        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 5f))
            {
                if (hit.transform.TryGetComponent(out GridPart gridPart))
                    ListHelper.AddToList(gridPart, myNeighbours);
            }
        }
    }

    #endregion
}