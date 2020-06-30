using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public interface IPlayerSelectable
{
    void OnSelect ();
    void OnDeselect ();
}

public interface IPlayerDraggable
{
    void OnDragStart ();
    void OnDrag ();
    void OnDragEnd ();
}

public abstract class PlayerBehavior : MonoBehaviour
{
    private PlayerController m_playerController;
    private PlayerInput m_playerInput;

    private GameObject m_selectedGameObject;
    private IPlayerSelectable m_selectable;
    private IPlayerDraggable m_draggable;

    public PlayerController PlayerController
    {
        get { return m_playerController; }
    }

    public PlayerInput PlayerInput
    {
        get { return m_playerInput; }
    }

    protected GameObject SelectedGameObject
    {
        get { return m_selectedGameObject; }
    }

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag ("Player");

        if (player)
        {
            m_playerController = player.GetComponent<PlayerController> ();
            m_playerController.Behavior = this;

            m_playerInput = player.GetComponent<PlayerInput> ();
        }
    }

    private void Update ()
    {
        if (m_draggable != null)
        {
            m_draggable.OnDrag ();
        }
    }

    protected bool Select (GameObject target)
    {
        if (!target)
        {
            return false;
        }

        Deselect ();

        var selectable = target.GetComponent<IPlayerSelectable> ();

        if (selectable == null)
        {
            return false;
        }

        m_selectedGameObject = target;
        m_selectable = selectable;
        m_selectable.OnSelect ();

        m_draggable = m_selectedGameObject.GetComponent<IPlayerDraggable> ();

        if (m_draggable != null)
        {
            m_draggable.OnDragStart ();
        }

        return true;
    }

    protected void Deselect ()
    {
        if (m_selectedGameObject)
        {
            if (m_draggable != null)
            {
                m_draggable.OnDragEnd ();
                m_draggable = null;
            }

            m_selectable.OnDeselect ();
            m_selectable = null;
            m_selectedGameObject = null;
        }
    }
}
