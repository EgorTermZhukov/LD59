using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct WindowReferenceData
{
    public string windowName;
    public Window reference;
}

public class WindowGroup : MonoBehaviour
{
    [SerializeField]
    public List<WindowReferenceData> RegisteredWindows;
    public Window CurrentOpenedWindow;
    public Window PlaceholderWindow;

    public bool IsLocked = false;

    void Start()
    {
        if(CurrentOpenedWindow != null)
            CurrentOpenedWindow.Open();
    }

    void Update()
    {
        
    }

    public void OpenWindow(string windowName)
    {
        if(IsLocked)
            return;
        var window = RegisteredWindows.Find(x => x.windowName == windowName);

        if (window.reference == null)
        {
            CloseCurrentlyOpenedWindow();
            PlaceholderWindow.SetTitle(windowName);
            PlaceholderWindow.Open();
            CurrentOpenedWindow = PlaceholderWindow;
            return;
        }
        if (CurrentOpenedWindow == window.reference)
            return;

        CloseCurrentlyOpenedWindow();
        window.reference.Open();
        CurrentOpenedWindow = window.reference;
    }
    public void CloseCurrentlyOpenedWindow()
    {
        if (CurrentOpenedWindow == null)
            return;
        CurrentOpenedWindow.Close();
        CurrentOpenedWindow = null;
    }
    public void LockCurrentWindow()
    {
        IsLocked = true;
    }
    public void UnlockCurrentWindow()
    {
        IsLocked = false;
    }
}
