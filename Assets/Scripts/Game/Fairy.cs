using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fairy : MonoBehaviour, IPlayerSelectable, IPlayerDraggable
{
    [SerializeField]
    private SpriteRenderer[] m_spriteRenderers;

    [Header ("Outline")]
    [SerializeField, Range (0.0f, 0.015f)]
    private float m_normalOutlineThickness;
    [SerializeField, ColorUsage (true, true)]
    private Color m_normalOutlineColor;
    [SerializeField, Range (0.0f, 0.015f)]
    private float m_selectedOutlineThickness;
    [SerializeField, ColorUsage (true, true)]
    private Color m_selectedOutlineColor;

    [Header ("Patrol")]
    [SerializeField]
    private float m_patrolRange;
    [SerializeField]
    private float m_patrolTime;

    private Floor m_floor;
    private Vector3 m_dragStartPosition;
    private NavMeshAgent m_navMeshAgent;
    private float m_lastPatrolUpdateTime;
    private SpriteOutline m_spriteOutline;

    public Floor AssignedFloor
    {
        set
        {
            if (!value)
            {
                return;
            }

            m_floor = value;
            m_navMeshAgent.Warp (m_floor.FindClosestNavMeshPoint (m_floor.transform.position));

            SetRandomPatrolPoint ();
        }
    }

    private void Awake ()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent> ();
        m_navMeshAgent.updateRotation = false;

        m_spriteOutline = GetComponent<SpriteOutline> ();
    }

    private void Start ()
    {
        SetRandomPatrolPoint ();
        SetRandomColor ();
    }

    private void Update ()
    {
        bool bOnDestination = (m_navMeshAgent.destination - transform.position).sqrMagnitude < 0.0001f;
        bool bUpdatePatrolPosition = (Time.time - m_lastPatrolUpdateTime) >= m_patrolTime && bOnDestination;

        if (bUpdatePatrolPosition)
        {
            SetRandomPatrolPoint ();
        }
    }

    private void SetRandomPatrolPoint ()
    {
        Vector3 randomPoint = m_floor.RandomPoint;

        m_navMeshAgent.SetDestination (randomPoint);
        m_lastPatrolUpdateTime = Time.time;
    }

    private void SetRandomColor ()
    {
        Vector4 cmyk = Vector4.zero;
        cmyk.x = Random.Range (0.0f, 1.0f);
        cmyk.y = Random.Range (0.0f, 1.0f);
        cmyk.z = Random.Range (0.0f, 1.0f);
        cmyk.w = Random.Range (0, 3) < 1 ? Random.Range (0.0f, 0.3f) : Random.Range (0.3f, 1.0f);

        Color rgb = CMYK.CMYKToRGB (cmyk);

        foreach (var spriteRenderer in m_spriteRenderers)
        {
            spriteRenderer.color = rgb;
        }
    }

    public void OnSelect ()
    {
        m_spriteOutline.OutlineThickness = m_selectedOutlineThickness;
        m_spriteOutline.OutlineColor = m_selectedOutlineColor;
    }

    public void OnDeselect ()
    {
        m_spriteOutline.OutlineThickness = m_normalOutlineThickness;
        m_spriteOutline.OutlineColor = m_normalOutlineColor;
    }

    public void OnDragStart ()
    {
        m_navMeshAgent.updatePosition = false;
        m_dragStartPosition = transform.position;
    }

    public void OnDrag ()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        Vector3 position = transform.position;
        position.x = mousePosition.x;
        position.z = mousePosition.z;

        transform.position = position;
    }

    public void OnDragEnd ()
    {
        bool bOriginalPosition = true;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit[] rayHit = Physics.RaycastAll (ray, float.PositiveInfinity);

        foreach (var hit in rayHit)
        {
            GameObject hitGameObject = hit.collider.gameObject;
            Floor hitFloor = hitGameObject.GetComponent<Floor> ();

            if (hitFloor && hitFloor != m_floor)
            {
                bOriginalPosition = false;
                AssignedFloor = hitFloor;
            }
        }

        if (bOriginalPosition)
        {
            m_navMeshAgent.ResetPath ();
            m_navMeshAgent.Warp (m_dragStartPosition);
        }

        m_navMeshAgent.updatePosition = true;
    }
}
