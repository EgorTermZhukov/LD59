using UnityEngine;

public class Guy : MonoBehaviour, IStation
{
    public GameObject QuestionMark;
    public StationWindow StationWindow;
    public SpriteRenderer Sprite;
    public string GetID()
    {
        return "Guy";
    }

    public void Init(StationWindow window)
    {
        StationWindow = window;
    }

    public void ReceiveUpgrade(UpgradeData upgradeData, int level)
    {
        if (!StationWindow.IsStationUnlocked)
        {
            Debug.LogError("Station has not been unlocked");
            return;
        }
        //Big switch case for every upgrade in the game
        Debug.Log($"Buying upgrade {upgradeData.id} for {GetID()} with the level {level}");
        switch (upgradeData.type)
        {
            case UpgradeType.IncCoinsPerMeter:
                G.main.CoinsPerMeter += upgradeData.incCoinsPerMeter[level].v;
                break;
            default:
                Debug.LogError($"Upgrade {upgradeData.id} is not supported for station {GetID()}");
                break;
        }
    }
    public void ShowUnlockAllert()
    {
        QuestionMark.SetActive(true);
        Debug.Log("Alert shown");
    }

    public void HideUnlockAllert()
    {
        Debug.Log("Alert hidden");
        QuestionMark.SetActive(false);
    }
    public void ShowUpgradeTooltip(UpgradeData upgradeData, int level, Vector2 pointerPosition)
    {
        Debug.Log($"Showing upgrade tooltip for {GetID()}");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Unlock()
    {
        StationWindow.Unlock();
        Sprite.color = Color.white;
        // play puff animation or smth like that
    }
    public void Lock()
    {
        StationWindow.Lock();
        Sprite.color = Color.red;
    }

    public Sprite GetIcon()
    {
        return GetComponentInChildren<SpriteRenderer>().sprite;
    }
}
