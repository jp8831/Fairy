using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : UIElement
{
    [SerializeField]
    private bool m_bRenderModeCamera = false;
    [SerializeField]
    private string m_sortLayerName = "Default";
    [SerializeField]
    private int m_sortIndex = 0;

    private Canvas m_canvas;

    protected override void Awake()
    {
        base.Awake ();

        m_canvas = GetComponent<Canvas> ();
        m_canvas.sortingLayerName = m_sortLayerName;
        m_canvas.sortingOrder = m_sortIndex;
    }

    protected override void Start ()
    {
        base.Start ();

        if (m_bRenderModeCamera && Camera.main)
        {
            m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
            m_canvas.worldCamera = Camera.main;
        }
    }
}
