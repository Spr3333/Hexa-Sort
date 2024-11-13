using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    public List<Hexagon> hexagons { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Add(Hexagon hexagon)
    {
        if (hexagons == null)
            hexagons = new List<Hexagon>();

        hexagons.Add(hexagon);
        hexagon.SetParent(transform);
    }

    public Color GetTopHexagonColor()
    {
        return hexagons[^1].color;
    }

    public void Placed()
    {
        foreach (Hexagon hexagon in hexagons)
        {
            hexagon.DisableCollider();
        }
    }

    public bool Contains(Hexagon hexagon) => hexagons.Contains(hexagon);


    public void Remove(Hexagon hexagon)
    {
        hexagons.Remove(hexagon);
        if (hexagons.Count <= 0)
            DestroyImmediate(this.gameObject);
    }
}
