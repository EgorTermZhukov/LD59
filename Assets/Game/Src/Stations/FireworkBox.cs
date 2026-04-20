using System;
using System.Collections.Generic;
using UnityEngine;

public class FireworkBox : MonoBehaviour, IStation
{
    // Should I use windowGroup for that?
    // Or maybe construct some type of generic station window view?

    // Right now I should completely refactor the parameters out of firework and firework launcher

    public FireworkLauncher FireworkLauncherPfb;
    // reference window pfb

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

    void Start()
    {
        
    }
    void Update()
    {

    }
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
    public void AddLauncher()
    {

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
    }

    public void ShowUpgradeTooltip(UpgradeData upgradeData, int level, Vector2 pointerPosition)
    {
        //UI.Tooltip.Show(etc)
    }

    public string GetID()
    {
        return "Firework Box";
    }

    public void SetWindowReference(StationWindow window)
    {
        StationWindow = window;
    }

    // Some real bullshitting is going to come soon oh boy... 00:11, time to sleep...
    // I hope I will make it on time for the jam mode
}
