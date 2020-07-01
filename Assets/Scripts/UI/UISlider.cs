using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : UIElement
{
    private Slider m_slider;

    public float Value
    {
        get { return m_slider.value; }
        set { m_slider.value = value; }
    }

    public float MinValue
    {
        get { return m_slider.minValue; }
        set { m_slider.minValue = value; }
    }

    public float MaxValue
    {
        get { return m_slider.maxValue; }
        set { m_slider.maxValue = value; }
    }

    protected override void Awake ()
    {
        base.Awake ();

        m_slider = GetComponent<Slider> ();
    }
}
