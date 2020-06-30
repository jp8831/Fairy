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
    private Floor m_controlFloor;
    [SerializeField]
    private float m_fairySpawnPrice;

    [Header ("Paper Resource")]
    [SerializeField]
    private Floor m_paperFloor;
    [SerializeField]
    private float m_startPaperResourece;
    [SerializeField]
    private float m_maxPaperResource;
    [SerializeField]
    private float m_paperGainPerLevel;

    [Header ("Ink Resource")]
    [SerializeField]
    private Floor m_inkFloor;
    [SerializeField]
    private float m_startInkResource;
    [SerializeField]
    private float m_maxInkResource;
    [SerializeField]
    private float m_inkGainPerLevel;

    [Header ("Elec Resource")]
    [SerializeField]
    private Floor m_elecFloor;
    [SerializeField]
    private float m_startElecResource;
    [SerializeField]
    private float m_maxElecResource;
    [SerializeField]
    private float m_elecGainPerLevel;

    [Header ("Audio")]
    [SerializeField]
    private AudioClip m_spawnFairyAudio;
    [SerializeField]
    private AudioClip m_menuOpenAudio;
    [SerializeField]
    private AudioClip m_menuCloseAudio;

    private float m_paperResource;
    private float m_inkResource;
    private float m_elecResource;
    private UIText m_paperText;
    private UIText m_inkText;
    private UIText m_elecText;

    private UIElement m_pauseMenu;
    private UIButton m_pauseMenuButton;
    private UIButton m_continueButton;
    private UIButton m_exitButton;

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

    private void OnDestroy ()
    {
        if (m_pauseMenuButton)
        {
            m_pauseMenuButton.RemoveOnClickListener (PausePlay);
        }

        if (m_continueButton)
        {
            m_continueButton.RemoveOnClickListener (ContinuePlay);
        }

        if (m_exitButton)
        {
            m_exitButton.RemoveOnClickListener (ExitPlay);
        }
    }

    public override void OnPlayStart ()
    {
        base.OnPlayStart ();

        SpawnFairy (m_controlFloor);  

        m_paperText = UIController.FindUI<UIText> ("Text_Paper");
        m_inkText = UIController.FindUI<UIText> ("Text_Ink");
        m_elecText = UIController.FindUI<UIText> ("Text_Elec");

        PaperResource = m_startPaperResourece;
        InkResource = m_startInkResource;
        ElecResource = m_startElecResource;

        m_pauseMenu = UIController.FindUI<UIElement> ("Group_Pause Menu");
        m_pauseMenuButton = UIController.FindUI<UIButton> ("Button_Menu");
        m_pauseMenuButton.AddOnClickListener (PausePlay);

        m_continueButton = UIController.FindUI<UIButton> ("Button_Continue");
        m_continueButton.AddOnClickListener (ContinuePlay);

        m_exitButton = UIController.FindUI<UIButton> ("Button_Exit");
        m_exitButton.AddOnClickListener (ExitPlay);
    }

    public override void OnPlay ()
    {
        PaperResource += m_paperGainPerLevel * m_paperFloor.Level * Time.deltaTime;
        InkResource += m_inkGainPerLevel * m_inkFloor.Level * Time.deltaTime;
        ElecResource += m_elecGainPerLevel * m_elecFloor.Level * Time.deltaTime;
    }

    public override void OnPlayEnd ()
    {
        base.OnPlayEnd ();
    }

    private void PausePlay ()
    {
        UIController.ActivateUI (m_pauseMenu.ElementID);
        AudioManager.PlayUIAudio (m_menuOpenAudio);
    }

    private void ContinuePlay ()
    {
        UIController.DeactivateUI (m_pauseMenu.ElementID);
        AudioManager.PlayUIAudio (m_menuCloseAudio);
    }

    private void ExitPlay ()
    {
        UIController.DeactivateUI (m_pauseMenu.ElementID);
        UIController.DeactivateUI (UICanvasName);
        GameController.StopPlay ();
    }

    public void SpawnFairy (Floor floor)
    {
        AudioManager.PlayGameAudio (m_spawnFairyAudio);

        var fairy = Instantiate (m_fairyPrefab, Vector3.zero, Quaternion.identity).GetComponent<Fairy> ();
        fairy.AssignedFloor = floor;
    }

    public void UpgradeFloor (Floor floor)
    {
        if (ElecResource < 5.0f)
        {
            return;
        }

        floor.Level += 1;
        ElecResource -= 5.0f;
    }
}
