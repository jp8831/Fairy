using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float m_endTime;

    private float m_startTime;
    private UIText m_remainTimeText;
    private UnityEvent m_endEvent;

    public UnityEvent EndEvent
    {
        get { return m_endEvent; }
    }

    public UIText RemainTimeUI
    {
        set { m_remainTimeText = value; }
    }

    private void Awake ()
    {
        m_endEvent = new UnityEvent ();
    }

    private void OnEnable ()
    {
        m_startTime = Time.time;
    }

    void Update()
    {
        float remainTime = Mathf.Max (m_endTime - (Time.time - m_startTime), 0.0f);

        if (remainTime <= 0.0f)
        {
            m_endEvent.Invoke ();
            enabled = false;
        }

        int minutes = Mathf.Clamp (Mathf.FloorToInt (remainTime / 60.0f), 0, 99);
        int seconds = Mathf.Max (0, Mathf.CeilToInt (remainTime - minutes * 60.0f));

        if (seconds == 60)
        {
            minutes += 1;
            seconds = 0;
        }

        if (m_remainTimeText)
        {
            m_remainTimeText.Value = string.Format ("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}
