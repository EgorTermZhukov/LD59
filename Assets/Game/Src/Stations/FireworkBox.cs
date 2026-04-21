using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FireworkBox : MonoBehaviour, IStation
{
    // Should I use windowGroup for that?
    // Or maybe construct some type of generic station window view?

    // Right now I should completely refactor the parameters out of firework and firework launcher
    public SpriteRenderer Sprite;
    public GameObject QuestionMark;

    public FireworkLauncher LauncherPfb;

    public float LauncherSpacingPixels = 16;


    public int MaxLaunchers;

    public float FireworkLifetime;

    public float FireworkStartSpeed;
    public float Acceleration;
    public float Drag;

    public float FrequencyX;
    public float FrequencyY;

    public float WobbleDelay;

    public float LauncherReloadTimeSeconds;
    public float LauncherStartupTimeSeconds;

    public StationWindow StationWindow;

    public List<FireworkLauncher> Launchers;
    public List<Firework> ActiveFireworks;

    public void EvaluateExplosion(Firework firework)
    {
        // change launcher anchor to ground anchor
        var fireworkPosition = firework.transform.position;
        float overallDistanceMeters = (firework.transform.position.y - G.main.RulerStartAnchor.position.y) * G.main.MetersPerUnit;
        var distanceRounded = (float)Math.Round(overallDistanceMeters, 2);
        ActiveFireworks.Remove(firework);

        // may move explode here in case I want to apply effects and such
        Debug.Log($"Firework flew for {firework.InitialLifetime}s");
        firework.Explode();
        G.main.EvaluateCoinsPerDistanceAndSpawnPopup(distanceRounded, this, fireworkPosition);
    }
    public void AddFirework(Firework firework)
    {
        ActiveFireworks.Add(firework);
    }
    public void AddLauncherAndStart()
    {
        if(Launchers.Count > MaxLaunchers)
        {
            Debug.LogError($"Cannot add more than {MaxLaunchers} launchers");
            return;
        }
        // Move all launchers to the left because i have no idea how to add layouts
        var launcherPosition = transform.position + ((float)LauncherSpacingPixels / G.main.PixelsPerUnit) * (Launchers.Count + 1) * Vector3.left;
        var launcher = Instantiate(LauncherPfb, launcherPosition, quaternion.identity);
        Launchers.Add(launcher);
        launcher.Init(this);
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
            case UpgradeType.IncLifetime:
                FireworkLifetime += upgradeData.incLifetime[level].v;
                break;
            case UpgradeType.IncSpeed:
                FireworkStartSpeed += upgradeData.incSpeed[level].v;
                break;
            case UpgradeType.IncFireworkCount:
                AddLauncherAndStart();
                break;
            case UpgradeType.DecStartupTime:
                LauncherStartupTimeSeconds -= upgradeData.decStartupTime[level].v;
                LauncherReloadTimeSeconds -= upgradeData.decStartupTime[level].v / 2;
                break;
            default:
                Debug.LogError($"Upgrade {upgradeData.id} is not supported for station {GetID()}");
                break;
        }
    }

    public void ShowUpgradeTooltip(UpgradeData upgradeData, int level, Vector2 pointerPosition)
    {
        //UI.Tooltip.Show(etc)
    }

    public string GetID()
    {
        return "Firework Box";
    }

    public void Init(StationWindow window)
    {
        StationWindow = window;
        MaxLaunchers = BaseValues.maxLaunchers;
        FireworkLifetime = BaseValues.lifetime;
        FireworkStartSpeed = BaseValues.startSpeed;
        Acceleration = BaseValues.acceleration;
        Drag = BaseValues.drag;
        FrequencyX = BaseValues.wobbleFreqX;
        FrequencyY = BaseValues.wobbleFreqY;
        WobbleDelay = BaseValues.wobbleDelay;
        LauncherReloadTimeSeconds = BaseValues.reloadTime;
        LauncherStartupTimeSeconds = BaseValues.startupTime;
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

    public Sprite GetIcon()
    {
        return GetComponentInChildren<SpriteRenderer>().sprite;
    }
    // Some real bullshitting is going to come soon oh boy... 00:11, time to sleep...
    // I hope I will make it on time for the jam mode
}
