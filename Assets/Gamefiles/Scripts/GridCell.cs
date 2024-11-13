using UnityEngine;

public class GridCell : MonoBehaviour
{
    public HexStack stack { get; private set; }
    public bool isOccupied
    {
        get => stack != null;
        private set { }
    }

    public void AssignStack(HexStack stack)
    {
        this.stack = stack;
    }
}
