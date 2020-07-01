using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private Camera m_paintCamera;
    [SerializeField]
    private RawImage m_paintImage;
    [SerializeField]
    private Vector2Int m_paintSize;
    [SerializeField]
    private Color m_backgroundColor;
    [SerializeField]
    private RectTransform m_paintCanvasTransform;
    [SerializeField]
    private string m_paintLayerName;
    [SerializeField]
    private string m_defaultLayerName;

    [Header ("Brush")]
    [SerializeField]
    private float m_minBrushSize;
    [SerializeField]
    private float m_maxBrushSize;
    [SerializeField]
    private RectTransform m_burshTransform;
    [SerializeField]
    private Image m_brushImage;

    private float m_brushSize;
    private RenderTexture m_paintTexture;

    public bool DrawEnabled
    {
        set
        {
            var paintLayer = LayerMask.NameToLayer (m_paintLayerName);
            var defaultLayer = LayerMask.NameToLayer (m_defaultLayerName);
            m_burshTransform.gameObject.layer = value ? paintLayer : defaultLayer;
        }
    }

    public float MaxBrushSize
    {
        get { return m_maxBrushSize; }
    }

    public float MinBrushSize
    {
        get { return m_minBrushSize; }
    }

    public float BrushSize
    {
        get { return m_brushSize; }
        set
        {
            m_brushSize = value;
            m_burshTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, m_brushSize);
            m_burshTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, m_brushSize);
        }
    }

    public Vector4 BrushColorCMYK
    {
        get
        {
            return ColorConvertor.RGBToCMYK (m_brushImage.color);
        }

        set
        {
            m_brushImage.color = ColorConvertor.CMYKToRGB (value);
        }
    }

    public Texture2D PaintTexture
    {
        get
        {
            var paintTextureCopy = new Texture2D (m_paintTexture.width, m_paintTexture.height, TextureFormat.ARGB32, false);

            RenderTexture.active = m_paintTexture;

            paintTextureCopy.ReadPixels (new Rect (0, 0, m_paintSize.x, m_paintSize.y), 0, 0);
            paintTextureCopy.Apply ();

            return paintTextureCopy;
        }
    }

    void Start ()
    {
        m_paintTexture = new RenderTexture (m_paintSize.x, m_paintSize.y, 0, RenderTextureFormat.ARGB32);

        m_paintImage.texture = m_paintTexture;
        m_paintCamera.targetTexture = m_paintTexture;

        ClearPaintTexture ();

        Vector3 paintCameraPosition = m_paintCamera.transform.localPosition;
        paintCameraPosition.x = m_paintCanvasTransform.localPosition.x;
        paintCameraPosition.y = m_paintCanvasTransform.localPosition.y;
        m_paintCamera.transform.localPosition = paintCameraPosition;

        Vector2 paintCanvasSize = m_paintCanvasTransform.rect.size;
        m_paintCamera.orthographicSize = 0.5f * paintCanvasSize.y;

        float paintCameraAspectRatio = paintCanvasSize.x / paintCanvasSize.y;
        m_paintCamera.aspect = paintCameraAspectRatio;

        BrushSize = 0.5f * (m_minBrushSize + m_maxBrushSize);
        DrawEnabled = false;
    }

    void Update ()
    {
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 localMousePos = m_paintCanvasTransform.InverseTransformPoint (worldMousePos);
        m_burshTransform.localPosition = new Vector3 (localMousePos.x, localMousePos.y, m_burshTransform.localPosition.z);
    }

    public void ClearPaintTexture ()
    {
        Graphics.SetRenderTarget(m_paintTexture);
        GL.Clear(true, true, m_backgroundColor, 1.0f);
    }
}
