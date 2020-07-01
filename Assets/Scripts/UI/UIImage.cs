using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIElement
{
    private Image m_image;

    public Sprite Sprite
    {
        get { return m_image.sprite; }
        set { m_image.sprite = value; }
    }

    protected override void Awake ()
    {
        base.Awake ();

        m_image = GetComponent<Image> ();
    }
}
