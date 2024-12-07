using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    private void Awake()
    {
        StackController.OnStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.OnStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridCell cell)
    {
        StartCoroutine(StackPlacedCoroutine(cell));
        //Does the cell Have Neigbours?


    }

    IEnumerator StackPlacedCoroutine(GridCell cell)
    {
        yield return CheckForMerge(cell);
    }

    IEnumerator CheckForMerge(GridCell cell)
    {
        List<GridCell> neighborGridCells = GetNeighborGridCells(cell);

        if (neighborGridCells.Count <= 0)
        {
            yield break;
        }
        //we have number of gridcell which are ocupied
        Color gridCellTopHexogonColor = cell.stack.GetTopHexagonColor();

        //Do the neighbors cell have same top hex color
        List<GridCell> similarNeigborGridCells = GetSimilarNeigborGridCells(gridCellTopHexogonColor, neighborGridCells);

        if (similarNeigborGridCells.Count <= 0)
        {
            Debug.Log("No similar neighbor found");
            yield break;
        }

        //we have similar neighbors with same top color
        List<Hexagon> hexagonToAdd = GetHexagonsToAdd(gridCellTopHexogonColor, similarNeigborGridCells);

        //Remove the hexagons from thos neigbor cells
        RemoveHexagonsFromStacks(similarNeigborGridCells, hexagonToAdd);

        //Move hexagons to main cell
        MoveHexagons(cell, hexagonToAdd);

        yield return new WaitForSeconds(.2f + (hexagonToAdd.Count + 1) * .01f);

        //If the hexagons added is more than 10 merge them and them to score
        yield return CheckForCompletStack(cell, gridCellTopHexogonColor);
    }

    private List<GridCell> GetNeighborGridCells(GridCell cell)
    {
        LayerMask gridCellMask = 1 << cell.gameObject.layer;

        List<GridCell> neighborGridCells = new List<GridCell>();

        Collider[] neighborGridCellCol = Physics.OverlapSphere(cell.transform.position, 2, gridCellMask);

        foreach (Collider neighborcol in neighborGridCellCol)
        {
            GridCell neighborCell = neighborcol.GetComponent<GridCell>();

            if (!neighborCell.isOccupied)
                continue;

            if (neighborCell == cell)
                continue;

            neighborGridCells.Add(neighborCell);

        }

        return neighborGridCells;
    }

    private List<GridCell> GetSimilarNeigborGridCells(Color gridCellTopHexogonColor, List<GridCell> neighborGridCells)
    {
        List<GridCell> similarNeigborGridCells = new List<GridCell>();

        foreach (GridCell similarNeighbor in neighborGridCells)
        {
            Color similarNeighborTopHexagonColor = similarNeighbor.stack.GetTopHexagonColor();

            if (similarNeighborTopHexagonColor == gridCellTopHexogonColor)
                similarNeigborGridCells.Add(similarNeighbor);
        }
        return similarNeigborGridCells;
    }

    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexogonColor, List<GridCell> similarNeigborGridCells)
    {
        List<Hexagon> hexagonToAdd = new List<Hexagon>();

        foreach (GridCell similarNeighbor in similarNeigborGridCells)
        {
            HexStack neigborStack = similarNeighbor.stack;
            for (int i = neigborStack.hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neigborStack.hexagons[i];

                if (hexagon.color != gridCellTopHexogonColor)
                    break;

                hexagonToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }
        return hexagonToAdd;
    }

    private void RemoveHexagonsFromStacks(List<GridCell> similarNeigborGridCells, List<Hexagon> hexagonToAdd)
    {
        foreach (GridCell neighborCell in similarNeigborGridCells)
        {
            HexStack stack = neighborCell.stack;
            foreach (Hexagon hexagon in hexagonToAdd)
            {
                if (stack.Contains(hexagon))
                {
                    stack.Remove(hexagon);
                }
            }
        }
    }

    private void MoveHexagons(GridCell cell, List<Hexagon> hexagonToAdd)
    {
        float initialY = cell.stack.hexagons.Count * .2f;

        for (int i = 0; i < hexagonToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonToAdd[i];

            float targetY = initialY + i * .2f;
            Vector3 targetLocalPosition = Vector3.up * targetY;

            cell.stack.Add(hexagon);
            hexagon.MoveToLocal(targetLocalPosition);
        }
    }

    private IEnumerator CheckForCompletStack(GridCell cell, Color topColor)
    {
        if (cell.stack.hexagons.Count < 10)
            yield break;
        List<Hexagon> similarHexagons = new List<Hexagon>();

        for (int i = cell.stack.hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hex = cell.stack.hexagons[i];
            if (hex.color != topColor)
                break;
            similarHexagons.Add(hex);
        }

        int similarHexagonsCount = similarHexagons.Count;

        if (similarHexagons.Count < 10)
            yield break;

        float delay = 0;

        while (similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            similarHexagons[0].Vanish(delay);
            //DestroyImmediate(similarHexagons[0].gameObject);
            delay += .01f;

            cell.stack.Remove(similarHexagons[0]);
            similarHexagons.RemoveAt(0);
        }

        yield return new WaitForSeconds(.2f + (similarHexagonsCount + 1) * .01f);
    }





}
