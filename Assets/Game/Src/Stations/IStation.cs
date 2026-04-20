using UnityEngine;

public interface IStation
{
    public string GetID();
    public void ReceiveUpgrade(UpgradeData ugpradeData, int level);
    public void ShowUpgradeTooltip(UpgradeData upgradeData, int level, Vector2 pointerPosition);
    public void Init(StationWindow window);
    public void ShowUnlockAllert();
    public void HideUnlockAllert();
    public void Lock();
    public void Unlock();
    public Sprite GetIcon();
}
