using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum LauncherState
{
    Idle,
    Startup,
    Reload
}

public class FireworkLauncher : MonoBehaviour
{
    public Firework FireworkPfb;
    public Transform FireworkLaunchAnchor;
    public FireworkBox FireworkBox;


    public TMP_Text CurrentDistanceT;
    public TMP_Text CurrentSpeedT;
    public TMP_Text CurrentAccelerationT;
    public TMP_Text CurrentLifetimeT;


    public LauncherState State = LauncherState.Idle;

    public Firework CurrentChargedFirework;

    public float StartupTimer;
    public float ReloadTimer;

    public void Init(FireworkBox box)
    {
        FireworkBox = box;

        StartupTimer = box.LauncherStartupTimeSeconds;
        StartupFirework();
    }
    void Start()
    {

    }
    void Update()
    {
        if (State == LauncherState.Idle)
            return;
        if (State == LauncherState.Startup)
        {
            StartupTimer -= Time.deltaTime;
            if(StartupTimer <= 0f)
            {
                StartupTimer = FireworkBox.LauncherStartupTimeSeconds;
                ReloadTimer = FireworkBox.LauncherReloadTimeSeconds; 

                State = LauncherState.Reload;

                Debug.Log("Starting the firework!");
                LaunchFirework(CurrentChargedFirework);
            }
        }
        else
        {
            ReloadTimer -= Time.deltaTime;
            if(ReloadTimer <= 0f)
            {
                StartupTimer = FireworkBox.LauncherStartupTimeSeconds;
                StartupFirework();
                State = LauncherState.Startup;
            }
        }
    }

    public void StartupFirework()
    {
        State = LauncherState.Startup;

        Debug.Log("Starting up the firework...");

        var firework = Instantiate(FireworkPfb, FireworkLaunchAnchor.position, Quaternion.identity);
        CurrentChargedFirework = firework;

        firework.Init(FireworkBox, this);

        FireworkBox.AddFirework(firework);
    }
    public void LaunchFirework(Firework firework)
    {
        firework.Launch();
        CurrentChargedFirework = null;
    }

    // Will move this to box if I have time, this isn't really needed
    public void EvaluateAndShowStats(Firework firework)
    {
        float overallDistanceMeters = (firework.transform.position.y - FireworkLaunchAnchor.position.y) * G.main.MetersPerUnit;
        var distanceRounded = Math.Round(overallDistanceMeters, 2);
        CurrentLifetimeT.text = $"{firework.Lifetime}s";
        CurrentSpeedT.text = $"{firework.Speed}m/s";
        CurrentAccelerationT.text = $"{firework.Acceleration}m2/s";
        CurrentDistanceT.text = $"{distanceRounded}m";
    }
}
