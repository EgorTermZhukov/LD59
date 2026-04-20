using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeData Data;
    public int Level;
    public StationWindow StationWindow;

    public Button BuyButton;
    public Image ButtonBG;
    public TMP_Text ButtonText;

    public Color BuyButtonOriginalColor;
    public Color BuyButtonTint;


    public TMP_Text UpgradeNameT;
    public TMP_Text UpgradeCostT;

    public float Cost;
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
    public void Refresh()
    {
        if(Level > MaxLevel)
            return;
        var cost = Data.GetCost(Level);

        if(G.main.Coins < cost)
        {
            BuyButton.enabled = false;
            UpgradeCostT.color = Color.red;
            ButtonBG.color = BuyButtonTint;
            ButtonText.color = Color.grey;
        }
        else
        {
            BuyButton.enabled = true;
            UpgradeCostT.color = Color.white;
            ButtonBG.color = BuyButtonOriginalColor;
            ButtonText.color = Color.white;
        }
    }
    public void SetLevelAndCost(int level)
    {
        Level = level;
        UpgradeNameT.text = FormatUpgradeName();
        if(level > MaxLevel)
        {
            BuyButton.gameObject.SetActive(false);
            UpgradeCostT.text = "Max";
            return;
        }
        var cost = Data.GetCost(level);
        SetCost(cost);
    }
    public void SetCost(float cost)
    {
        string costText = G.ui.FormatString(cost);
        UpgradeCostT.text = costText;
    }
    public string FormatUpgradeName()
    {
        var availableLevels = Data.GetAvaliableLevels();
        if(Level == 0)
        {
            return Data.name;
        }
        else if(Level > availableLevels)
        {
            return $"{Data.name}";
        }
        else
        {
            return $"{Data.name} {Level}";
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        G.ui.HideUpgradeTooltip();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        var availableLevels = Data.GetAvaliableLevels();

        // penis
        Debug.Log("-------");
        Debug.Log($"Hovering over {Data.id}");
        Debug.Log($"AvailableLevels: {availableLevels}, current level: {Level}");
        Debug.Log("-------");

        var tooltipPosition = eventData.position;
        StationWindow.Station.ShowUpgradeTooltip(Data, Level, tooltipPosition);
    }
    public void TryBuy()
    {
        if(Level > MaxLevel)
            return;
        StationWindow.TryBuyUpgrade(Data, Level, this);
    }
}
