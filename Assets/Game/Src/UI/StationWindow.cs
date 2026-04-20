using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StationWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Upgrade view will be there too, I will maybe think about a more
    // interesting solution when im done with the gamejam

    public TMP_Text Header;

    public TabGroup TopTabs;

    // ITab.Open close etc later

    // I can later insert upgrade tab into tab group and manage it through this

    public UpgradeTab UpgradeTab;

    public GameObject View;
    public Image Hover;

    public UpgradeTab UpgradeTabPfb;

    public Color ActiveHoverAlphaColor;
    public Color InactiveHoverAlphaColor;

    public List<UpgradeData> Upgrades;

    public IStation Station;

    public bool IsStationUnlocked = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsStationUnlocked)
            return;
        View.SetActive(true);
        Hover.color = ActiveHoverAlphaColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!IsStationUnlocked)
            return;
        View.SetActive(false);
        Hover.color = InactiveHoverAlphaColor;
    }
    public void Init(IStation station)
    {
        UpgradeTab.StationWindow = this;
        Station = station;

        string stationName = station.GetID();
        var allStationUpgrades = CMS.StationsUpgrades.Find(x => x.stationId == stationName);

        if(allStationUpgrades.stationId == null)
        {
            Debug.LogError($"Station with the id {stationName} is invalid");
            return;
        }

        // for now instantiate just one tab without grouping
        var tabData = allStationUpgrades.stationData[0];
        var upgradesID = tabData.upgrades;
        var upgradesToAdd = new List<UpgradeData>(); 

        // I feel like i need to move this to cms in postjam version
        foreach(var id in upgradesID)
        {
            var upgrade = CMS.All.Find(x=>x.id == id);
            if(upgrade.id == null)
                Debug.LogError($"Upgrade with the id {id} is invalid");
            upgradesToAdd.Add(upgrade);
        }
        foreach(var upgrade in upgradesToAdd)
        {
            var panel = UpgradeTab.AddUpgradePanel(upgrade);
            panel.SetLevelAndCost(0);
            panel.Refresh();
        }
    }
    public void SetHeader(string header)
    {
        Header.text = header;
    }
    public void UnlockUpgradeTab(string upgradeTabName)
    {

    }
    public void Unlock()
    {
        if (IsStationUnlocked)
        {
            Debug.LogError("Station has already been unlocked");
        }
        IsStationUnlocked = true;
        gameObject.SetActive(true);
        View.SetActive(false);
    }
    public void Lock()
    {
        IsStationUnlocked = false;
        gameObject.SetActive(false);
    }
    public void TryBuyUpgrade(UpgradeData data, int level, UpgradePanel panel)
    {
        var avaliableLevels = data.GetAvaliableLevels();
        if(avaliableLevels < level)
        {
            Debug.Log($"Cannot buy upgrade with the name {data.name}, cannot increase past level {avaliableLevels}");
            return;
        }
        float cost = data.GetCost(level);
        if(G.main.Coins < cost)
        {
            Debug.Log($"Cannot buy upgrade with the name {data.name} of type {data.type}, with cost {cost}, of level {level}");
            Debug.Log($"Current money {G.main.Coins}< cost {cost}");
            return;
        }
        level++;
        panel.SetLevelAndCost(level);
        panel.Refresh();
        Station.ReceiveUpgrade(data, level);
    }
    public void UpdateAllTabs()
    {
        foreach(var panel in UpgradeTab.Panels)
        {
            panel.Refresh();
        }
    }
}
