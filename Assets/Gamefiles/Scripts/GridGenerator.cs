using NaughtyAttributes;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    [Header("Grid")]
    [SerializeField] private Grid grid;
    [OnValueChanged("GenerateGrid")]
    [SerializeField] private int gridSize;


    [Header("Prefab")]
    [SerializeField] private GameObject spawnPrefab;


    private void GenerateGrid()
    {
        transform.Clear();

        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));
                if (spawnPos.magnitude > grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * gridSize)
                {
                    continue;
                }

                Instantiate(spawnPrefab, spawnPos, Quaternion.identity, transform);
            }
        }
    }
}
