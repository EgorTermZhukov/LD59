using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class FireworkState
{
    public float Lifetime;
    public float Speed;

    // maybe some modifiers as well?
}

public class Main : MonoBehaviour
{

    public ParticleSystem EndscreenParticles;
    private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);
    public int PixelsPerUnit = 16;
    public int PixelsPerMeter = 64;
    public float MetersPerUnit;


    public float CoinsPerMeter;

    public string PersonsName;
    public bool ShowTextInput;
    public float Coins = 0;



    public LineRenderer NotchLinePfb;
    public FireworkLauncher LauncherPfb;
    public FireworkBox FireworkBoxPfb;
    public Guy GuyPfb;


    public float MaxDistance;


    public Transform RulerStartAnchor;

    public Transform LauncherAnchor;
    public Transform GuyAnchor;


    public List<GameObject> RulerNotches;

    public Guy Guy;
    public List<FireworkBox> Boxes;
    public List<Transform> BoxesPositions;

    public bool FirstFireworkExploded = false;
    public bool GameWon = false;

    void Awake()
    {
        G.main = this;
        CMS.Load();
        Coins = BaseValues.coins;
        MaxDistance = 0f;
        CoinsPerMeter = BaseValues.coinsPerMeter;
    }
    IEnumerator Start()
    {
        G.ui.HideHUD();
        MetersPerUnit = (float)PixelsPerUnit / PixelsPerMeter;
        Coins = BaseValues.coins;
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

        Coins = BaseValues.coins;
        G.ui.UpdateCoins(Coins);

        G.ui.ShowFade();
        if (ShowTextInput)
        {
            G.ui.ShowInputBox();
            yield return G.ui.AskForPersonName();
        }


        StartCoroutine(CreateBoxesAndCreateWindows());
        StartCoroutine(CreateGuyAndCreateWindow());

        AudioController.Instance.SetLoopAndPlay("Wind");

        G.ui.FadeOut();
        G.ui.HideInputBox();

        G.ui.ShowHUD();
        EndscreenParticles.Play();
        yield break;
        //StartCoroutine(CreateLauncher());
    }
    public void SetPersonsName(string name)
    {
        PersonsName = name;
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
    public void EvaluateCoinsPerDistanceAndSpawnPopup(float overallDistanceMeters, FireworkBox fireworkBox, Vector3 fireworkPosition)
    {
        // this should be evaluated in fireworkbox
        // oh wait no, nvm, coins per meter is a char upgrade
        // calculate distance there instead of firework

        var distanceRounded = (float)Math.Round(overallDistanceMeters);
        if(distanceRounded > MaxDistance)
        {
            MaxDistance = overallDistanceMeters;
            G.ui.UpdateMaxDistance(MaxDistance, fireworkPosition);
        }

        float coinsGained = distanceRounded * CoinsPerMeter;
        Debug.Log($"Distance travelled: {distanceRounded}m");
        Debug.Log($"Gained: {coinsGained} coins");

        G.ui.SpawnCoinPopup($"+ {G.ui.FormatString(coinsGained)}", fireworkPosition);

        ReceiveCoins(coinsGained);


        if (!FirstFireworkExploded)
        {
            FirstFireworkExploded = true;
            AudioController.Instance.SetLoopAndPlay("Piano", 1);
        }
    }
    public void ReceiveCoins(float coinsToReceive)
    {
        Coins += coinsToReceive;
        Coins = (float)Math.Round(Coins, 1);
        G.ui.UpdateCoins(Coins);
        if(Coins >= BaseValues.winCondition && !GameWon)
        {
            GameWon = true;
            G.ui.HideHUD();
            StartCoroutine(PlayWinCondition());
        }
    }
    public void SpendCoins(float coinsToSpend)
    {
        Coins -= coinsToSpend;
        G.ui.UpdateCoins(Coins);
    }
    public void UnlockFireworkBox(FireworkBox box)
    {
        box.StationWindow.Unlock();
    }
    public IEnumerator CreateGuyAndCreateWindow()
    {
        var guy = Instantiate(GuyPfb, GuyAnchor);
        var window = G.ui.CreateStationWindowAndHide(guy, GuyAnchor.position);
        guy.Init(window);
        guy.Lock();
        Guy = guy;
        Guy.Unlock();

        yield break;
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
            box.Unlock();
            //box.AddLauncherAndStart();
        }
        yield break;
    }
    public IEnumerator PlayWinCondition()
    {
        AudioController.Instance.SetLoopAndPlay("WetFroggo", 1);
        yield return G.ui.DisplayEndscreen();
        yield break;
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
