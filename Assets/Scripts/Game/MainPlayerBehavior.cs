using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerBehavior : PlayerBehavior
{
    [SerializeField]
    private LayerMask m_fairyLayer;
    [SerializeField]
    private string m_fairyTag;

    private void OnEnable ()
    {
        m_playerInput.OnMouseButtonDownEventMap.AddListener (0, () => {
            SelectOnMouse (m_fairyLayer, m_fairyTag, true);
        });

        m_playerInput.OnMouseButtonEventMap.AddListener (0, () => {
            if (m_selectedGameObject)
            {
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                RaycastHit rayHit;

                if (Physics.Raycast (ray, out rayHit, float.PositiveInfinity, m_fairyLayer.value))
                {
                    m_selectedGameObject.transform.position = rayHit.point;
                };
            }
        });

        m_playerInput.OnMouseButtonUpEventMap.AddListener (0, () => {
            Deselect ();
        });
    }
}
