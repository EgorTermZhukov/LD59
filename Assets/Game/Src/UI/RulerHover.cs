using UnityEngine;
using UnityEngine.EventSystems;

public class RulerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        G.main.ShowRuler();
        Debug.Log("Showing ruler");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        G.main.HideRuler();
        Debug.Log("Hiding ruler");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
