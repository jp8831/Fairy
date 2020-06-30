using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Floor : MonoBehaviour, IPlayerSelectable
{
    [SerializeField]
    private int m_startLevel;
    [SerializeField]
    private GameObject[] m_levelObjects;
    [SerializeField]
    private BoxCollider m_navigationCollider;

    private int m_level;
    private int m_fairyCount;

    public int MaxLevel
    {
        get { return m_levelObjects.Length; }
    }

    public int Level
    {
        get { return m_level; }
        set
        {
            m_level = Mathf.Clamp(value, 0, MaxLevel);

            for (int i = 0; i < MaxLevel; i++)
            {
                bool bActive = i < m_level;
                m_levelObjects[i].SetActive(bActive);
            }
        }
    }

    public int FairyCount
    {
        get { return m_fairyCount; }
    }

    public Vector3 RandomPoint
    {
        get
        {
            Vector3 desiredRandomPoint = transform.position;
            desiredRandomPoint.x = Random.Range (m_navigationCollider.bounds.min.x, m_navigationCollider.bounds.max.x);
            desiredRandomPoint.z = Random.Range (m_navigationCollider.bounds.min.z, m_navigationCollider.bounds.max.z);

            return FindClosestNavMeshPoint (desiredRandomPoint);
        }
    }

    private void Start()
    {
        Level = m_startLevel;
    }

    public void OnDeselect ()
    {
        
    }

    public void OnSelect ()
    {
        
    }

    public Vector3 FindClosestNavMeshPoint (Vector3 desiredPoint)
    {
        Vector3 closestPoint = transform.position;
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition (desiredPoint, out navMeshHit, float.PositiveInfinity, NavMesh.AllAreas))
        {
            closestPoint = navMeshHit.position;
        }

        return closestPoint;
    }
}
