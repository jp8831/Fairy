using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public abstract class PlayerBehavior : MonoBehaviour
{
    protected PlayerController m_playerController;
    protected PlayerInput m_playerInput;

    protected GameObject m_selectedGameObject;
    protected ISelectable m_selected;

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

    protected GameObject SelectOnMouse (LayerMask selectLayer, string selectTag, bool bQueryTrigger)
    {
        GameObject selectedGameObject = null;

        var triggerInteraction = bQueryTrigger ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast (ray, out rayHit, float.PositiveInfinity, selectLayer.value, triggerInteraction))
        {
            GameObject hitGameObject = rayHit.collider.gameObject;

            if (hitGameObject.CompareTag (selectTag))
            {
                if (Select (hitGameObject))
                {
                    selectedGameObject = hitGameObject;
                }
            }
        }

        return selectedGameObject;
    }

    protected bool Select (GameObject target)
    {
        if (!target)
        {
            return false;
        }

        if (m_selectedGameObject)
        {
            Deselect ();
        }

        var selectable = target.GetComponent<ISelectable> ();

        if (selectable == null)
        {
            return false;
        }

        m_selectedGameObject = target;
        m_selected = selectable;
        m_selected.OnSelect ();

        return true;
    }

    protected void Deselect ()
    {
        if (m_selectedGameObject)
        {
            m_selected.OnDeselect ();
            m_selected = null;
            m_selectedGameObject = null;
        }
    }
}
