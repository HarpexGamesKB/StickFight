using DG.Tweening;
using TMPro;
using UnityEngine;

public class MultiplierGates : MonoBehaviour
{
    public enum SurfaceType
    {
        Opaque,
        Transparent
    }

    public enum BlendMode
    {
        Alpha,
        Premultiply,
        Additive,
        Multiply
    }

    public enum Type
    {
        Multiply,
        Add,
        Minus,
        Divide
    }

    [SerializeField] private TextMeshPro Text;
    [SerializeField] private Renderer Circle;
    [SerializeField] private Material Material;

    [Header("Params")]
    public Type MultiplierType;

    public int Value;

    private Material _ownMaterial;
    public bool Used { private set; get; }

    private bool _debug = true;

    public void Use()
    {
        Used = true;
        GetComponent<Collider>().enabled = false;
        transform.DOScale(Vector3.zero, .8f).SetEase(Ease.InBack);
    }

    private void Start()
    {
        ChangeVisual();
        Circle.transform.DORotate(Vector3.up * 360f, 10f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
    }

    private void OnValidate()
    {
        ChangeVisual();
    }

    private void Reset()
    {
        ChangeVisual();
    }

    private void ChangeVisual()
    {
        if (Text == null) return;
        if (_ownMaterial == null)
        {
            _ownMaterial = new Material(GetComponent<MaterialKeeper>().Material);
        }
        if (Value < 1)
        {
            Value = 1;
        }
        string text = "";
        switch (MultiplierType)
        {
            case Type.Add:
                text = "+" + Value;
                break;

            case Type.Multiply:
                text = "x" + Value;
                break;

            case Type.Minus:
                text = "-" + Value;
                break;

            case Type.Divide:
                text = "÷" + Value;
                break;
        }
        Text.text = text;
        if (Circle == null)
        {
            return;
        }
        if (_ownMaterial == null)
        {
            _ownMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            _ownMaterial.SetFloat("_Surface", (float)SurfaceType.Transparent);
            _ownMaterial.SetFloat("_Blend", (float)SurfaceType.Transparent);
        }
        else
        {
            _ownMaterial.color = (MultiplierType == Type.Add || MultiplierType == Type.Multiply ? Color.green / 1.4f : Color.red / 1.4f);
            Text.color = (MultiplierType == Type.Add || MultiplierType == Type.Multiply ? Color.green / 1.3f : Color.red / 1.3f);
        }
        if (Circle.sharedMaterial != _ownMaterial)
        {
            Circle.sharedMaterial = _ownMaterial;
        }

        name = "Gates " + text;
    }
}