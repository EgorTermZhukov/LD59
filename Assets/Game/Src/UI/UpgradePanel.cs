using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeData Data;
    public int level;
    public StationWindow StationWindow;

    public BuyButton BuyButton;

    public TMP_Text UpgradeNameT;
    public TMP_Text UpgradeCostT;

    public float Cost;
    public int CurrentLevel;
    public int MaxLevel;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Init(UpgradeData data, StationWindow stationWindow)
    {
        Data = data;
        StationWindow = stationWindow;
        MaxLevel = data.GetAvaliableLevels();
        // Set text
    }
    public void UpdateButton()
    {

    }
    public void SetLevelAndCost(int level)
    {
        CurrentLevel = level;
        UpgradeNameT.text = FormatUpgradeName();
        if(level > MaxLevel)
        {
            BuyButton.gameObject.SetActive(false);
            UpgradeCostT.text = "-";
            return;
        }
        var cost = Data.GetCost(level);
        SetCost(cost);
    }
    public void SetCost(float cost)
    {
        var availableLevels = Data.GetAvaliableLevels();

        string costText = G.ui.FormatString(cost);
        if(availableLevels > level)
        {
            costText = "-";
        }
        UpgradeCostT.text = costText;
    }
    public string FormatUpgradeName()
    {
        var availableLevels = Data.GetAvaliableLevels();
        if(level == 0)
        {
            return Data.name;
        }
        else if(level > availableLevels)
        {
            return $"{Data.name} (max)";
        }
        else
        {
            return $"{Data.name} {level}";
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        G.ui.HideUpgradeTooltip();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        var tooltipPosition = eventData.position;
        StationWindow.Station.ShowUpgradeTooltip(Data, level, tooltipPosition);
    }
    public void TryBuy()
    {
        if(level > MaxLevel)
            return;
        StationWindow.TryBuyUpgrade(Data, level, this);
    }
}
