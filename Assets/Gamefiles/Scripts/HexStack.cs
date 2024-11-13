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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Placed()
    {
        foreach (Hexagon hexagon in hexagons)
        {
            hexagon.DisableCollider();
        }
    }
}
