using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (AspectRatioFitter)), ExecuteInEditMode]
public class UIImageFill : MonoBehaviour
{
    private void OnEnable()
    {
        var fitter = GetComponent<AspectRatioFitter>();
        var imageSize = GetComponent<Image>().sprite.rect.size;

        fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        fitter.aspectRatio = imageSize.y == 0.0f ? 0.0f : imageSize.x / imageSize.y;
    }
}
