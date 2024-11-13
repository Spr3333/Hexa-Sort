using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{

    [Header("Hexagon Stack")]
    [SerializeField] private Transform stackPosParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexStack hexagonStackPrefab;
    [NaughtyAttributes.MinMaxSlider(2, 8)]
    [SerializeField] private Vector2Int minMaxStackCount;


    [Header("Color")]
    [SerializeField] private Color[] colors;

    //Helpers
    private int counter;

    private void Awake()
    {
        StackController.OnStackPlaced += StackPlacedCallback;
    }

    private void OnDestroy()
    {
        StackController.OnStackPlaced -= StackPlacedCallback;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateStacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [NaughtyAttributes.Button]
    private void GenerateStacks()
    {
        for (int i = 0; i < stackPosParent.childCount; i++)
            GenerateStack(stackPosParent.GetChild(i));
    }

    private void GenerateStack(Transform parent)
    {
        HexStack hexStack = Instantiate(hexagonStackPrefab, parent.position, Quaternion.identity, parent);
        hexStack.name = $"Stack {parent.GetSiblingIndex()}";

        int amount = Random.Range(2, 7);

        int firstColorHexagon = Random.Range(0, amount);

        Color[] colorArray = GetRandomColors();

        for (int i = 0; i < amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * .2f;
            Vector3 spawnPos = hexStack.transform.TransformPoint(hexagonLocalPos);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPos, Quaternion.identity, hexStack.transform);
            hexagonInstance.color = i < firstColorHexagon ? colorArray[0] : colorArray[1];
            hexagonInstance.Configure(hexStack);
            hexStack.Add(hexagonInstance);

        }
    }

    private Color[] GetRandomColors()
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);

        if (colorList.Count <= 0)
        {
            Debug.Log("No Color Found");
            return null;
        }

        Color firstColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(firstColor);

        if (colorList.Count <= 0)
        {
            Debug.Log("only One color Found");
            return null;
        }

        Color secondColor = colorList.OrderBy(x => Random.value).First();

        return new Color[] { firstColor, secondColor };
    }

    void StackPlacedCallback(GridCell cell)
    {
        counter++;
        if (counter >= 3)
        {
            counter = 0;
            GenerateStacks();
        }
    }
}
