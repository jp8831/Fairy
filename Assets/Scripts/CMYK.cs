using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMYK : MonoBehaviour
{
    public static Color CMYKToRGB (Vector4 cmyk)
    {
        float r = (1.0f - cmyk.x) * (1.0f - cmyk.w);
        float g = (1.0f - cmyk.y) * (1.0f - cmyk.w);
        float b = (1.0f - cmyk.z) * (1.0f - cmyk.w);

        return new Color (r, g, b, 1.0f);
    }

    public static Vector4 RGBToCMYK (Color rgb)
    {
        float k = 1.0f - Mathf.Max (rgb.r, rgb.g, rgb.b);
        float c = (1.0f - rgb.r - k) / Mathf.Max (0.0000000001f, 1.0f - k);
        float m = (1.0f - rgb.g - k) / Mathf.Max (0.0000000001f, 1.0f - k);
        float y = (1.0f - rgb.b - k) / Mathf.Max (0.0000000001f, 1.0f - k);

        return new Vector4 (c, m, y, k);
    }
}
