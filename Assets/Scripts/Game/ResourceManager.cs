using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header ("Paper")]
    [SerializeField]
    private Floor m_paperFloor;
    [SerializeField]
    private float m_startPaperResourece;
    [SerializeField]
    private float m_maxPaperResource;
    [SerializeField]
    private float m_paperGainPerLevel;

    [Header ("Ink")]
    [SerializeField]
    private Floor m_inkFloor;
    [SerializeField]
    private float m_startInkResource;
    [SerializeField]
    private float m_maxInkResource;
    [SerializeField]
    private float m_inkGainPerLevel;

    [Header ("Elec")]
    [SerializeField]
    private Floor m_elecFloor;
    [SerializeField]
    private float m_startElecResource;
    [SerializeField]
    private float m_maxElecResource;
    [SerializeField]
    private float m_elecGainPerLevel;

    [Header ("UI")]
    [SerializeField]
    private string m_paperTextUIName;
    [SerializeField]
    private string m_inkTextUIName;
    [SerializeField]
    private string m_elecTextUIName;

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
            m_paperResource = Mathf.Clamp (value, 0.0f, m_maxPaperResource);
            m_paperText.Value = $"{Mathf.FloorToInt (m_paperResource)}";
        }
    }

    public float InkResource
    {
        get { return m_inkResource; }
        set
        {
            m_inkResource = Mathf.Clamp (value, 0.0f, m_maxInkResource);
            m_inkText.Value = $"{Mathf.FloorToInt (m_inkResource)}";
        }
    }

    public float ElecResource
    {
        get { return m_elecResource; }
        set
        {
            m_elecResource = Mathf.Clamp (value, 0.0f, m_maxElecResource);
            m_elecText.Value = $"{Mathf.FloorToInt (m_elecResource)}";
        }
    }

    private void Awake ()
    {
        var rule = GetComponent<MainGameRule> ();

        m_paperText = rule.UIController.FindUI<UIText> (m_paperTextUIName);
        m_inkText = rule.UIController.FindUI<UIText> (m_inkTextUIName);
        m_elecText = rule.UIController.FindUI<UIText> (m_elecTextUIName);

        PaperResource = m_startPaperResourece;
        InkResource = m_startInkResource;
        ElecResource = m_startElecResource;
    }

    private void Update ()
    {
        PaperResource += m_paperGainPerLevel * m_paperFloor.Level * Time.deltaTime;
        InkResource += m_inkGainPerLevel * m_inkFloor.Level * Time.deltaTime;
        ElecResource += m_elecGainPerLevel * m_elecFloor.Level * Time.deltaTime;
    }
}
