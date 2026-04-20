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


    public int CoinsPerMeter = 1;

    public string PersonsName;
    public float Coins = 0;



    public LineRenderer NotchLinePfb;
    public FireworkLauncher LauncherPfb;
    public FireworkBox FireworkBoxPfb;


    public float TravelledMeters;


    public Transform RulerStartAnchor;

    public Transform LauncherAnchor;


    public List<GameObject> RulerNotches;

    public List<FireworkBox> Boxes;

    public List<Transform> BoxesPositions;
    void Awake()
    {
        G.main = this;
        CMS.Load();
    }
    void Start()
    {
        MetersPerUnit = (float)PixelsPerUnit / PixelsPerMeter;
        Debug.Log($"Meter size: {MetersPerUnit} per unit");

        RulerNotches = new List<GameObject>();
        for(int i = 1; i < 20; i++)
        {
            var position = (i / MetersPerUnit * Vector3.up) + RulerStartAnchor.position;
            var notch = Instantiate(NotchLinePfb, new Vector3(RulerStartAnchor.position.x, position.y), Quaternion.identity);
            notch.SetPositions(new Vector3[]
            {
                position + Vector3.left * 10f,
                position + Vector3.right * 10f
            });
            Debug.Log(notch.transform.position);
            Debug.Log($"Start anchor positon: {RulerStartAnchor.position}");

            RulerNotches.Add(notch.gameObject);
        }
        HideRuler();

        Coins = 0;
        G.ui.UpdateCoins(Coins);

        StartCoroutine(CreateBoxesAndCreateWindows());


        //StartCoroutine(CreateLauncher());
    }
    void Update()
    {
    }
    public void ShowRuler()
    {
        foreach(var notch in RulerNotches)
        {
            notch.SetActive(true);
        }
    }
    public void HideRuler()
    {
        foreach (var notch in RulerNotches)
        {
            notch.SetActive(false);
        }
    }

    // Reference firework box
    public void EvaluateCoinsPerDistanceAndSpawnPopup(float overallDistanceMeters, FireworkBox fireworkBox, Vector3 popupPosition)
    {
        // this should be evaluated in fireworkbox
        // oh wait no, nvm, coins per meter is a char upgrade

        float coinsGained = overallDistanceMeters * CoinsPerMeter;
        float coinsGainedRound = (float)Math.Round((double)coinsGained, 1); 
        Debug.Log($"Distance travelled: {overallDistanceMeters}m");
        Debug.Log($"Gained: {coinsGained} coins");

        float coinsToReceive = (float)Math.Round((double)coinsGained, 1);
        ReceiveCoins(coinsToReceive, popupPosition);
    }
    public void ReceiveCoins(float coinsToReceive, Vector3 popupPosition)
    {
        Coins += coinsToReceive;
        Coins = (float)Math.Round(Coins, 1);
        G.ui.SpawnPopup($"+ {G.ui.FormatString(coinsToReceive)}", PopupType.Coin, popupPosition);
        G.ui.UpdateCoins(Coins);
    }
    public void UnlockFireworkBox(FireworkBox box)
    {
        box.StationWindow.Unlock();
    }
    public IEnumerator CreateBoxesAndCreateWindows()
    {
        foreach(var boxAnchor in BoxesPositions)
        {
            var box = Instantiate(FireworkBoxPfb, boxAnchor);
            var window = G.ui.CreateStationWindowAndHide(box, boxAnchor.position);
            box.Init(window);
            box.Lock();
            Boxes.Add(box);
            yield return new WaitForSeconds(2f);
            box.Unlock();
            box.AddLauncherAndStart();
        }
    }
    /*
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
    */
}
