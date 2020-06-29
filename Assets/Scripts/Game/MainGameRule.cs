using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainGameRule : GameRule
{
    [SerializeField]
    private GameObject m_fairyPrefab;
    [SerializeField]
    private GameObject[] m_floors;

    [SerializeField]
    private float m_startPaperResourece;
    [SerializeField]
    private float m_startInkResource;
    [SerializeField]
    private float m_startElecResource;

    private float m_paperResource;
    private float m_inkResource;
    private float m_elecResource;
    private UIText m_paperText;
    private UIText m_inkText;
    private UIText m_elecText;

    public float PaperResource
    {
        get { return m_paperResource; }
        set
        {
            m_paperResource = value;
            m_paperText.Value = $"{Mathf.FloorToInt (m_paperResource)}";
        }
    }

    public float InkResource
    {
        get { return m_inkResource; }
        set
        {
            m_inkResource = value;
            m_inkText.Value = $"{Mathf.FloorToInt (m_inkResource)}";
        }
    }

    public float ElecResource
    {
        get { return m_elecResource; }
        set
        {
            m_elecResource = value;
            m_elecText.Value = $"{Mathf.FloorToInt (m_elecResource)}";
        }
    }

    public override void OnPlayStart ()
    {
        SpawnFairy (1);

        m_paperText = m_uiController.FindUI<UIText> ("Text_Paper");
        m_inkText = m_uiController.FindUI<UIText> ("Text_Ink");
        m_elecText = m_uiController.FindUI<UIText> ("Text_Elec");

        PaperResource = m_startPaperResourece;
        InkResource = m_startInkResource;
        ElecResource = m_startElecResource;
    }

    public override void OnPlayEnd ()
    {
        
    }

    private void SpawnFairy (int floorIndex)
    {
        bool bInvalidFloor = floorIndex < 0 || floorIndex >= m_floors.Length;

        if (bInvalidFloor)
        {
            return;
        }

        Vector3 floorPosition = m_floors[floorIndex].transform.position;
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition (floorPosition, out navMeshHit, 5.0f, NavMesh.AllAreas))
        {
            GameObject spawned = Instantiate (m_fairyPrefab, navMeshHit.position, Quaternion.identity);
        }
    }
}
