using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPlayerBehavior : PlayerBehavior
{
    private enum ESelectableType
    {
        Fairy, Floor
    }

    [SerializeField]
    private LayerMask m_selectLayer;
    [SerializeField]
    private string m_fairyTag;
    [SerializeField]
    private string m_floorTag;
    [SerializeField]
    private string m_floorUpgradeUIName;
    [SerializeField]
    private string m_floorUpgradeButtonName;

    private MainGameRule m_rule;
    private ESelectableType m_selectableType;

    private void Start ()
    {
        m_rule = GetComponent<MainGameRule> ();
    }

    private void OnEnable ()
    {
        PlayerInput.OnMouseButtonDownEventMap.AddListener (0, SelectOnMouse);
        PlayerInput.OnMouseButtonUpEventMap.AddListener (0, OnLeftMouseUp);
    }

    private void SelectOnMouse ()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            return;
        }

        bool bNothing = true;

        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast (ray, out rayHit, float.PositiveInfinity, m_selectLayer.value, QueryTriggerInteraction.Collide))
        {
            GameObject hitGameObject = rayHit.collider.gameObject;

            if (Select (hitGameObject))
            {
                if (hitGameObject.CompareTag (m_fairyTag))
                {
                    m_selectableType = ESelectableType.Fairy;
                }
                else if (hitGameObject.CompareTag (m_floorTag))
                {
                    m_selectableType = ESelectableType.Floor;
                    m_rule.UIController.ActivateUI (m_floorUpgradeUIName);
                }

                bNothing = false;
            }
        }

        if (bNothing)
        {
            if (SelectedGameObject && m_selectableType == ESelectableType.Floor)
            {
                m_rule.UIController.DeactivateUI (m_floorUpgradeUIName);
            }

            Deselect ();
        }
    }

    private void OnLeftMouseUp ()
    {
        if (m_selectableType == ESelectableType.Fairy)
        {
            Deselect ();
        }
    }
}
