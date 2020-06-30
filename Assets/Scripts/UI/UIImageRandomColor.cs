using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageRandomColor : MonoBehaviour
{
    [SerializeField]
    private float m_updateSeconds;
    [SerializeField]
    private bool m_bRandomR;
    [SerializeField]
    private bool m_bRandomG;
    [SerializeField]
    private bool m_bRandomB;

    private Image m_image;
    private float m_lastUpdateTime;
    private Color m_targetColor;

    private void Awake ()
    {
        m_image = GetComponent<Image> ();
    }

    private void Update ()
    {
        float interpolation = Mathf.Min ((Time.time - m_lastUpdateTime) / (m_updateSeconds + Mathf.Epsilon), 1.0f);
        m_image.color = Color.Lerp (m_image.color, m_targetColor, interpolation);

        if (interpolation >= 1.0f)
        {
            UpdateTargetColor ();
        }
    }

    private void OnEnable ()
    {
        UpdateTargetColor ();
    }

    private void UpdateTargetColor ()
    {
        m_lastUpdateTime = Time.time;
        m_targetColor = m_image.color;

        if (m_bRandomR)
        {
            m_targetColor.r = Random.Range (0.0f, 1.0f);
        }

        if (m_bRandomG)
        {
            m_targetColor.g = Random.Range (0.0f, 1.0f);
        }

        if (m_bRandomB)
        {
            m_targetColor.b = Random.Range (0.0f, 1.0f);
        }
    }
}
