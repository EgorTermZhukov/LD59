using System.Collections.Generic;
using UnityEngine;

public interface ITab
{
    public void Show();
    public void Hide();
    public string GetName();
}

public class TabGroup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public List<(string name, ITab tab)> Tabs;

    public string CurrentOpenedTab = "None";

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(string tabName)
    {
        var tab = Tabs.Find(x=>x.name == tabName);
        if(tab.name == null)
        {
            Debug.LogError($"Tab with the name {tabName} is not registered in {gameObject.name}");
            return;
        }
        if(tab.name == CurrentOpenedTab)
        {
            return;
        }
        tab.tab.Show();
    }
    public void AddTab(ITab tab, TabButton button, string name)
    {
        Tabs.Add(new (name, tab));
        button.Init(this, name);
    }
}
