using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIButton : UIElement
{
    private Button m_button;

    private void Start()
    {
        m_button = GetComponent<Button>();
    }

    public void AddOnClickListener(UnityAction onClick)
    {
        m_button.onClick.AddListener(onClick);
    }

    public void RemoveOnClickListener(UnityAction onClick)
    {
        m_button.onClick.RemoveListener(onClick);
    }
}
