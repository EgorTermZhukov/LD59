using System.Collections;
using UnityEngine;
using TMPro;

public class NameTextureGenerator : MonoBehaviour
{
    [Header("Offscreen Rendering")]
    public Camera CaptureCamera;        // the dedicated capture camera
    public RenderTexture RenderTex;     // the RenderTexture the capture camera draws to
    public TMP_Text OffscreenText;      // TMP_Text on the offscreen canvas

    [Header("Particles")]
    public ParticleSystem NameParticles;

    [Header("Texture Settings")]
    // Threshold below which a pixel is considered transparent (0-1)
    // Increase if particles appear in the background, decrease if letters look sparse
    public float AlphaThreshold = 0.5f;

    private Texture2D _capturedTexture;

    public void Capture(string name)
    {
        OffscreenText.text = name;

        // Force TMP to rebuild its mesh before we capture so the
        // text is fully rendered into the RenderTexture this frame
        Canvas.ForceUpdateCanvases();
        OffscreenText.ForceMeshUpdate();

        StartCoroutine(CaptureAndApply());
    }

    private IEnumerator CaptureAndApply()
    {
        yield return new WaitForEndOfFrame();

        if (_capturedTexture != null)
            Destroy(_capturedTexture);

        _capturedTexture = new Texture2D(RenderTex.width, RenderTex.height, TextureFormat.RGBA32, false);

        RenderTexture previouslyActive = RenderTexture.active;
        RenderTexture.active = RenderTex;
        _capturedTexture.ReadPixels(new Rect(0, 0, RenderTex.width, RenderTex.height), 0, 0);
        _capturedTexture.Apply();
        RenderTexture.active = previouslyActive;

        // Apply texture as a mask onto the shape — no shapeType change needed
        var shape = NameParticles.shape;
        shape.enabled = true;
        shape.texture = _capturedTexture;
        shape.textureAlphaAffectsParticles = true;
        shape.textureClipChannel = ParticleSystemShapeTextureChannel.Alpha;
        shape.textureClipThreshold = AlphaThreshold;

        // No Play() call — particle system is ready for you to use however you want
    }

    void OnDestroy()
    {
        if (_capturedTexture != null)
            Destroy(_capturedTexture);
    }
}