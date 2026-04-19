using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
    public TMP_Text CoinsT;
    public float LatestValue;
    public RectTransform CoinIcon;

    public float CoinJumpDistance;

    public Vector3 InitialCoinPosition;
    public Vector3 InitialTextPosition;
    void Start()
    {
        InitialCoinPosition = CoinIcon.position;
        InitialTextPosition = CoinsT.transform.position;
        CoinJumpDistance = 5 / G.main.PixelsPerUnit;
    }
    void Update()
    {
        
    }
    public void Set(float coins)
    {
        var delta = coins - LatestValue;
        CoinsT.text = G.ui.FormatString(coins);
        if (coins <= 0)
            return;

        CoinsT.DOKill();
        CoinsT.transform.localScale = Vector3.one;


        //Vector3 punchDir = new Vector3(0f, 2f / G.main.PixelsPerUnit, 0f);
        //CoinsT.transform.DOPunchPosition(punchDir, 0.2f, 1, 0f);

        //Vector3 punchScale = new Vector3(0f, -0.2f, 0f);
        //CoinsT.transform.DOPunchScale(punchScale, 0.2f, 1, 0f);
    }
}
