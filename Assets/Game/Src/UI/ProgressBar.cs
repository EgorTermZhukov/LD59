using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image GhostImage;
    public Image FillImage;

    public float CurrentProgress;
    public float PreviousProgress;

    void Start()
    {
        CurrentProgress = FillImage.fillAmount;
        PreviousProgress = FillImage.fillAmount;
    }
    void Update()
    {
        
    }
    public void SetValue(float fillAmount, bool ghosting = false, float ghostingSpeed = 1f)
    {
        FillImage.DOKill();
        PreviousProgress = CurrentProgress;

        var boundAmount = Mathf.Clamp(fillAmount, 0f, 1f);
        CurrentProgress = boundAmount;

        if (ghosting)
        {
            GhostImage.fillAmount = CurrentProgress;
            FillImage.DOFillAmount(CurrentProgress, ghostingSpeed).SetEase(Ease.OutExpo);
        }
        else
        {
            FillImage.fillAmount = CurrentProgress;
            GhostImage.fillAmount = CurrentProgress;
        }
    }
}
