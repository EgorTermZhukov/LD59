using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPopup : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Text;


    void Start()
    {
    }
    void Update()
    {
    }
    public void SetAndFlyToTheCenter(string text, float localX)
    {
        Text.text = text;
        float ppu = G.main.PixelsPerUnit;

        // Random drift distance, biased away from center on X
        float xNudge = Random.Range(1f, 3f) / ppu;
        float yNudge = Random.Range(1.5f, 3.5f) / ppu;

        float xDir = localX > 0 ? -xNudge : xNudge;
        // Small random horizontal scatter so stacked popups spread apart
        xDir += Random.Range(-0.5f, 0.5f) / ppu;

        transform.DOLocalMoveY(yNudge, 1f, true).SetEase(Ease.OutCubic).OnComplete(() => Destroy(gameObject));
        transform.DOLocalMoveX(xDir, 1f, true).SetEase(Ease.OutCubic);
    }
}
