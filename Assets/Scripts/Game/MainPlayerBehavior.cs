using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPlayerBehavior : PlayerBehavior
{
    private enum SelectedType
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
    private SelectedType m_selectedType;

    private void Start ()
    {
        m_rule = GetComponent<MainGameRule> ();
        m_rule.UIController.FindUI<UIButton> (m_floorUpgradeButtonName).AddOnClickListener (UpgradeFloor);
    }

    private void OnEnable ()
    {
        PlayerInput.OnMouseButtonDownEventMap.AddListener (0, SelectOnMouse);
        PlayerInput.OnMouseButtonUpEventMap.AddListener (0, OnLeftMouseUp);
    }

    private void UpgradeFloor ()
    {
        if (SelectedGameObject)
        {
            var floor = SelectedGameObject.GetComponent<Floor> ();
            m_rule.UpgradeFloor (floor);
        }
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
                    m_selectedType = SelectedType.Fairy;
                }
                else if (hitGameObject.CompareTag (m_floorTag))
                {
                    m_selectedType = SelectedType.Floor;
                    m_rule.UIController.ActivateUI (m_floorUpgradeUIName);
                }

                bNothing = false;
            }
        }

        if (bNothing)
        {
            if (SelectedGameObject && m_selectedType == SelectedType.Floor)
            {
                m_rule.UIController.DeactivateUI (m_floorUpgradeUIName);
            }

            Deselect ();
        }
    }

    private void OnLeftMouseUp ()
    {
        if (m_selectedType == SelectedType.Fairy)
        {
            Deselect ();
        }
    }
}
