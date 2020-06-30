using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    public float OutlineThickness
    {
        set { m_spriteRenderer.material.SetFloat ("_Thickness", value); }
    }

    public Color OutlineColor
    {
        set { m_spriteRenderer.material.SetColor ("_OutlineColor", value); }
    }
}
