using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] m_spriteRenderers;

    public float OutlineThickness
    {
        set
        {
            foreach (var spriteRenderer in m_spriteRenderers)
            {
                spriteRenderer.material.SetFloat ("_Thickness", value);
            }
        }
    }

    public Color OutlineColor
    {
        set
        {
            foreach (var spriteRenderer in m_spriteRenderers)
            {
                spriteRenderer.material.SetColor ("_OutlineColor", value);
            }
        }
    }
}
