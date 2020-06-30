using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : UIElement
{
    private Text m_text;

    public string Value
    {
        get { return m_text.text; }
        set { m_text.text = value; }
    }

    protected override void Awake ()
    {
        base.Awake ();

        m_text = GetComponent<Text> ();
    }
}
