﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MainGameRule : GameRule
{
    [SerializeField]
    private GameObject m_fairyPrefab;
    [SerializeField]
    private Floor m_controlFloor;
    [SerializeField]
    private string m_upgradeButtonName;
    [SerializeField]
    private float m_upgradeFloorPrice;
    [SerializeField]
    private string m_spawnButtonName;
    [SerializeField]
    private float m_spawnFairyPrice;

    [Header ("Mini Game")]
    [SerializeField]
    private float m_miniGameInterval;
    [SerializeField]
    private string m_miniGameSceneName;
    [SerializeField]
    private string m_miniGameTimerTextName;

    [Header ("Audio")]
    [SerializeField]
    private AudioClip m_upgradeFloorAudio;
    [SerializeField]
    private AudioClip m_spawnFairyAudio;
    [SerializeField]
    private AudioClip m_menuOpenAudio;
    [SerializeField]
    private AudioClip m_menuCloseAudio;

    [Header ("Menu")]
    [SerializeField]
    private string m_menuUIName;
    [SerializeField]
    private string m_openMenuButtonName;
    [SerializeField]
    private string m_continueButtonName;
    [SerializeField]
    private string m_exitButtonName;

    private UIElement m_menuUI;
    private UIButton m_openMenuButton;
    private UIButton m_continueButton;
    private UIButton m_exitButton;
    private UIButton m_upgradeButton;
    private UIButton m_spawnButton;

    private MainPlayerBehavior m_player;
    private ResourceManager m_resourceManager;
    private Timer m_miniGameTimer;

    private void Start ()
    {
        m_player = GetComponent<MainPlayerBehavior> ();
        m_resourceManager = GetComponent<ResourceManager> ();

        m_menuUI = UIController.FindUI<UIElement> (m_menuUIName);

        m_openMenuButton = UIController.FindUI<UIButton> (m_openMenuButtonName);
        m_openMenuButton.AddOnClickListener (PausePlay);

        m_continueButton = UIController.FindUI<UIButton> (m_continueButtonName);
        m_continueButton.AddOnClickListener (ContinuePlay);

        m_exitButton = UIController.FindUI<UIButton> (m_exitButtonName);
        m_exitButton.AddOnClickListener (ExitPlay);

        m_upgradeButton = UIController.FindUI<UIButton> (m_upgradeButtonName);
        m_upgradeButton.AddOnClickListener (UpgradeSelectedFloor);

        m_spawnButton = UIController.FindUI<UIButton> (m_spawnButtonName);
        m_spawnButton.AddOnClickListener (SpawnFairyOnSelected);

        m_miniGameTimer = GetComponent<Timer> ();
        m_miniGameTimer.RemainTimeUI = UIController.FindUI<UIText> (m_miniGameTimerTextName);
        m_miniGameTimer.EndEvent.AddListener (PlayMiniGame);
    }

    private void OnDestroy ()
    {
        m_openMenuButton.RemoveOnClickListener (PausePlay);
        m_continueButton.RemoveOnClickListener (ContinuePlay);
        m_exitButton.RemoveOnClickListener (ExitPlay);
        m_upgradeButton.RemoveOnClickListener (UpgradeSelectedFloor);
        m_spawnButton.RemoveOnClickListener (SpawnFairyOnSelected);

        m_miniGameTimer.EndEvent.RemoveListener (PlayMiniGame);
    }

    public override void OnPlayStart ()
    {
        base.OnPlayStart ();

        SpawnFairy (m_controlFloor, true);
    }

    public override void OnPlay ()
    {
    }

    public override void OnPlayEnd ()
    {
        base.OnPlayEnd ();

        UIController.DeactivateUI (m_menuUI.ElementID);
    }

    private void PausePlay ()
    {
        UIController.ActivateUI (m_menuUI.ElementID);
        AudioManager.PlayUIAudio (m_menuOpenAudio);
    }

    private void ContinuePlay ()
    {
        UIController.DeactivateUI (m_menuUI.ElementID);
        AudioManager.PlayUIAudio (m_menuCloseAudio);
    }

    private void ExitPlay ()
    {
        GameController.StopPlay ();
    }

    private void PlayMiniGame ()
    {
        GameController.StartPlay (m_miniGameSceneName);
    }

    private void UpgradeSelectedFloor ()
    {
        var selected = m_player.SelectedGameObject;

        if (selected)
        {
            var floor = selected.GetComponent<Floor> ();

            if (floor)
            {
                UpgradeFloor (floor);
            }
        }
    }

    private void UpgradeFloor (Floor floor)
    {
        if (m_resourceManager.ElecResource < m_upgradeFloorPrice)
        {
            return;
        }

        AudioManager.PlayGameAudio (m_upgradeFloorAudio);

        floor.Level += 1;
        m_resourceManager.ElecResource -= m_upgradeFloorPrice;
    }

    private void SpawnFairyOnSelected ()
    {
        var selected = m_player.SelectedGameObject;

        if (selected)
        {
            var floor = selected.GetComponent<Floor> ();

            if (floor)
            {
                SpawnFairy (floor, false);
            }
        }
    }

    private void SpawnFairy (Floor floor, bool bFree)
    {
        if (m_resourceManager.InkResource < m_spawnFairyPrice && bFree == false)
        {
            return;
        }

        AudioManager.PlayGameAudio (m_spawnFairyAudio);

        var fairy = Instantiate (m_fairyPrefab, Vector3.zero, Quaternion.identity).GetComponent<Fairy> ();
        fairy.AssignedFloor = floor;

        if (bFree == false)
        {
            m_resourceManager.InkResource -= m_spawnFairyPrice;
        }
    }
}
