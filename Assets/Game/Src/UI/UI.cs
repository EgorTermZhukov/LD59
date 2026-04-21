using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum PopupType
{
    Coin
}
public class UI : MonoBehaviour
{
    public Image Fader;
    public Canvas Root;
    public CoinsDisplay CoinsDisplay;


    public TextPopup CoinPopup;

    public StationWindow StationWindowPfb;

    public Canvas WindowCanvas;
    public Canvas DisplayCanvas;
    public Canvas EndScreen;

    public List<StationWindow> StationWindows;

    public GameObject InputBoxParent;
    public TMP_InputField InputField;

    public bool PersonNameHasBeenSelected = false;

    public ProgressBar ProgressBar;

    public TMP_Text WinConditionText;
    public TMP_Text PersonNameText;


    void Awake()
    {
        G.ui = this;
    }
    void Start()
    {
        WinConditionText.text = FormatString(BaseValues.winCondition);
    }
    public void UpdateCoins(float coins)
    {
        // Update all ui elements
        CoinsDisplay.Set(coins);

        foreach(var window in StationWindows)
        {
            window.UpdateAllTabs();
        }
        ProgressBar.SetValue(coins/BaseValues.winCondition, true);
    }

    void Update()
    {
        
    }
    public string FormatString(float value)
    {
        return value switch
        {
            >= 1_000_000 => (value / 1_000_000f).ToString("0.#") + "M",
            >= 1_000 => (value / 1_000f).ToString("0.#") + "k",
            _ => value.ToString("0.#")
        };
    }
    public void SpawnPopup(string text, PopupType type, Vector3 worldPosition)
    {
        var popup = Instantiate(CoinPopup, Root.GetComponent<RectTransform>());

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
        Vector2 localPoint;
        if (Root.renderMode == RenderMode.ScreenSpaceCamera)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Root.GetComponent<RectTransform>(), screenPoint, Camera.main, out localPoint);
        else
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Root.GetComponent<RectTransform>(), screenPoint, null, out localPoint);

        popup.GetComponent<RectTransform>().anchoredPosition = localPoint;
        popup.Set(text, localPoint.x);
    }
    public StationWindow CreateStationWindowAndHide(IStation station, Vector3 worldPosition)
    {
        var window = Instantiate(StationWindowPfb, WindowCanvas.GetComponent<RectTransform>());
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
        Vector2 localPoint;
        if (Root.renderMode == RenderMode.ScreenSpaceCamera)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Root.GetComponent<RectTransform>(), screenPoint, Camera.main, out localPoint);
        else
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Root.GetComponent<RectTransform>(), screenPoint, null, out localPoint);
        window.GetComponent<RectTransform>().anchoredPosition = localPoint;
        window.Init(station);
        window.gameObject.SetActive(false);
        window.SetHeader(station.GetID());
        StationWindows.Add(window);
        return window;
    }
    public void HideUpgradeTooltip()
    {
    }
    public void ShowUpgradeTooltip(string formattedStringToShow)
    {
    }

    public void HideHUD()
    {
        DisplayCanvas.gameObject.SetActive(false);
        WindowCanvas.gameObject.SetActive(false);
    }
    public void ShowHUD()
    {
        DisplayCanvas.gameObject.SetActive(true);
        WindowCanvas.gameObject.SetActive(true);
    }
    public void ShowFade()
    {
        Fader.gameObject.SetActive(true);
    }
    public void FadeIn()
    {
        Fader.DOFade(1f, 0.5f);
    }
    public void FadeOut()
    {
        Fader.DOFade(0f, 0.5f).OnComplete(()=> Fader.gameObject.SetActive(false));
    }

    public void ShowInputBox()
    {
        InputBoxParent.SetActive(true);
    }

    public void HideInputBox()
    {
        InputBoxParent.SetActive(false);
    }
    public IEnumerator AskForPersonName()
    {
        InputField.ActivateInputField();
        while (PersonNameHasBeenSelected == false)
        {
            yield return null;
        }
    }
    public void SetName(string name)
    {
        PersonNameHasBeenSelected = true;
    }
    public IEnumerator DisplayEndscreen()
    {
        PersonNameText.text = G.main.PersonsName;
        Debug.Log($"Persons name: {G.main.PersonsName}");
        foreach(var text in EndScreen.gameObject.GetComponentsInChildren<TMP_Text>())
        {
            text.DOFade(1f, 2f);
            yield return new WaitForSeconds(2f);
        }
        yield break;
    }
}
