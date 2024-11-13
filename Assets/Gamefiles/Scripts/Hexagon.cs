using UnityEngine;

public class Hexagon : MonoBehaviour
{

    [Header("Renderer")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider col;
    public HexStack hexStack { get; private set; }

    public Color color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }

    public void Configure(HexStack hexStack)
    {
        this.hexStack = hexStack;
    }

    public void DisableCollider() => col.enabled = false;

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }


}
