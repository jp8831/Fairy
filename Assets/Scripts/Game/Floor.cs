using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, ISelectable
{
    [SerializeField]
    private int m_startLevel;
    [SerializeField]
    private GameObject[] m_levelObjects;

    private int m_level;

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
}
