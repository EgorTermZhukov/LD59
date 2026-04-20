using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;


public enum PopupType
{
    Coin
}
public class UI : MonoBehaviour
{
    public Canvas Root;
    public CoinsDisplay CoinsDisplay;

    public TextPopup CoinPopup;

    public StationWindow StationWindowPfb;

    public Canvas WindowCanvas;
    public List<StationWindow> StationWindows;
    void Awake()
    {
        G.ui = this;
    }
    public void UpdateCoins(float coins)
    {
        // Update all ui elements
        CoinsDisplay.Set(coins);

        foreach(var window in StationWindows)
        {
            window.UpdateAllTabs();
        }
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
        popup.Set(text);
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
}
