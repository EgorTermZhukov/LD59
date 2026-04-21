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
    public void Set(string text, float localX)
    {
        //6f / G.main.PixelsPerUnit
        Text.text = text;
        var rect = GetComponent<RectTransform>();

        float xNudge = 2f / G.main.PixelsPerUnit;
        float xDir = localX > 0 ? -xNudge :xNudge;
        rect.DOLocalMoveY(2f / G.main.PixelsPerUnit, 1f, true).SetEase(Ease.OutCubic).OnComplete(() => Destroy(gameObject));
        rect.DOLocalMoveX(xDir, 1f, true).SetEase(Ease.OutCubic);
    }
}
