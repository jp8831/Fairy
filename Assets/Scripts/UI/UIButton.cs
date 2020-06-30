using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIButton : UIElement
{
    private Button m_button;
    private UnityEvent m_buttonOnClick = new UnityEvent ();

    protected override void Start()
    {
        base.Start ();

        m_button = GetComponent<Button>();
        m_button.onClick.AddListener (OnButtonClick);
    }

    public void AddOnClickListener(UnityAction onClick)
    {
        m_buttonOnClick.AddListener (onClick);
    }

    public void RemoveOnClickListener(UnityAction onClick)
    {
        m_buttonOnClick.RemoveListener (onClick);
    }

    private void OnButtonClick ()
    {
        m_buttonOnClick.Invoke ();
    }
}
