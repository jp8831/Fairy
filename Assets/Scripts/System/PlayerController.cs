using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput m_input;
    private PlayerBehavior m_behavior;

    public PlayerInput Input
    {
        get { return m_input; }
    }

    public PlayerBehavior Behavior
    {
        get { return m_behavior; }
        set { m_behavior = value; }
    }

    private void Start()
    {
        m_input = GetComponent<PlayerInput>();
    }
}
