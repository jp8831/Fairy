using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageFlicker : MonoBehaviour
{
    [SerializeField]
    private float m_flickerSeconds;

    private Image m_image;
    private Color m_originalColor;
    private float m_startTime;

    private void Awake ()
    {
        m_image = GetComponent<Image> ();
        m_originalColor = m_image.color;
    }

    private void Update ()
    {
        float interpolation = (Time.time - m_startTime) / (0.5f * m_flickerSeconds + Mathf.Epsilon);
        float alpha = Mathf.SmoothStep (0.0f, m_originalColor.a, Mathf.PingPong (interpolation, 1.0f));

        var color = m_image.color;
        color.a = alpha;

        m_image.color = color;
    }

    private void OnEnable ()
    {
        m_startTime = Time.time;
    }

    private void OnDisable ()
    {
        m_image.color = m_originalColor;
    }
}
