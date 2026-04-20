using UnityEngine;
using UnityEngine.UI;


public class CameraSlider : MonoBehaviour
{
    public Camera Cam;
    public Slider Slider;

    // The top boundary wall transform - camera will stop before it becomes visible
    public Transform TopWall;

    private float _minY;   // starting camera Y (bottom bound)
    private float _maxY;   // highest Y camera can reach before wall enters screen

    void Start()
    {
        _minY = Cam.transform.position.y;

        // Calculate how far up the camera can go before the wall enters view.
        // orthographicSize is half the screen height in world units.
        // So the top edge of the camera viewport is camY + orthographicSize.
        // We want: camY + orthographicSize <= wallY
        // Therefore: camY <= wallY - orthographicSize
        _maxY = TopWall.position.y - Cam.orthographicSize;

        // Clamp in case scene is set up so wall is already within starting view
        _maxY = Mathf.Max(_maxY, _minY);

        Slider.minValue = 0f;
        Slider.maxValue = 1f;
        Slider.value = 0f;

        Slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnDestroy()
    {
        Slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        var pos = Cam.transform.position;
        pos.y = Mathf.Lerp(_minY, _maxY, value);
        Cam.transform.position = pos;
    }
}