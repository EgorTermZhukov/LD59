using System.Collections.Generic;
using UnityEngine;

public class UpgradeTab : MonoBehaviour, ITab
{
    public Transform Layouter;
    public StationWindow StationWindow;

    public UpgradePanel PanelPfb;

    public List<UpgradePanel> Panels;

    public string Name;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Init(string name)
    {
        Name = name;
    }
    public UpgradePanel AddUpgradePanel(UpgradeData upgradeData)
    {
        var panel = CreateUpgradePanel(upgradeData);
        return panel;
    }
    public UpgradePanel CreateUpgradePanel(UpgradeData upgradeData)
    {
        var panel = Instantiate(PanelPfb, Layouter);
        panel.Init(upgradeData, StationWindow);
        Panels.Add(panel);
        return panel;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public string GetName()
    {
        return Name;
    }

}
