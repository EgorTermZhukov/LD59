using UnityEngine;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup TabGroup;
    public string TabName;

    public void Init(TabGroup tabGroup, string tabName)
    {
        TabGroup = tabGroup;
        TabName = tabName;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        TabGroup.Show(TabName);

        // Display
        if(TabGroup.CurrentOpenedTab == TabName)
            return;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
