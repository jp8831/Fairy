using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_backgroundMusicAudio;
    [SerializeField]
    private AudioSource m_uiAudio;
    [SerializeField]
    private AudioSource m_gameAudio;

    public void SetBackgroundMusic (AudioClip clip)
    {
        m_backgroundMusicAudio.clip = clip;
        m_backgroundMusicAudio.time = 0.0f;
    }

    public void PlayUIAudio (AudioClip clip)
    {
        m_uiAudio.PlayOneShot (clip);
    }

    public void PlayGameAudio (AudioClip clip)
    {
        m_gameAudio.PlayOneShot (clip);
    }
}
