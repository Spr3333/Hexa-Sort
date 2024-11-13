using System;
using UnityEngine;

public class StackController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private LayerMask groundMask;
    private HexStack currentStack;
    private Vector3 currentStackInitialPos;


    [Header("Events")]
    public static Action<GridCell> OnStackPlaced;

    //Helpers
    private GridCell targetCell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ManageControl();
    }

    void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();
        else if (Input.GetMouseButton(0) && currentStack != null)
            ManageMouseDrag();
        else if (Input.GetMouseButtonUp(0) && currentStack != null)
            ManageMouseUp();
    }

    void ManageMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(ClickedRay(), out hit, 500, hexagonLayerMask);

        if (hit.collider == null)
        {
            Debug.Log("Didnt hit anyone");
            return;
        }
        currentStack = hit.collider.GetComponent<Hexagon>().hexStack;
        currentStackInitialPos = currentStack.transform.position;
    }

    void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(ClickedRay(), out hit, 500, gridLayerMask);

        if (hit.collider == null)
            DraggingAbove();
        else
            DragingAboveGridCell(hit);
    }


    private void ManageMouseUp()
    {
        if (targetCell == null)
        {
            currentStack.transform.position = currentStackInitialPos;
            currentStack = null;
            return;
        }

        currentStack.transform.position = targetCell.transform.position.With(y: 0.2f);
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Placed();
        targetCell.AssignStack(currentStack);
        OnStackPlaced?.Invoke(targetCell);

        targetCell = null;
        currentStack = null;

    }

    private Ray ClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
    private void DraggingAbove()
    {
        RaycastHit hit;
        Physics.Raycast(ClickedRay(), out hit, 500, groundMask);

        if (hit.collider == null)
        {
            Debug.Log("No Ground hit");
            return;
        }

        Vector3 currentStackPos = hit.point.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, currentStackPos, Time.deltaTime * 30);
        targetCell = null;
    }
    private void DragingAboveGridCell(RaycastHit hit)
    {
        GridCell cell = hit.collider.GetComponent<GridCell>();

        if (cell.isOccupied)
            DraggingAbove();
        else
            DraggingAboveNonOccupiedGridCell(cell);
    }

    void DraggingAboveNonOccupiedGridCell(GridCell cell)
    {
        Vector3 currentStackPos = cell.transform.position.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, currentStackPos, Time.deltaTime * 30);
        targetCell = cell;

    }

}
