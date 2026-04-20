using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public enum UpgradeType
{
    IncLifetime,
    IncSpeed,
    IncFireworkCount,
    IncAcceleration,
    DecDrag,
    IncWobbleDelay,
    DecWobbleFreqX,
    DecWobbleFreqY,
    IncCoinsPerMeter,
    DecStartupTime,
    DecReloadTime,
    IncBoxesCount,
    UnlockClicking,
    UnlockTrajectory
}

public class UpgradeData
{
    public string id;
    public string name;
    public UpgradeType type;
    public string soundUpgraded;

    // (value, cost)
    public List<(float v, float c)> incLifetime;
    public List<(float v, float c)> incSpeed;
    public List<(int v, float c)> incFireworkCount;
    public List<(float v, float c)> incAcceleration;
    public List<(float v, float c)> decDrag;
    public List<(float v, float c)> incWobbleDelay;
    public List<(float v, float c)> decWobbleFreqX;
    public List<(float v, float c)> decWobbleFreqY;
    public List<(float v, float c)> incCoinsPerMeter;
    public List<(float v, float c)> decStartupTime;
    public List<(float v, float c)> decReloadTime;
    public List<(int v, float c)> incBoxesCount;

    public bool UnlockClicking;
    public bool UnlockTrajectory;

    // thanks claude for the solution, very nice type matching
    public float GetCost(int level)
    {
        return type switch
        {
            UpgradeType.IncLifetime => incLifetime[level].c,
            UpgradeType.IncSpeed => incSpeed[level].c,
            UpgradeType.IncFireworkCount => incFireworkCount[level].c,
            UpgradeType.IncBoxesCount => incBoxesCount[level].c,
            UpgradeType.IncCoinsPerMeter => incCoinsPerMeter[level].c,
            UpgradeType.DecDrag => decDrag[level].c,
            _ => throw new Exception($"Unhandled UpgradeType: {type}")
        };
    }
    public int GetAvaliableLevels()
    {
        return type switch
        {
            UpgradeType.IncLifetime => incLifetime.Count,
            UpgradeType.IncSpeed => incSpeed.Count,
            UpgradeType.IncFireworkCount => incFireworkCount.Count,
            UpgradeType.IncBoxesCount => incBoxesCount.Count,
            UpgradeType.IncCoinsPerMeter => incCoinsPerMeter.Count,
            UpgradeType.DecDrag => decDrag.Count,
            _ => throw new Exception($"Unhandled UpgradeType: {type}")
        };
    }

    // put this into the station
    /*
    public void Apply(int level)
    {
        switch (type)
        {
            case UpgradeType.IncLifetime: BaseValues.lifetime += incLifetime[level].v; break;
            case UpgradeType.IncSpeed: BaseValues.startSpeed += incSpeed[level].v; break;
            case UpgradeType.IncFireworkCount: BaseValues.fireworks += incFireworkCount[level].v; break;
            case UpgradeType.IncBoxesCount: BaseValues.boxesCount += incBoxesCount[level].v; break;
            case UpgradeType.IncCoinsPerMeter: BaseValues.coinsPerMeter += incCoinsPerMeter[level].v; break;
            default: throw new Exception($"Unhandled UpgradeType: {type}");
        }
    }
    */

}

public static class BaseValues
{
    public static string personName = "Tim Murray";
    public static float coins = 0f;
    public static float lifetime = 0.5f;
    public static float startSpeed = 10f;
    public static int maxLaunchers = 4;
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
    public static bool Loaded = false;
    public static List<UpgradeData> All;
    public static List<(string stationId, List<(string tab, List<string> upgrades)> stationData)> StationsUpgrades;

    public static void Load()
    {
        // maybe i should use some kind of curves or ask ai to generate values based on functions
        All = new List<UpgradeData>()
        {
            new()
            {
                id = "lifetime",
                name = "Firework Lifetime",
                type = UpgradeType.IncLifetime,
                incLifetime = new()
                {
                    (0.5f, 10f),
                    (0.5f, 40f)
                }
            },
            new()
            {
                id = "speed",
                name = "Firework Speed",
                type = UpgradeType.IncSpeed,
                incSpeed = new()
                {
                    (5f, 5f),
                    (5f, 100f)
                }
            },
            new()
            {
                id = "fireworks",
                name = "More fireworks",
                type = UpgradeType.IncFireworkCount,
                incFireworkCount = new()
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
                name = "More boxes",
                type = UpgradeType.IncBoxesCount,
                incBoxesCount = new()
                {
                    (1, 0f)
                }
            },
            new()
            {
                id = "coins_per_meter",
                name = "Coins Per Meter",
                type = UpgradeType.IncCoinsPerMeter,
                incCoinsPerMeter = new()
                {
                    (0.5f, 50f),
                    (0.5f, 200f)
                }
            }
        };
        StationsUpgrades = new()
        {
            ("Firework Box", new()
            {
                ("Firework", new()
                {
                    "fireworks",
                    "lifetime",
                    "speed",
                }
                )
            }
            ),
            ("Guy", new()
            {
                ("General", new()
                {
                    "coins_per_meter"
                }
                )
            }
            )
        };
        Loaded = true;
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
