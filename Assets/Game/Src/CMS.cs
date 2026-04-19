using System.Collections.Generic;
using UnityEngine;


public class UpgradeData
{
    public string id;

    public List<float> IncSpeed;
}
public class CMS
{
    public List<UpgradeData> All;
    public void Load()
    {
        All = new List<UpgradeData>()
        {
            new UpgradeData()
            {
                id = "Speed",
                IncSpeed = new List<float>()
                {
                    1f,
                    2f,
                    3f,
                    4f
                }
            }
        };
    }

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
}
