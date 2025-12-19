using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PlayerAttributes : MonoBehaviour
{
    public static PlayerAttributes single;
    public float _power;
    public int _maxLevelOfCharacteristics;
    public bool _isMaximazed;
    public Characteristic[] _characteristics;
    public CharacterAttribute[] _characterAttributes;
    [Header("Level & Experience")]
    public int _currentLevel = 1;
    public Animation _levelUpEffect;
    public float _currentExperience;
    public float _levelPoints;
    public float _originNeedExperience;
    public float _needExperience;
    [Space]
    public HealthSys hs;
    public Coroutine _saveCharacteristics;

    public enum CharacteristicType
    {
        Strength,
        Agility,
        Perception
    }

    public enum AttributeType
    {
        Health,
        Regeneration,
        MulPhysicalDamage,
        MulMagicalDamage,
        CriticalDamageChance,
        CriticalDamageStrength,
        MoveSpeed,
        Mana,
        Expirience,
        BonusExpirience
    }

    private void Awake()
    {
        single = this;
        _needExperience = _originNeedExperience;
    }

    void Start()
    {
        hs = GetComponent<HealthSys>();

        _currentLevel = YG2.saves.characterLevel;
        _needExperience = _originNeedExperience * (_currentLevel + 1);

        _levelPoints = YG2.saves.levelPoints;


        for (int i = 0; i < _characteristics.Length; i++)
        {
            if (YG2.saves.characteristics != null)
            {
                if (YG2.saves.characteristics.Length > 0)
                {
                    _characteristics[i].currentLevel = YG2.saves.characteristics[i].currentLevel;
                }
            }
            else
            {
                _characteristics[i].currentLevel = 0;
            }

            bool haveSave = false;

            if (YG2.saves.characteristics != null)
            { 
                if (YG2.saves.characteristics.Length > 0)
                {
                    if (YG2.saves.characteristics[i].needLevel > 1)
                    {
                        haveSave = true;
                    }
                }
            }

            if (haveSave)
            {
                _characteristics[i].currentLevel = YG2.saves.characteristics[i].currentLevel;
                _characteristics[i].needLevel = YG2.saves.characteristics[i].needLevel;
            }
            else
            {
                _characteristics[i].currentLevel = 0;
            }
        }

        SetStartBonuses();
        InterfaceSys.single.SetWarningLevelPoint(_levelPoints);
        CheckCharacteristicUpgradeAvailable();
    }

    public void TakeDamage(float damage)
    {
        CharacterAttribute health = FindAttribute(AttributeType.Health);
        health.currentValue -= damage;

        if (health.currentValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeExperience(float exp)
    {
        _currentExperience += exp;

        if (!_isMaximazed)
        {
            if (_currentExperience >= _needExperience)
            {
                _currentExperience -= _needExperience;
                LevelUp();
                _needExperience = _originNeedExperience * (_currentLevel + 1);
                CheckCharacteristicUpgradeAvailable();
            }
        }
        else
        {
            _currentExperience = _needExperience = 0;
        }

        InterfaceSys.single.UpdateExperiencePanel(_currentExperience, _needExperience, _currentLevel);
    }

    public void ResetExpAfterDeath()
    {
        _currentExperience = 0f;
        InterfaceSys.single.UpdateExperiencePanel(_currentExperience, _needExperience, _currentLevel);
    }

    public void LevelUp()
    {
        _levelUpEffect.Play("levelUp");
        AudioManager.single.Play("lvlup");

        _currentLevel++;
        _levelPoints++;

        if (_currentLevel >= (_maxLevelOfCharacteristics * _characteristics.Length) - 1)
            _isMaximazed = true;

        GameManager.single.ChangeAngelicEssenceAmount(_currentLevel);
        InterfaceSys.single.UpdateResources();

        SetUpgrade(0);

        InterfaceSys.single.SetWarningLevelPoint(_levelPoints);
        SetBonusesToParameters();

        SaveCharacteristics(false);
        SpawnerSys.single.GetLastTimeAndLevelAfterLevelUp();
        SaveSys.single.Save();
    }

    public void UpgradeCharacteristic(int index)
    {
        SetUpgrade(index);

        _characteristics[index - 1].currentLevel++;
        _characteristics[index - 1].needLevel += Mathf.CeilToInt((_characteristics[index - 1].needLevel + 2) / 5f);
        _levelPoints--;

        CheckCharacteristicUpgradeAvailable();
        InterfaceSys.single.SetWarningLevelPoint(_levelPoints);
        SetBonusesToParameters();
        SaveCharacteristics();
    }

    public void SaveCharacteristics(bool value = true)
    {
        if (_saveCharacteristics != null)
            StopCoroutine(_saveCharacteristics);

        if (value)
            _saveCharacteristics = StartCoroutine(SaveCharacteristicsProcess());
    }

    public IEnumerator SaveCharacteristicsProcess()
    {
        float timer = 0;

        while (timer < 5f)
        {
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }

        SaveSys.single.Save();
        _saveCharacteristics = null;
    }

    public void CheckCharacteristicUpgradeAvailable()
    {
        bool[] values = new bool[_characteristics.Length];

        for (int i = 0; i < _characteristics.Length; i++)
        {
            values[i] = _characteristics[i].currentLevel < _maxLevelOfCharacteristics && _currentLevel + 1 >= _characteristics[i].needLevel && _levelPoints > 0;
        }

        InterfaceSys.single.SetButtonsCharacteristicUpgrade(values);
    }

    public void SetUpgrade(int index)
    {
        Upgrade[] upgrades = UpgradesManager.single._upgrades;
        for (int i = 0; i < upgrades[index].attribute.Length; i++)
        {
            UpgradeBonus(upgrades[index].attribute[i].type, upgrades[index].attribute[i].value);
        }
    }

    public void SetStartBonuses()
    {
        UpgradesManager um = UpgradesManager.single;

        bool haveSave = false;

        if (YG2.saves.attributesBonuses != null)
        {
            if (YG2.saves.attributesBonuses.Length > 0)
            {
                haveSave = true;
            }
        }

        if (haveSave)
        {
            for (int i = 0; i < _characterAttributes.Length; i++)
            {
                _characterAttributes[i].bonusValue = YG2.saves.attributesBonuses[i];
            }
        }

        for (int i = 0; i < um._startUpgrades.Length; i++)
        {
            StartUpgrade up = um._startUpgrades[i];

            if (YG2.saves.countsOfStartedUpgrades != null)
            {
                if (YG2.saves.countsOfStartedUpgrades.Length > 0)
                {
                    up.count = YG2.saves.countsOfStartedUpgrades[i];
                }
            }

            for (int j = 0; j < up.attribute.Length; j++)
            {
                if (up.originType == StartUpgrade.OriginType.Value)
                    FindAttribute(up.attribute[j].type).originValue += up.attribute[j].value * up.count;
                else
                {
                    if (!haveSave)
                    {
                        FindAttribute(up.attribute[j].type).bonusValue += up.attribute[j].value * up.count;
                    }
                }
            }
        }
    }

    public void SetBaseParameters()
    {
        CharacterAttribute hp = FindAttribute(AttributeType.Health);


        SetBonus(hp);

        hs._maxHealth = hp.currentValue;
        hs._currentHealth = hp.currentValue;

        InterfaceSys.single.UpdateExperiencePanel(_currentExperience, _needExperience, _currentLevel);
        InterfaceSys.single.UpdateExperiencePanelStatus(true);
    }

    public void UpgradeBonus(AttributeType type, float value)
    {
        CharacterAttribute attribute = FindAttribute(type);

        if (attribute.valueType == CharacterAttribute.ValueType.Numerical)
        {
            attribute.bonusValue = attribute.bonusValue * (1f + value / 100f);
        }
        else
        {
            attribute.bonusValue = attribute.bonusValue + value;
        }
    }

    public void SetBonusesToParameters()
    {
        CharacterAttribute moveSpeed = FindAttribute(AttributeType.MoveSpeed);
        CharacterAttribute health = FindAttribute(AttributeType.Health);
        CharacterAttribute regen = FindAttribute(AttributeType.Regeneration);
        CharacterAttribute physDamage = FindAttribute(AttributeType.MulPhysicalDamage);
        CharacterAttribute criticalChance = FindAttribute(AttributeType.CriticalDamageChance);
        CharacterAttribute criticalStrength = FindAttribute(AttributeType.CriticalDamageStrength);

        SetBonus(health);

        hs._maxHealth = health.currentValue;

        InterfaceSys.single.UpdateHealthPanel(hs._currentHealth, hs._maxHealth);

        SetBonus(regen);

        hs._regenerationHealth = regen.currentValue;
        hs.RegenerationStatus(true);

        SetBonus(moveSpeed, false);

        GetComponent<PlayerMovement>()._moveSpeed = moveSpeed.currentValue;

        SetBonus(physDamage);
        SetBonus(criticalChance);
        SetBonus(criticalStrength);

        _power = health.currentValue + physDamage.currentValue * 10f + regen.currentValue * 10f + 
                 criticalChance.currentValue * 10f + criticalStrength.currentValue * 10f;

        InterfaceSys.single.UpdateAttributesPanel();
    }

    public void SetBonus(CharacterAttribute ca, bool isRound = true)
    {
        if (ca.valueType == CharacterAttribute.ValueType.Numerical)
            ca.currentValue = Mathf.Round(ca.originValue * ca.bonusValue);
        else
            ca.currentValue = Mathf.Round(ca.originValue + ca.bonusValue);

        if (!isRound)
            ca.currentValue = ca.originValue * ca.bonusValue;
    }

    public CharacterAttribute FindAttribute(AttributeType type)
    {
        for (int i = 0; i < _characterAttributes.Length; i++)
        {
            if (_characterAttributes[i].type == type)
            {
                return _characterAttributes[i];
            }
        }

        return null;
    }
}

[System.Serializable]
public class Characteristic
{
    public string name;
    public PlayerAttributes.CharacteristicType type;
    public int currentLevel;
    public int needLevel;
}

[System.Serializable]
public class CharacterAttribute
{
    public string name;
    public PlayerAttributes.AttributeType type;
    public float originValue;
    //public float maxValue;
    public float currentValue;
    public RoundType roundType;
    public float bonusValue;
    public ValueType valueType;
    public enum ValueType
    {
        Numerical,
        Procent
    }
    
    public enum RoundType
    {
        Round,
        Non
    }
}

[System.Serializable]
public class pAttribute
{
    public PlayerAttributes.AttributeType type;
    public float value;
}


