using System;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;


public class FireworkState
{
    public float Lifetime;
    public float Speed;

    // maybe some modifiers as well?
}

public class Main : MonoBehaviour
{

    public int PixelsPerUnit = 16;
    public int PixelsPerMeter = 32;
    public float MetersPerUnit;

    public float MinLifetimeSeconds = 0.3f;
    public float UpperLifetimeVariationSeconds = 0.2f;
    public float LifeTimeTimer;

    public float ChargeInterval = 2f;
    public float ChargeTimer;

    public float RechargeInterval = 1f;

    // Am I going to introduce acceleration too?...

    public float FireworkMinSpeed = 2f;
    public float FireworkTopSpeed = 4f;
    public float FireworkSpeed = 0.5f;

    public int CoinsPerMeter = 1;

    public int Coins = 0;


    public GameObject FireworkPfb;
    public GameObject FireworkStandPfb;
    public Transform FireworkLaunchAnchor;
    public Transform FireworkStandAnchor;

    
    public float TravelledMeters;

    public TMP_Text CurrentTravelledDistanceMetersT;
    public TMP_Text CoinsT;

    public LineRenderer LineRenderer;
    public Transform RulerStartAnchor;
    public float RulerLength = 5f;

    public GameObject NotchPfb;

    void Start()
    {
        MetersPerUnit = (float)PixelsPerUnit / PixelsPerMeter;
        Debug.Log($"Meter size: {MetersPerUnit} per unit");
        LineRenderer.SetPositions(new Vector3[]
        {
            RulerStartAnchor.position,
            new Vector3(RulerStartAnchor.position.x, RulerStartAnchor.position.y + RulerLength, RulerStartAnchor.position.z)
        });

        Coins = 0;
        CoinsT.text = $"Coins: {Coins}";
        StartCoroutine(LaunchFirework());
    }
    void Update()
    {
    }

    // should remake all of this into an update function
    // the stand should also be refreshed while the firework launches i think
    // tweens don't work like that...
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
        while(LifeTimeTimer > 0)
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
        // okay I will change this later but for now I am just going to observe
        yield return LaunchFirework();
    }
}
