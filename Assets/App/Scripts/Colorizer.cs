using UnityEngine;

public class Colorizer : MonoBehaviour
{
    [SerializeField] private Color BaseColor;
    [SerializeField] private Color CelColor;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _renderer.material = new Material(_renderer.material);
        _renderer.material.SetFloat("_SelfShadingSize", .7f);
        _renderer.material.SetFloat("_ShadowEdgeSize", 0f);
        _renderer.material.SetFloat("_ShadowEdgeSize", 0f);
        _renderer.material.SetColor("_OutlineColor", Color.black);
        _renderer.material.SetFloat("_OutlineWidth", 2);
        SetColor(BaseColor, CelColor);
    }

    public void SetColor(Color baseColor, Color celColor)
    {
        _renderer.material.color = baseColor;
        _renderer.material.SetColor("_ColorDim", celColor);
    }
}