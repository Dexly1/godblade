using TMPro;
using UnityEngine;
using YG;

public class StartUpgradesInterface : MonoBehaviour
{
    public static StartUpgradesInterface single;
    public ParticleSystem _stars;
    public GameObject[] _buttonsToUpgrade;
    public TextMeshProUGUI _angelicEssencesText;
    public TextMeshProUGUI _angelicEssencesValue;
    public TextMeshProUGUI[] _upgradeTitle;
    public TextMeshProUGUI[] _countTextTitle;
    public TextMeshProUGUI[] _countTextValue;
    public TextMeshProUGUI[] _pricesTextValue;
    public TextMeshProUGUI _upgradesTitleText;
    public TextMeshProUGUI _rebirthTextButton;

    private void Awake()
    {
        single = this;
    }

    private void Start()
    {
        BlackScreenSys.single.Show(false);
        AudioManager.single.Play("amb", true);
        SetLanguage();
        CheckPrices();
        Time.timeScale = 1f;
    }

    public void SetLanguage()
    {
        UpgradesManager um = UpgradesManager.single;
        _angelicEssencesText.text = LanguageManager.single._currentLanguage.uiElements[4]+":";

        for (int i = 0; i < um._startUpgrades.Length; i++)
        {
            _countTextTitle[i].text = LanguageManager.single._currentLanguage.uiElements[6];
            _upgradeTitle[i].text = LanguageManager.single._currentLanguage.startUpgradesDescription[i];
        }

        _rebirthTextButton.text = LanguageManager.single._currentLanguage.uiElements[21];
        _upgradesTitleText.text = LanguageManager.single._currentLanguage.uiElements[26];
    }

    public void CheckPrices()
    {
        UpgradesManager um = UpgradesManager.single;
        _angelicEssencesValue.text = GameManager.single._angelicEssence.ToString();

        for (int i = 0; i < um._startUpgrades.Length; i++)
        {
            StartUpgrade up = um._startUpgrades[i];
            int price = up.originPrice * (up.count + 1);
            _buttonsToUpgrade[i].SetActive(price <= GameManager.single._angelicEssence && up.count < um._maxLevelOfStartUpgrades);
            _pricesTextValue[i].text = price.ToString() + " " + LanguageManager.single._currentLanguage.uiElements[5];
            _countTextValue[i].text = "+" + (up.count * up.attribute[0].value).ToString();
        }
    }

    public void EndOfRebirth()
    {
        SaveSys.single.ResetCharacter();
        SaveSys.single.Save();
        AudioManager.single.Stop("amb", true);
        GameManager.single.ChangeGameStatus(GameManager.GameStatus.PreStart);
    }

    public void AddCountUpgradeBonus(int index)
    {
        UpgradesManager.single.AddCountToUpgrade(index);
    }
}
