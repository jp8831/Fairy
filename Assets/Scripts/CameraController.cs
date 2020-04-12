using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private float m_minSize;
    [SerializeField]
    private float m_maxSize;

    private Camera m_camera;

    void Start ()
    {
        m_camera = GetComponent<Camera> ();
    }

    void Update()
    {
        float h = 0.0f;

        if (Input.GetKey (KeyCode.A))
        {
            h -= 1.0f;
        }
        if (Input.GetKey (KeyCode.D))
        {
            h += 1.0f;
        }

        float v = 0.0f;

        if (Input.GetKey (KeyCode.S))
        {
            v -= 1.0f;
        }
        if (Input.GetKey (KeyCode.W))
        {
            v += 1.0f;
        }

        transform.position += new Vector3 (h, v, 0.0f).normalized * Time.deltaTime;

        m_camera.orthographicSize = Mathf.Clamp (m_camera.orthographicSize + Input.mouseScrollDelta.y, m_minSize, m_maxSize);
    }
}
