using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameRule : GameRule
{
    [SerializeField]
    private string m_mainGameSceneName;

    [Header ("Painter")]
    [SerializeField]
    private Sprite[] m_miniGameSprites;
    [SerializeField]
    private Painter m_painter;
    [SerializeField]
    private string m_cyanSliderName;
    [SerializeField]
    private string m_magentaSliderName;
    [SerializeField]
    private string m_yellowSliderName;
    [SerializeField]
    private string m_keySliderName;
    [SerializeField]
    private string m_brushSizeSliderName;
    [SerializeField]
    private string m_clearButtonName;
    [SerializeField]
    private string m_targetImageName;
    [SerializeField]
    private string m_timerTextName;

    [Header ("Score")]
    [SerializeField]
    private string m_scoreUIName;
    [SerializeField]
    private string m_scoreTextName;
    [SerializeField]
    private string m_exitButtonName;

    private UISlider m_cSlider;
    private UISlider m_mSlider;
    private UISlider m_ySlider;
    private UISlider m_kSlider;
    private UISlider m_brushSizeSlider;
    private UIButton m_clearButton;
    private UIImage m_targetImage;
    private UIElement m_scoreUI;
    private UIText m_scoreText;
    private UIButton m_exitButton;

    private Timer m_timer;
    private bool m_bFinished;

    public bool Finished
    {
        get { return m_bFinished; }
    }

    private void Start ()
    {
        m_cSlider = UIController.FindUI<UISlider> (m_cyanSliderName);
        m_mSlider = UIController.FindUI<UISlider> (m_magentaSliderName);
        m_ySlider = UIController.FindUI<UISlider> (m_yellowSliderName);
        m_kSlider = UIController.FindUI<UISlider> (m_keySliderName);

        m_brushSizeSlider = UIController.FindUI<UISlider> (m_brushSizeSliderName);
        m_brushSizeSlider.MinValue = m_painter.MinBrushSize;
        m_brushSizeSlider.MaxValue = m_painter.MaxBrushSize;
        m_brushSizeSlider.Value = 0.5f * (m_painter.MinBrushSize + m_painter.MaxBrushSize);

        m_clearButton = UIController.FindUI<UIButton> (m_clearButtonName);
        m_clearButton.AddOnClickListener (ClearImage);

        m_targetImage = UIController.FindUI<UIImage> (m_targetImageName);
        m_targetImage.Sprite = m_miniGameSprites[Random.Range (0, m_miniGameSprites.Length)];

        m_timer = GetComponent<Timer> ();
        m_timer.RemainTimeUI = UIController.FindUI<UIText> (m_timerTextName);
        m_timer.EndEvent.AddListener (FinishGame);

        m_scoreUI = UIController.FindUI<UIElement> (m_scoreUIName);
        m_scoreText = UIController.FindUI<UIText> (m_scoreTextName);

        m_exitButton = UIController.FindUI<UIButton> (m_exitButtonName);
        m_exitButton.AddOnClickListener (Exit);
    }

    private void OnDestroy ()
    {
        m_clearButton.RemoveOnClickListener (ClearImage);
        m_timer.EndEvent.RemoveListener (FinishGame);
        m_exitButton.RemoveOnClickListener (Exit);
    }

    public override void OnPlayStart ()
    {
        base.OnPlayStart ();

        m_bFinished = false;
        UIController.DeactivateUI (m_scoreUIName);
    }

    public override void OnPlay ()
    {
        m_painter.BrushColorCMYK = new Vector4 (m_cSlider.Value, m_mSlider.Value, m_ySlider.Value, m_kSlider.Value);
        m_painter.BrushSize = m_brushSizeSlider.Value;
    }

    public override void OnPlayEnd ()
    {
        base.OnPlayEnd ();
    }

    private void ClearImage ()
    {
        m_painter.ClearPaintTexture ();
    }

    private void FinishGame ()
    {
        m_bFinished = true;

        float colorDiff = 0.0f;
        var paintTexture = m_painter.PaintTexture;

        for (int x = 0; x < paintTexture.width; x++)
        {
            for (int y = 0; y < paintTexture.height; y++)
            {
                Color color = paintTexture.GetPixel (x, y);
                Color targetColor = m_targetImage.Sprite.texture.GetPixelBilinear ((float) x / paintTexture.width, (float) y / paintTexture.height);
                float diff = Vector3.Distance (ColorConvertor.RGBToYUV (color), ColorConvertor.RGBToYUV (targetColor));

                colorDiff += diff;
            }
        }

        float score = colorDiff / (paintTexture.width + paintTexture.height);
        m_scoreText.Value = $"{score:F0}";

        UIController.ActivateUI (m_scoreUIName);
    }

    private void Exit ()
    {
        GameController.StartPlay (m_mainGameSceneName);
    }
}
