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

    public void MoveToLocal(Vector3 pos)
    {
        LeanTween.moveLocal(gameObject, pos, .2f).setEase(LeanTweenType.easeInOutSine).setDelay(transform.GetSiblingIndex() * .01f);
    }

    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, .2f).setEase(LeanTweenType.easeInBack).setDelay(delay).setOnComplete(() => Destroy(gameObject));
    }


}
