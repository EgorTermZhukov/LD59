using System;
using System.Collections;
using System.Collections.Generic;
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
    private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);
    public int PixelsPerUnit = 16;
    public int PixelsPerMeter = 64;
    public float MetersPerUnit;


    public float LifeTimeTimer;


    public float ChargeTimer;

    public int CoinsPerMeter = 1;

    public float Coins = 0;



    public FireworkLauncher LauncherPfb;
    public Transform LauncherAnchor;

    
    public float TravelledMeters;

    public TMP_Text CurrentTravelledDistanceMetersT;
    public TMP_Text CoinsT;

    public LineRenderer LineRenderer;
    public Transform RulerStartAnchor;
    public float RulerLength = 5f;

    public GameObject NotchPfb;

    public List<FireworkLauncher> Launchers;
    void Awake()
    {
        G.main = this;
    }
    void Start()
    {
        MetersPerUnit = (float)PixelsPerUnit / PixelsPerMeter;
        Debug.Log($"Meter size: {MetersPerUnit} per unit");
        LineRenderer.SetPositions(new Vector3[]
        {
            RulerStartAnchor.position,
            new Vector3(RulerStartAnchor.position.x, RulerStartAnchor.position.y + RulerLength, RulerStartAnchor.position.z)
        });

        for(int i = 0; i < 20; i++)
        {
            var position = i / MetersPerUnit * Vector3.up +  LauncherPfb.FireworkLaunchAnchor.position;
            Instantiate(NotchPfb, position, Quaternion.identity);
        }

        Coins = 0;
        CoinsT.text = $"$ {Coins}";
        StartCoroutine(CreateLauncher());
    }
    void Update()
    {
    }
    public void EvaluateCoinsPerDistance(float overallDistanceMeters)
    {

        float coinsGained = overallDistanceMeters * CoinsPerMeter;

        Debug.Log($"Distance travelled: {overallDistanceMeters}m");
        Debug.Log($"Gained: {coinsGained} coins");

        Coins += (float) Math.Round((double)coinsGained, 1);
        CoinsT.text = $"$ {Coins}";
    }
    public IEnumerator CreateLauncher()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 4; i++)
        {
            var launcher = Instantiate(LauncherPfb, LauncherAnchor.position + Vector3.left * (i * 20 / PixelsPerUnit), Quaternion.identity);
            launcher.Init();
            Launchers.Add(launcher);
            yield return _waitForSeconds1;
        }
    }

    // should remake all of this into an update function
    // the stand should also be refreshed while the firework launches i think
    // tweens don't work like that...

}
