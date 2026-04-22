using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraSlider : MonoBehaviour
{
    [Header("Core")]
    public Camera Cam;
    public Slider Slider;
    public Transform TopWall;

    [Header("Scroll")]
    public float ScrollSensitivity = 0.1f;

    [Header("Max Distance Notch")]
    public RectTransform MaxDistanceNotch;
    public TMP_Text MaxDistanceLabel;

    [Header("Height Indicator")]
    public TMP_Text HeightLabel;

    [Header("Help Icon")]
    public GameObject HelpIcon;

    private float _minY;
    private float _maxY;
    private bool _helpIconShown = false;
    private bool _helpIconDismissed = false;

    void Start()
    {
        _minY = Cam.transform.position.y;

        // Camera can scroll until its top edge reaches the wall
        _maxY = TopWall.position.y - Cam.orthographicSize;
        _maxY = Mathf.Max(_maxY, _minY);

        Slider.minValue = 0f;
        Slider.maxValue = 1f;
        Slider.value = 0f;

        Slider.onValueChanged.AddListener(OnSliderChanged);

        if (HelpIcon != null)
            HelpIcon.SetActive(false);

        if (HeightLabel != null)
            HeightLabel.text = "0m";

        if (MaxDistanceNotch != null)
            MaxDistanceNotch.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        Slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            if (!_helpIconDismissed && _helpIconShown)
            {
                _helpIconDismissed = true;
                if (HelpIcon != null)
                    HelpIcon.SetActive(false);
            }
            Slider.value = Mathf.Clamp01(Slider.value + scroll * ScrollSensitivity);
        }
    }

    void OnSliderChanged(float value)
    {
        var pos = Cam.transform.position;
        pos.y = Mathf.Lerp(_minY, _maxY, value);
        Cam.transform.position = pos;

        UpdateHeightLabel(pos.y);
    }

    // Called from Main.cs whenever a new max distance firework lands
    public void UpdateMaxDistanceNotch(float fireworkWorldY)
    {
        // Show help icon the first time a firework goes off the top of the screen
        if (!_helpIconShown && !_helpIconDismissed)
        {
            float camTopEdge = Cam.transform.position.y + Cam.orthographicSize;
            if (fireworkWorldY > camTopEdge)
            {
                _helpIconShown = true;
                StartCoroutine(ShowHelpIconBriefly());
            }
        }

        // Update max distance label
        if (MaxDistanceLabel != null)
        {
            float meters = (fireworkWorldY - G.main.RulerStartAnchor.position.y) * G.main.MetersPerUnit;
            MaxDistanceLabel.text = $"{Mathf.RoundToInt(Mathf.Max(0f, meters))}m";
        }

        // Position notch on slider bar
        // Fraction uses RulerStartAnchor as zero reference — same as every other
        // distance calculation in the game. MetersPerUnit cancels so we work in world units.
        if (MaxDistanceNotch == null) return;
        MaxDistanceNotch.gameObject.SetActive(true);

        float fraction = Mathf.Clamp01(
            (fireworkWorldY - G.main.RulerStartAnchor.position.y) /
            (_maxY - G.main.RulerStartAnchor.position.y)
        );

        RectTransform sliderRect = Slider.GetComponent<RectTransform>();
        float barHeight = sliderRect.rect.height;
        MaxDistanceNotch.anchoredPosition = new Vector2(
            MaxDistanceNotch.anchoredPosition.x,
            Mathf.Lerp(-barHeight * 0.5f, barHeight * 0.5f, fraction)
        );
    }

    private void UpdateHeightLabel(float camWorldY)
    {
        if (HeightLabel == null) return;
        // Same formula as firework distance: (worldY - rulerAnchor) * MetersPerUnit
        float heightMeters = (camWorldY - G.main.RulerStartAnchor.position.y) * G.main.MetersPerUnit;
        HeightLabel.text = $"{Mathf.RoundToInt(Mathf.Max(0f, heightMeters))}m";
    }

    private IEnumerator ShowHelpIconBriefly()
    {
        if (HelpIcon == null) yield break;
        HelpIcon.SetActive(true);
        float elapsed = 0f;
        while (elapsed < 10f && !_helpIconDismissed)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        HelpIcon.SetActive(false);
    }
}