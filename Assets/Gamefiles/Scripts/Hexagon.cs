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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
