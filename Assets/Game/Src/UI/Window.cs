using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Window : MonoBehaviour
{
    public bool Opened = false;
    public TMP_Text Header;

    public string OpenSound;
    public string ClosedSound;


    public UnityEvent OnWindowOpen;
    public UnityEvent OnWindowClosed;

    //SetSorting Order when focused

    public void Open()
    {
        OnWindowOpen?.Invoke();
        Opened = true;
        gameObject.SetActive(Opened);
        AudioController.Instance.PlaySound2D(OpenSound);
    }

    public void Close()
    {
        OnWindowClosed?.Invoke();
        Opened = false;
        gameObject.SetActive(Opened);
        AudioController.Instance.PlaySound2D(ClosedSound);
    }

    void Start()
    {
        
    }
    void Update()
    {
    }
    public void SetTitle(string text)
    {
        if (Header != null)
            Header.text = text;
    }
}
