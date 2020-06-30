using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : UIElement
{
    [SerializeField]
    private Image m_progressImage;

    public float Value
    {
        set
        {
            m_progressImage.fillAmount = Mathf.Clamp (value, 0.0f, 1.0f);
        }
    }

    protected override void Awake()
    {
        base.Awake ();

        m_progressImage = GetComponentInChildren<Image> ();
    }
}
