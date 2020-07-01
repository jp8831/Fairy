using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private Camera m_paintCamera;
    [SerializeField]
    private Color m_backgroundColor;
    [SerializeField]
    private RenderTexture m_paintTexture;
    [SerializeField]
    private RectTransform m_paintCanvasTransform;
    [SerializeField]
    private RectTransform m_brushCanvasTransform;

    [SerializeField]
    private float m_minBrushSize;
    [SerializeField]
    private float m_maxBrushSize;

    [SerializeField]
    private Slider m_brushSizeSlider;
    [SerializeField]
    private Slider m_cSlider;
    [SerializeField]
    private Slider m_mSlider;
    [SerializeField]
    private Slider m_ySlider;
    [SerializeField]
    private Slider m_kSlider;

    private float m_brushSize;
    private Vector4 m_cmyk;

    public bool DrawEnabled
    {
        set { m_brushCanvasTransform.gameObject.layer = value ? LayerMask.NameToLayer("Paint") : LayerMask.NameToLayer("Default"); }
    }

    void Start ()
    {
        ClearPaintTexture();

        Vector3 paintCameraPosition = m_paintCamera.transform.localPosition;
        paintCameraPosition.x = m_paintCanvasTransform.localPosition.x;
        paintCameraPosition.y = m_paintCanvasTransform.localPosition.y;
        m_paintCamera.transform.localPosition = paintCameraPosition;

        Vector2 paintCanvasSize = m_paintCanvasTransform.rect.size;
        m_paintCamera.orthographicSize = 0.5f * paintCanvasSize.y;

        float paintCameraAspectRatio = paintCanvasSize.x / paintCanvasSize.y;
        m_paintCamera.aspect = paintCameraAspectRatio;

        //m_paintCanvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2.0f * m_paintCamera.orthographicSize);
        //m_paintCanvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2.0f * m_paintCamera.orthographicSize);

        m_brushSize = 0.5f * (m_minBrushSize + m_maxBrushSize);
        m_brushSizeSlider.minValue = m_minBrushSize;
        m_brushSizeSlider.maxValue = m_maxBrushSize;
        m_brushSizeSlider.value = m_brushSize;

        Image brushImage = m_brushCanvasTransform.GetComponentInChildren<Image>();
        m_cmyk = ColorConvertor.RGBToCMYK(brushImage.color);
        m_cSlider.value = m_cmyk.x;
        m_mSlider.value = m_cmyk.y;
        m_ySlider.value = m_cmyk.z;
        m_kSlider.value = m_cmyk.w;

        DrawEnabled = false;
    }

    void Update ()
    {
        bool bDraw = Input.GetMouseButton (0);
        DrawEnabled = bDraw;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 localMousePos = m_paintCanvasTransform.InverseTransformPoint (worldMousePos);
        m_brushCanvasTransform.localPosition = new Vector3 (localMousePos.x, localMousePos.y, m_brushCanvasTransform.localPosition.z);
    }

    public void ClearPaintTexture ()
    {
        Graphics.SetRenderTarget(m_paintTexture);
        GL.Clear(true, true, m_backgroundColor, 1.0f);
    }

    public void OnCyanChange (Slider slider)
    {
        m_cmyk.x = slider.value;
        m_brushCanvasTransform.GetComponentInChildren<Image>().color = ColorConvertor.CMYKToRGB(m_cmyk);
    }

    public void OnMagentaChange(Slider slider)
    {
        m_cmyk.y = slider.value;
        m_brushCanvasTransform.GetComponentInChildren<Image>().color = ColorConvertor.CMYKToRGB(m_cmyk);
    }

    public void OnYellowChange(Slider slider)
    {
        m_cmyk.z = slider.value;
        m_brushCanvasTransform.GetComponentInChildren<Image>().color = ColorConvertor.CMYKToRGB(m_cmyk);
    }

    public void OnKeyChange(Slider slider)
    {
        m_cmyk.w = slider.value;
        m_brushCanvasTransform.GetComponentInChildren<Image>().color = ColorConvertor.CMYKToRGB(m_cmyk);
    }

    public void OnBrushSizeChange (Slider slider)
    {
        m_brushSize = slider.value;
        m_brushCanvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_brushSize);
        m_brushCanvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_brushSize);
    }
}
