using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pressAnyUI;
    public GameObject mainUI;

    void Update ()
    {
        if (Input.anyKey)
        {
            pressAnyUI.SetActive (false);
            mainUI.SetActive (true);
        }
    }

    public void OnPressPlay ()
    {
        SceneManager.LoadScene ("Play");
    }
}
