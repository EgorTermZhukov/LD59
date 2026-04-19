using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.WSA;

public enum LauncherState
{
    Idle,
    Setup,
    Recharge
}

public class FireworkLauncher : MonoBehaviour
{
    // References
    public Firework FireworkPfb;
    public Transform FireworkLaunchAnchor;


    // Parameters
    public float StartSpeed;

    public float TopSpeed;
    public float StartingAcceleration;
    public float Drag;

    public float MinLifetimeSeconds;
    public float UpperLifetimeVariationSeconds;

    public float SetupTimeSeconds;
    public float RechargeIntervalSeconds;

    public TMP_Text CurrentDistanceT;
    public TMP_Text CurrentSpeedT;
    public TMP_Text CurrentAccelerationT;
    public TMP_Text CurrentLifetimeT;


    public LauncherState State = LauncherState.Idle;

    public Firework CurrentChargedFirework;

    // Timers
    public float SetupTimer;
    public float RechargeTimer;

    // Collections
    public List<Firework> ActiveFireworks;
    public void Init()
    {
        // why does this method exist?
        // oh yeah, parameters, will do it later

        // Redundant parameter
        //TopSpeed = 10f;
        // starting acceleration was at -18
        Drag = 0f;
        StartingAcceleration = 0f;
        StartSpeed = 10f;
        MinLifetimeSeconds = 0.5f;
        UpperLifetimeVariationSeconds = 0f;
        SetupTimeSeconds = 4f;


        SetupTimer = SetupTimeSeconds;
        StartSetupFirework();
    }
    void Start()
    {

    }
    void Update()
    {
        if (State == LauncherState.Idle)
            return;
        if (State == LauncherState.Setup)
        {
            SetupTimer -= Time.deltaTime;
            if(SetupTimer <= 0f)
            {
                SetupTimer = SetupTimeSeconds;
                LaunchFirework(CurrentChargedFirework);
                RechargeTimer = CurrentChargedFirework.Lifetime; 
                CurrentChargedFirework = null;
                Debug.Log("Starting the firework!");
                State = LauncherState.Recharge;
            }
        }
        else
        {
            RechargeTimer -= Time.deltaTime;
            if(RechargeTimer <= 0f)
            {
                RechargeTimer = RechargeIntervalSeconds;
                StartSetupFirework();
                State = LauncherState.Setup;
            }
        }
    }
    public void EvaluateExplosion(Firework firework)
    {
        float overallDistanceMeters = (firework.transform.position.y - FireworkLaunchAnchor.position.y) * G.main.MetersPerUnit;
        var distanceRounded = (float)Math.Round(overallDistanceMeters, 2);
        ActiveFireworks.Remove(firework);

        // may move explode here in case I want to apply effects and such
        Debug.Log($"Firework flew for {firework.InitialLifetime}s");
        firework.Explode();
        G.main.EvaluateCoinsPerDistance(distanceRounded);
    }
    public void StartSetupFirework()
    {
        Debug.Log("Starting up the firework...");
        // change to create firework later, or move some kind of puff effect to the start method of firework
        var firework = Instantiate(FireworkPfb, FireworkLaunchAnchor.position, Quaternion.identity);
        CurrentChargedFirework = firework;
        firework.Setup(this);
        ActiveFireworks.Add(firework);
        State = LauncherState.Setup;
    }
    public void LaunchFirework(Firework firework)
    {
        firework.Launch();
    }

    public void EvaluateAndShowStats(Firework firework)
    {
        float overallDistanceMeters = (firework.transform.position.y - FireworkLaunchAnchor.position.y) * G.main.MetersPerUnit;
        var distanceRounded = Math.Round(overallDistanceMeters, 2);
        CurrentLifetimeT.text = $"{firework.Lifetime}s";
        CurrentSpeedT.text = $"{firework.Speed}m/s";
        CurrentAccelerationT.text = $"{firework.Acceleration}m2/s";
        CurrentDistanceT.text = $"{distanceRounded}m";
    }
    /*
    public IEnumerator LaunchFirework()
    {
        CurrentTravelledDistanceMetersT.text = "Travelled meters: 0";

        var firework = Instantiate(FireworkPfb, FireworkLaunchAnchor.position, Quaternion.identity);

        //var shakeTween = firework.transform.DOShakePosition(ChargeInterval, 0.7f, 2, 5);
        yield return new WaitForSeconds(ChargeInterval);

        //shakeTween.Kill();
        firework.transform.position = FireworkLaunchAnchor.position;

        LifeTimeTimer = UnityEngine.Random.Range(MinLifetimeSeconds, MinLifetimeSeconds + UpperLifetimeVariationSeconds);

        // should be in some kind of update function
        var flyTween = firework.transform.DOMove(FireworkLaunchAnchor.position + new Vector3(0, LifeTimeTimer * FireworkSpeed * MetersPerUnit), LifeTimeTimer).SetEase(Ease.Linear);
        int overallDistanceMeters = 0;
        // calculate distance every frame
        while (LifeTimeTimer > 0)
        {
            overallDistanceMeters = (int)((firework.transform.position.y - FireworkLaunchAnchor.position.y) / MetersPerUnit);
            var distanceRounded = Math.Round((double)overallDistanceMeters, 2);
            CurrentTravelledDistanceMetersT.text = $"Travelled meters: {distanceRounded}m";

            LifeTimeTimer -= Time.deltaTime;
            yield return null;
        }
        flyTween.Kill();

        overallDistanceMeters = (int)((firework.transform.position.y - FireworkLaunchAnchor.position.y) / MetersPerUnit);
        var rounded = Math.Round((double)overallDistanceMeters, 2);
        CurrentTravelledDistanceMetersT.text = $"Travelled meters: {rounded}m";

        Destroy(firework);

        var coinsGained = overallDistanceMeters * CoinsPerMeter;

        Debug.Log($"Distance travelled: {overallDistanceMeters}m");
        Debug.Log($"Gained: {coinsGained} coins");

        Coins += coinsGained;
        CoinsT.text = $"Coins: {Coins}";

        yield return new WaitForSeconds(RechargeInterval);
    }
    */
}
