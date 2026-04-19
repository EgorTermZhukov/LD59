using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class UpgradeData
{
    public string id;

    // (value, cost)
    public List<(float v, float c)> IncLifetime;
    public List<(float v, float c)> IncSpeed;
    public List<(int v, float c)> IncFireworkCount;
    public List<(float v, float c)> IncAcceleration;
    public List<(float v, float c)> DecDrag;
    public List<(float v, float c)> IncWobbleDelay;
    public List<(float v, float c)> DecWobbleFreqX;
    public List<(float v, float c)> DecWobbleFreqY;
    public List<(float v, float c)> IncCoinsPerMeter;
    public List<(float v, float c)> DecStartupTime;
    public List<(float v, float c)> DecReloadTime;
    public List<(int v, float c)> IncBoxesCount;

    public bool UnlockClicking;
    public bool UnlockTrajectory;
}

public static class BaseValues
{
    public static string name = "Tim";
    public static float coins = 0f;
    public static float lifetime = 0.5f;
    public static float startSpeed = 10f;
    public static int fireworks = 0;
    public static float acceleration = 0f;
    public static float drag = 0f;
    public static float wobbleDelay = 0f;
    public static float wobbleFreqX = 5f;
    public static float wobbleFreqY = 0.5f;
    public static float coinsPerMeter = 1;
    public static float startupTime = 4f;
    public static float reloadTime = 0.5f;
    public static int boxesCount = 0;

}
public static class CMS
{
    public static List<UpgradeData> All;
    public static List<(string tab, List<string> upgrages)> FireworkBoxUpgrades;
    public static List<(string tab, List<string> upgrades)> GuyUpgrades;
    public static void Load()
    {
        // maybe i should use some kind of curves or ask ai to generate values based on functions
        All = new List<UpgradeData>()
        {
            new()
            {
                id = "lifetime",
                IncLifetime = new()
                {
                    (0.2f, 10f),
                    (0.2f, 40f),
                    (0.2f, 100f),
                    (0.2f, 145f)
                }
            },
            new()
            {
                id = "speed",
                IncSpeed = new()
                {
                    (1f, 5f),
                    (2f, 10f)
                }
            },
            new()
            {
                id = "fireworks",
                IncFireworkCount = new()
                {
                    (1, 0f),
                    (1, 200f),
                    (1, 500f),
                    (1, 900f)
                }
            },
            new()
            {
                id = "boxes",
                IncBoxesCount = new()
                {
                    (1, 0f)
                }
            },
            new()
            {
                id = "coins_per_meter",
                IncCoinsPerMeter = new()
                {
                    (0.5f, 50f),
                    (0.5f, 200f)
                }
            }
        };
        FireworkBoxUpgrades = new()
        {
            ("1", new()
            {
                "lifetime",
                "speed",
                "fireworks"
            })
        };
        GuyUpgrades = new()
        {
            ("1", new()
            {
                "boxes",
                "coins_per_meter"
            })
        };
    }

    /*
    public string GetDescription(string id)
    {
        // yeah not very useful, will make the descriptions inside the corresponding upgrade units like firework boxes
        // i dont want to pass the current parameters or smth like that

        var upgrade = All.Find(x => x.id == id);
        if(upgrade == null)
            Debug.LogError($"Upgrade with id: {id} does not exist");
        string description = "";
        switch (upgrade.id)
        {
            case "Velocity":
                description = "";
                break;
        }
        return description;
    }
    */
}
