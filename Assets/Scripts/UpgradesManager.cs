using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager single;
    public int _maxLevelOfStartUpgrades;
    public Upgrade[] _upgrades;
    public StartUpgrade[] _startUpgrades;

    private void Awake()
    {
        if (single == null)
        {
            single = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCountToUpgrade(int index)
    {
        StartUpgrade up = _startUpgrades[index];
        GameManager.single.ChangeAngelicEssenceAmount(-Mathf.RoundToInt(up.originPrice * (up.count + 1)));
        up.count++;
        StartUpgradesInterface.single.CheckPrices();
    }
}

[System.Serializable]
public class Upgrade
{
    public string name;
    public pAttribute[] attribute;
    public UpgradeValue upgradeValue;
    public enum UpgradeValue
    {
        Numeric,
        Procent
    }
}

[System.Serializable]
public class StartUpgrade
{
    public string name;
    public pAttribute[] attribute;
    public UpgradeValue upgradeValue;
    public OriginType originType;
    public int count;
    public int originPrice;
    public enum UpgradeValue
    {
        Numeric,
        Procent
    }

    public enum OriginType
    {
        Value,
        Bonus
    }
}
