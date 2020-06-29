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

    private void Start ()
    {
        m_text = GetComponent<Text> ();
    }
}
