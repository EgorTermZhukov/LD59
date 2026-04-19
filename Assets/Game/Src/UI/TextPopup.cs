using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

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
    public void Set(string text)
    {
        //6f / G.main.PixelsPerUnit
        Text.text = text;
        GetComponent<RectTransform>().DOLocalMoveY(2f / G.main.PixelsPerUnit, 1f, true).SetEase(Ease.OutCubic).OnComplete(()=>Destroy(gameObject));
    }
}
