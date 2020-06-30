using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIButton : UIElement
{
    [SerializeField]
    private AudioClip m_clickAudio;

    private Button m_button;
    private UnityEvent m_onClick = new UnityEvent ();
    private AudioManager m_audioManager;

    protected override void Awake()
    {
        base.Awake ();

        m_button = GetComponent<Button>();
        m_button.onClick.AddListener (OnButtonClick);

        m_audioManager = UIController.GetComponent<AudioManager> ();
    }

    public void AddOnClickListener(UnityAction onClick)
    {
        m_onClick.AddListener (onClick);
    }

    public void RemoveOnClickListener(UnityAction onClick)
    {
        m_onClick.RemoveListener (onClick);
    }

    private void OnButtonClick ()
    {
        if (m_clickAudio)
        {
            m_audioManager.PlayUIAudio (m_clickAudio);
        }

        m_onClick.Invoke ();
    }
}
