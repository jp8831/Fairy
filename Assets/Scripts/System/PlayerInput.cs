using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private EventMap<KeyCode> m_onKeyEventMap;
    private EventMap<KeyCode> m_onKeyUpEventMap;
    private EventMap<KeyCode> m_onKeyDownEventMap;

    private EventMap<int> m_onMouseButtonEventMap;
    private EventMap<int> m_onMouseButtonUpEventMap;
    private EventMap<int> m_onMouseButtonDownEventMap;

    private Action<Vector3> m_mousePositionEvent;
    private Action<Vector3> m_mouseMovementEvent;
    private Action<Vector2> m_mouseWheelDeltaEvent;

    public EventMap<KeyCode> OnKeyEventMap
    {
        get { return m_onKeyEventMap; }
    }

    public EventMap<KeyCode> OnKeyUpEventMap
    {
        get { return m_onKeyUpEventMap; }
    }

    public EventMap<KeyCode> OnKeyDownEventMap
    {
        get { return m_onKeyDownEventMap; }
    }

    public EventMap<int> OnMouseButtonEventMap
    {
        get { return m_onMouseButtonEventMap; }
    }

    public EventMap<int> OnMouseButtonUpEventMap
    {
        get { return m_onMouseButtonUpEventMap; }
    }

    public EventMap<int> OnMouseButtonDownEventMap
    {
        get { return m_onMouseButtonDownEventMap; }
    }

    public Action<Vector3> MousePositionEvent
    {
        get { return m_mousePositionEvent; }
        set { m_mousePositionEvent = value; }
    }

    public Action<Vector3> MouseMovementEvent
    {
        get { return m_mouseMovementEvent; }
        set { m_mouseMovementEvent = value; }
    }

    public Action<Vector2> MouseWheelDeltaListener
    {
        get { return m_mouseWheelDeltaEvent; }
        set { m_mouseWheelDeltaEvent = value; }
    }

    private void Awake ()
    {
        m_onKeyEventMap = new EventMap<KeyCode> ();
        m_onKeyUpEventMap = new EventMap<KeyCode> ();
        m_onKeyDownEventMap = new EventMap<KeyCode> ();

        m_onMouseButtonEventMap = new EventMap<int> ();
        m_onMouseButtonUpEventMap = new EventMap<int> ();
        m_onMouseButtonDownEventMap = new EventMap<int> ();
    }

    private void Update ()
    {
        InvokeKeyboardEvents ();
        InvokeMouseEvent ();        
    }

    private void InvokeKeyboardEvents ()
    {
        // Invoke keyboard events.
        foreach (var onKeyEvent in m_onKeyEventMap.ListenerMap)
        {
            if (Input.GetKey (onKeyEvent.Key))
            {
                onKeyEvent.Value.Invoke ();
            }
        }

        foreach (var onKeyUpEvent in m_onKeyUpEventMap.ListenerMap)
        {
            if (Input.GetKeyUp (onKeyUpEvent.Key))
            {
                onKeyUpEvent.Value.Invoke ();
            }
        }

        foreach (var onKeyDownEvent in m_onKeyDownEventMap.ListenerMap)
        {
            if (Input.GetKeyDown (onKeyDownEvent.Key))
            {
                onKeyDownEvent.Value.Invoke ();
            }
        }
    }

    private void InvokeMouseEvent ()
    {
        if (Input.mousePresent == false)
        {
            return;
        }

        // Invoke mouse button events.
        foreach (var onMouseEvent in m_onMouseButtonEventMap.ListenerMap)
        {
            if (Input.GetMouseButton (onMouseEvent.Key))
            {
                onMouseEvent.Value.Invoke ();
            }
        }

        foreach (var onMouseUpEvent in m_onMouseButtonUpEventMap.ListenerMap)
        {
            if (Input.GetMouseButtonUp (onMouseUpEvent.Key))
            {
                onMouseUpEvent.Value.Invoke ();
            }
        }

        foreach (var onMouseDownEvent in m_onMouseButtonDownEventMap.ListenerMap)
        {
            if (Input.GetMouseButtonDown (onMouseDownEvent.Key))
            {
                onMouseDownEvent.Value.Invoke ();
            }
        }

        // Invoke mouse state events.
        if (m_mousePositionEvent != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            m_mousePositionEvent.Invoke(mousePosition);
        }

        if (m_mouseMovementEvent != null)
        {
            float mouseMovementX = Input.GetAxis("Mouse X");
            float mouseMovementy = Input.GetAxis("Mouse Y");
            Vector3 mouseMovement = new Vector3(mouseMovementX, mouseMovementy, 0.0f);
            m_mouseMovementEvent.Invoke(mouseMovement);
        }

        if (m_mousePositionEvent != null)
        {
            Vector2 mouseWheelDelta = Input.mouseScrollDelta;
            m_mouseWheelDeltaEvent.Invoke(mouseWheelDelta);
        }
    }
}
