
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using YG;

public class InterfaceSys : MonoBehaviour
{
    public static InterfaceSys single;
    public float _barFillingSpeed;
    public float _barFastFillingSpeed;
    public TextMeshProUGUI[] _closeButtonsTexts;
    [Header("Left Panel")]
    public TextMeshProUGUI _gameTimeValueText;
    public TextMeshProUGUI _allPowerText;
    [Header("Health Panel")]
    public GameObject _healthBar;
    public TextMeshProUGUI _healthValueText;
    [Header("Experience & Level Panel")]
    public GameObject _experienceBar;
    public TextMeshProUGUI _experienceValueText;
    public TextMeshProUGUI _levelValueText;
    public Coroutine _updateExpProcess;
    [Header("Characteristics & Attributes")]
    public TextMeshProUGUI _attributesText;
    public TextMeshProUGUI _attributesValueText;
    public TextMeshProUGUI _characteristicsText;
    public TextMeshProUGUI _characteristicsValueText;
    public GameObject _warningLevelPanel;
    public TextMeshProUGUI _warningLevelPointText;
    public GameObject[] _buttonsCharacteristicUpgrade;
    public float _smallTextSize;
    public string _smallTextColor;
    [Header("Resources Panel")]
    public TextMeshProUGUI _angelicEssencesText;
    [Space]
    [Header("Right Panel")]
    public TextMeshProUGUI _gameSpeedText;
    public float _gameTimer;
    public TextMeshProUGUI _gameVersionText;
    [Header("Path Panel")]
    public GameObject _pathPanel;
    public TextMeshProUGUI _pathText;
    public GameObject _startPointForQuests;
    public TextMeshProUGUI _pathTitleText;
    public TextMeshProUGUI _pathSubtitleText;
    public TextMeshProUGUI[] _currentQuests;
    public GameObject _questPrefab;
    [Header("Rebirth Panel")]
    public GameObject _rebirthPanel;
    public TextMeshProUGUI[] _rebirthTitleText;
    public TextMeshProUGUI _rebirthSubtitleText;
    public TextMeshProUGUI[] _rebirthStartButtonsTexts;
    public TextMeshProUGUI _angelicEssencesTextWhenRebirthPanel;
    [Space]
    [Header("Center Panel")]
    public GameObject _savePanel;
    public TextMeshProUGUI _saveText;
    public GameObject _bossBattlePanel;
    public GameObject _bossBattleButton;
    public bool _bossBattleButtonWasEnabled;
    public TextMeshProUGUI _bossBattleStartText;
    public TextMeshProUGUI _bossBattleStartDescriptionText;
    public TextMeshProUGUI _bossBattleStartButtonText;
    public GameObject _absoluteRebirthPanel;
    public TextMeshProUGUI _absoluteRebirthText;
    public GameObject _endGamePanel;
    public TextMeshProUGUI _endGameText;
    public TextMeshProUGUI _endGameButtonText;
    public TextMeshProUGUI _angelicEssencesTextWhenAbsoluteRebirthPanel;
    public GameObject _enemyHealthPanel;
    public TextMeshProUGUI _enemyNameText;
    public GameObject _enemyHealthBar;
    [Header("3D")]
    public GameObject _dmgTextPrefab;
    public Color[] _dmgTextColor;

    public float _yOffset;
    float hp;
    float max;

    private void Awake()
    {
        single = this;
    }

    IEnumerator Start()
    {
        //Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        Application.targetFrameRate = 60;
        BlackScreenSys.single.Show(false);
        yield return new WaitForEndOfFrame();
        StartCoroutine(UpdateHealthPanelProcess());
        StartCoroutine(ShowEnemyHealthProcess());
        StartCoroutine(GameTimerProcess());
        SetAllCurrentQuests();
        SetLanguage();
    }

    public void ShowPath()
    {
        _pathPanel.SetActive(!_pathPanel.activeSelf);
    }

    public void ShowRebirth()
    {
        _rebirthPanel.SetActive(!_rebirthPanel.activeSelf);
    }

    public void StartRebirth()
    {
#if !UNITY_EDITOR
        AdvSys.single.InterAdv();
        YG2.onCloseAnyAdv += GameManager.single.Rebirth;
#endif

#if UNITY_EDITOR
        GameManager.single.Rebirth();
#endif

    }

    public void SetLanguage()
    {
        _attributesText.text = LanguageManager.single._currentLanguage.uiElements[1];
        _characteristicsText.text = LanguageManager.single._currentLanguage.uiElements[2];
        _gameSpeedText.text = LanguageManager.single._currentLanguage.uiElements[7] + " x" + Mathf.RoundToInt(Time.timeScale).ToString();
        _pathText.text = LanguageManager.single._currentLanguage.uiElements[8];
        _pathTitleText.text = LanguageManager.single._currentLanguage.uiElements[8];

        for (int i = 0; i < _rebirthTitleText.Length; i++)
        {
            _rebirthTitleText[i].text = LanguageManager.single._currentLanguage.uiElements[9];
        }

        _angelicEssencesText.text = GameManager.single._angelicEssence.ToString();
        _angelicEssencesTextWhenRebirthPanel.text = GameManager.single._angelicEssence.ToString();
        _gameVersionText.text = $"{LanguageManager.single._currentLanguage.uiElements[15]} {Application.version}";
        //_allPowerText.text = $"{LanguageManager.single._currentLanguage.uiElements[10]}";
        _bossBattleStartText.text = LanguageManager.single._currentLanguage.uiElements[16];
        _bossBattleStartDescriptionText.text = LanguageManager.single._currentLanguage.uiElements[17];
        _bossBattleStartButtonText.text = LanguageManager.single._currentLanguage.uiElements[18];

        for (int i = 0; i < _closeButtonsTexts.Length; i++)
        {
            _closeButtonsTexts[i].text = LanguageManager.single._currentLanguage.uiElements[19];
        }

        _saveText.text = LanguageManager.single._currentLanguage.uiElements[20];
        _absoluteRebirthText.text = LanguageManager.single._currentLanguage.uiElements[9];

        for (int i = 0; i < _rebirthStartButtonsTexts.Length; i++)
        {
            _rebirthStartButtonsTexts[i].text = LanguageManager.single._currentLanguage.uiElements[21];
        }

        _endGameText.text = LanguageManager.single._currentLanguage.uiElements[22];
        _endGameButtonText.text = LanguageManager.single._currentLanguage.uiElements[23];
        _pathSubtitleText.text = LanguageManager.single._currentLanguage.uiElements[24];
        _rebirthSubtitleText.text = LanguageManager.single._currentLanguage.uiElements[25];
    }

    public void ChangeGameTime()
    {
        _gameTimeValueText.text = TimeSpan.FromSeconds(Mathf.Abs(_gameTimer)).ToString("mm\\:ss");
    }

    public void betaResetSaves()
    {
        SaveSys.single.ResetSaves();
    }

    public IEnumerator GameTimerProcess()
    {
        _gameTimer = YG2.saves.gameTimer;
        float mul;

        while (true)
        {
            if (Time.timeScale > .5f)
            {
                mul = Time.timeScale;
            }
            else
            {
                mul = 1f;
            }

            _gameTimer += Time.deltaTime / mul;
            ChangeGameTime();
            yield return null;
        }
    }

    public void ShowSaveText()
    {
        _savePanel.GetComponent<Animation>().Play("showText");
    }

    public void ShowDamageText(Vector3 pos, float dmg, float offset, HealthSys.DamageType type)
    {
        Vector3 newPos = new Vector3(pos.x + UnityEngine.Random.Range(2f, -2f), offset + UnityEngine.Random.Range(0f, 1.5f), pos.z);
        GameObject dmgText = Instantiate(_dmgTextPrefab, newPos, Quaternion.identity);

        TextMeshPro dmgTextSys = dmgText.transform.GetChild(0).GetComponent<TextMeshPro>();

        dmgTextSys.text = dmg.ToString();
        dmgTextSys.color = _dmgTextColor[(int)type];

        Destroy(dmgText, 1.2f);
    }

    public void ShowBossBattleButton(bool active)
    {
        _bossBattleButton.SetActive(active);

        if (!_bossBattleButtonWasEnabled)
            _bossBattleButtonWasEnabled = true;
    }

    public void ShowBossBattlePanel()
    {
        _bossBattlePanel.SetActive(!_bossBattlePanel.activeSelf);
    }

    public void ShowEndGamePanel()
    {
        StartCoroutine(ShowEndGamePanelProcess());
    }

    public IEnumerator ShowEndGamePanelProcess()
    {
        yield return new WaitForSeconds(1f * Time.timeScale);
        _endGamePanel.SetActive(true);
    }

    public void AfterEndGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(AfterEndGameProcess());
    }

    public IEnumerator AfterEndGameProcess()
    {
        BlackScreenSys.single.Show(true);
        AudioManager.single.Stop("battle_music"+AudioManager.single._currentBattleMusic, true);
        AudioManager.single.Stop("rain", true);
        SaveSys.single.ResetSaves();
        yield return new WaitForSeconds(1f * Time.timeScale);
    }

    public void ShowAbsoluteRebirthPanel()
    {
        StartCoroutine(ShowAbsoluteRebirthPanelProcess());
    }

    public IEnumerator ShowAbsoluteRebirthPanelProcess()
    {
        yield return new WaitForSeconds(1f * Time.timeScale);
        _absoluteRebirthPanel.SetActive(true);
    }

    public void StartBossBattle()
    {
        ShowBossBattlePanel();
        _bossBattleButton.SetActive(false);
        GameManager.single.ChangeGameStatus(GameManager.GameStatus.StartBossBattle);
    }

    public void ChangeGameSpeed()
    {
        GameManager.single.ChangeGameSpeed();
        _gameSpeedText.text = LanguageManager.single._currentLanguage.uiElements[7] + " x" + Mathf.RoundToInt(Time.timeScale).ToString();

        _warningLevelPointText.GetComponent<Animation>()["warning"].speed = 1f / Time.timeScale;
        _bossBattleButton.GetComponent<Animation>()["warning"].speed = 1f / Time.timeScale;
    }

    public void SetWarningLevelPoint(float points)
    {
        _warningLevelPointText.text = LanguageManager.single._currentLanguage.uiElements[3];
        _warningLevelPointText.text = _warningLevelPointText.text.Replace("{value}", points.ToString());
        bool value = points > 0;
        _warningLevelPanel.SetActive(value);
    }

    public void SetButtonsCharacteristicUpgrade(bool[] values)
    {
        for (int i = 0; i < _buttonsCharacteristicUpgrade.Length; i++)
        {
            _buttonsCharacteristicUpgrade[i].SetActive(values[i]);
        }
    }

    public void SetAllCurrentQuests()
    {
        _currentQuests = new TextMeshProUGUI[QuestsManager.single._currentQuestsPack.quest.Length];

        for (int i = 0; i < QuestsManager.single._currentQuestsPack.quest.Length; i++)
        {
            SetQuest(QuestsManager.single._currentQuestsPack.quest[i], i);
        }
    }

    public void SetQuest(Quest quest, int indexOfQuest)
    {
        var questObj = Instantiate(_questPrefab, _startPointForQuests.transform.position, Quaternion.identity, _startPointForQuests.transform.parent);

        questObj.transform.localPosition = new (0f, _startPointForQuests.transform.localPosition.y + _yOffset * indexOfQuest, 0f);

        TextMeshProUGUI questText = questObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        questText.text = LanguageManager.single._currentLanguage.questPacksInfo[QuestsManager.single._currentQuestPackIndex].questDescription[indexOfQuest];
        questText.text = questText.text.Replace("{value}", quest.value.ToString());
        questText.text = questText.text.Replace("{maxValue}", quest.needValue.ToString());

        _currentQuests[indexOfQuest] = questText;
    }

    public void UpdateResources()
    {
        _angelicEssencesText.text = GameManager.single._angelicEssence.ToString();
        _angelicEssencesTextWhenRebirthPanel.text = GameManager.single._angelicEssence.ToString();
        _angelicEssencesTextWhenAbsoluteRebirthPanel.text = GameManager.single._angelicEssence.ToString();
    }

    public void UpdateQuests()
    {
        for (int i = 0; i < QuestsManager.single._currentQuestsPack.quest.Length; i++)
        {
            Quest q = QuestsManager.single._currentQuestsPack.quest[i];
            if (q.questType == Quest.QuestType.EnemyCount)
            {
                _currentQuests[i].text = LanguageManager.single._currentLanguage.questPacksInfo[QuestsManager.single._currentQuestPackIndex].questDescription[i];
                _currentQuests[i].text = _currentQuests[i].text.Replace("{value}", q.value.ToString());
                _currentQuests[i].text = _currentQuests[i].text.Replace("{maxValue}", q.needValue.ToString());
            }
        }
    }

    public void UpdateHealthPanel(float hp, float max)
    {
        _healthValueText.text = $"{Mathf.CeilToInt(hp)}/{Mathf.CeilToInt(max)}";
        this.hp = hp;
        this.max = max;
    }

    public IEnumerator UpdateHealthPanelProcess()
    {
        hp = 1f;
        max = 1f;

        while (true)
        {
            _healthBar.transform.localScale = Vector2.Lerp(_healthBar.transform.localScale, new Vector2(hp / max, 1f), 6f * Time.deltaTime);
            yield return null;
        }
    }

    public void UpdateExperiencePanel(double exp, double max, int level)
    {
        if (!PlayerAttributes.single._isMaximazed)
        {
            _experienceValueText.text = $"{exp}/{max}";
        }
        else
        {
            _experienceValueText.text = LanguageManager.single._currentLanguage.uiElements[11];
        }

        _levelValueText.text = $"{LanguageManager.single._currentLanguage.uiElements[0]} {level + 1}";
    }

    public void UpdateAttributesPanel()
    {
        Characteristic[] chars = PlayerAttributes.single._characteristics;
        CharacterAttribute[] attrs = PlayerAttributes.single._characterAttributes;

        _allPowerText.text = $"{LanguageManager.single._currentLanguage.uiElements[10]} {PlayerAttributes.single._power}";

        _attributesValueText.text = "";

        for (int i = 0; i < attrs.Length; i++)
        {
            if (attrs[i].bonusValue < 0f)
            {
                continue;
            }
            else
            {
                string round = "";

                if (attrs[i].roundType == CharacterAttribute.RoundType.Non)
                {
                    round = "F2";
                }

                if (attrs[i].valueType == CharacterAttribute.ValueType.Procent)
                {
                    _attributesValueText.text += $"<size={_smallTextSize}><color=#{_smallTextColor}>{attrs[i].originValue.ToString(round)}% + {attrs[i].bonusValue.ToString(round)}%</color></size> {attrs[i].currentValue.ToString(round)}%\n";
                }
                else
                {
                    _attributesValueText.text += $"<size={_smallTextSize}><color=#{_smallTextColor}>{attrs[i].originValue.ToString(round)} x{attrs[i].bonusValue:F2}</color></size> {attrs[i].currentValue.ToString(round)}\n";
                }
            }
        }

        _characteristicsValueText.text = "";

        for (int i = 0; i < chars.Length; i++)
        {
            _characteristicsValueText.text += LanguageManager.single._currentLanguage.uiElements[0] + " " + chars[i].currentLevel.ToString() + "\n";
        }
    }

    public void UpdateExperiencePanelStatus(bool value)
    {
        if (value)
        {
            if (_updateExpProcess == null)
            {
                _updateExpProcess = StartCoroutine(UpdateExperiencePanelProcess());
            }
        }
        else
        {
            if (_updateExpProcess != null)
            {
                StopCoroutine(_updateExpProcess);
            }
        }
    }

    public IEnumerator UpdateExperiencePanelProcess()
    {
        float cur = -1;
        PlayerAttributes pattr = PlayerAttributes.single;
        float mul;

        while (!PlayerAttributes.single._isMaximazed)
        {
            if (Time.timeScale > .5f)
            {
                mul = Time.timeScale;
            }
            else
            {
                mul = 1f;
            }

            if (GameManager.single._gameStatus == GameManager.GameStatus.Reincornation)
            {
                cur = -1;
                Vector2 newScale = new Vector2(pattr._currentExperience / pattr._needExperience, 1f);
                _experienceBar.transform.localScale = Vector2.Lerp(_experienceBar.transform.localScale, newScale, _barFillingSpeed / mul * Time.deltaTime);
            }
            else
            {
                if (cur <= pattr._currentExperience)
                {
                    Vector2 newScale = new Vector2(pattr._currentExperience / pattr._needExperience, 1f);
                    _experienceBar.transform.localScale = Vector2.Lerp(_experienceBar.transform.localScale, newScale, _barFillingSpeed / mul * Time.deltaTime);
                    cur = pattr._currentExperience;
                    yield return null;
                }
                else
                {
                    while (_experienceBar.transform.localScale.x < .98f)
                    {
                        _experienceBar.transform.localScale = Vector2.Lerp(_experienceBar.transform.localScale, new Vector2(1f, 1f), _barFastFillingSpeed / mul * Time.deltaTime);
                        yield return null;
                    }

                    cur = -1f;
                    _experienceBar.transform.localScale = new Vector2(0f, 1f);
                }
            }

            yield return null;
        }

        _experienceBar.transform.localScale = new Vector2(1f, 1f);
        //_experienceValueText.gameObject.SetActive(false);
    }

    bool settedTarget = false;

    public void ReshowEnemyHealth()
    {
        settedTarget = false;
        _enemyHealthPanel.SetActive(false);
    }

    public IEnumerator ShowEnemyHealthProcess()
    {
        GameObject target = null;
        float timer = 0;
        HealthSys health = null;

        while (true)
        {
            if (PlayerSys.single._currentAction == PlayerSys.Actions.Attacking)
            {
                if (!settedTarget)
                {
                    yield return new WaitForEndOfFrame();
                        target = PlayerSys.single.cs._currentTarget;
                    if (target != null)
                    {
                        settedTarget = true;
                    Debug.Log(settedTarget);
                        health = target.GetComponent<HealthSys>();
                        EnemySys es = target.GetComponent<EnemySys>();
                        _enemyNameText.text = LanguageManager.single._currentLanguage.questPacksInfo[QuestsManager.single._currentQuestPackIndex].enemyName[es._index];
                        _enemyHealthBar.transform.localScale = new Vector2(health._currentHealth / health._maxHealth, 1f);
                        _enemyHealthPanel.SetActive(true);
                    }
                }
            }
            else
            {
                if (GameManager.single._gameStatus != GameManager.GameStatus.Playing && GameManager.single._gameStatus != GameManager.GameStatus.BossBattle)
                {
                    settedTarget = false;
                    _enemyHealthPanel.SetActive(false);
                }
            }

            if (health != null)
            {
                _enemyHealthBar.transform.localScale = Vector2.Lerp(_enemyHealthBar.transform.localScale, new Vector2(health._currentHealth / health._maxHealth, 1f), 6f * Time.deltaTime);
                
                if (health._isDead)
                {
                    timer += Time.deltaTime;
                }

                if (timer > .3f)
                {
                    settedTarget = false;
                    _enemyHealthPanel.SetActive(false);
                    timer = 0f;
                }
            }

            yield return null;
        }
    }
}
