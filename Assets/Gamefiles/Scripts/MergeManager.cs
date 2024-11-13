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

        if (neighborGridCells.Count <= 0)
        {
            return;
        }

        Color gridCellTopHexogonColor = cell.stack.GetTopHexagonColor();

        List<GridCell> similarNeigborGridCells = new List<GridCell>();

        foreach (GridCell similarNeighbor in neighborGridCells)
        {
            Color similarNeighborTopHexagonColor = similarNeighbor.stack.GetTopHexagonColor();

            if (similarNeighborTopHexagonColor == gridCellTopHexogonColor)
                similarNeigborGridCells.Add(similarNeighbor);
        }

        if (similarNeigborGridCells.Count <= 0)
        {
            Debug.Log("No similar neighbor found");
            return;
        }

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

        float initialY = cell.stack.hexagons.Count * .2f;

        for (int i = 0; i < hexagonToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonToAdd[i];

            float targetY = initialY + i * .2f;
            Vector3 targetLocalPosition = Vector3.up * targetY;

            cell.stack.Add(hexagon);
            hexagon.transform.localPosition = targetLocalPosition;
        }
    }
}
