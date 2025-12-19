using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
public class SaveSys : MonoBehaviour
{
    public static SaveSys single;
    public Coroutine _saveProcess;

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

    private void Start()
    {
        SetData();
        LanguageManager.single.SetLanguage(YG2.lang);

        //if (_saveProcess == null)
        //{
        //    _saveProcess = StartCoroutine(SaveProcess());
        //}
    }

    public void SetData()
    {
        GameManager.single._angelicEssence = YG2.saves.angelicEssence;

        StartUpgrade[] up = UpgradesManager.single._startUpgrades;

        if (YG2.saves.countsOfStartedUpgrades != null)
        {
            for (int i = 0; i < YG2.saves.countsOfStartedUpgrades.Length; i++)
            {
                up[i].count = YG2.saves.countsOfStartedUpgrades[i];
            }
        }
    }

    public void ResetSaves()
    {
        YG2.SetDefaultSaves();
        GameManager.single._angelicEssence = 0;

        for (int i = 0; i < UpgradesManager.single._startUpgrades.Length; i++)
        {
            UpgradesManager.single._startUpgrades[i].count = 0;
        }

        YG2.SaveProgress();

        SceneManager.LoadScene(0);
    }

    public void ResetCharacter()
    {
        ConstData();

        YG2.saves.characterLevel = 0;
        YG2.saves.enemiesLevel = 0;
        YG2.saves.enemiesSpawnerTimer = 0;
        YG2.saves.enemiesSpawnerDelay = 0;
        YG2.saves.gameTimer = 0;
        YG2.saves.levelPoints = 1;

        YG2.saves.characteristics = new SavedCharacteristics[PlayerAttributes.single._characteristics.Length];

        YG2.saves.attributesBonuses = new float[0];

        YG2.SaveProgress();
    }

    public void ResetAttributesBonuses()
    {
        //for (int i = 0; i < YG2.saves.attributesBonuses.Length; i++)
        //{
        //    YG2.saves.attributesBonuses[i] = 0f;
        //}
    }

    public void ConstData()
    {
        StartUpgrade[] up = UpgradesManager.single._startUpgrades;
        YG2.saves.countsOfStartedUpgrades = new int[up.Length];

        for (int i = 0; i < YG2.saves.countsOfStartedUpgrades.Length; i++)
        {
            YG2.saves.countsOfStartedUpgrades[i] = up[i].count;
        }

        YG2.saves.angelicEssence = GameManager.single._angelicEssence;
    }

    public void Save()
    {
        ConstData();

        if (PlayerAttributes.single != null)
        {
            YG2.saves.characterLevel = PlayerAttributes.single._currentLevel;

            YG2.saves.characteristics = new SavedCharacteristics[PlayerAttributes.single._characteristics.Length];

            for (int i = 0; i < YG2.saves.characteristics.Length; i++)
            {
                SavedCharacteristics sc = new();

                sc.currentLevel = PlayerAttributes.single._characteristics[i].currentLevel;
                sc.needLevel = PlayerAttributes.single._characteristics[i].needLevel;

                YG2.saves.characteristics[i] = sc;
            }

            YG2.saves.attributesBonuses = new float[PlayerAttributes.single._characterAttributes.Length];

            for (int i = 0; i < YG2.saves.attributesBonuses.Length; i++)
            {
                YG2.saves.attributesBonuses[i] = PlayerAttributes.single._characterAttributes[i].bonusValue;
                Debug.Log(PlayerAttributes.single._characterAttributes[i].type + ": " + YG2.saves.attributesBonuses[i]);
            }

            YG2.saves.levelPoints = PlayerAttributes.single._levelPoints;
        }

        if (InterfaceSys.single != null)
        {
            YG2.saves.gameTimer = InterfaceSys.single._gameTimer;
            InterfaceSys.single.ShowSaveText();
        }

        YG2.SaveProgress();
    }

    public IEnumerator SaveProcess()
    {
        float timer = 0f;
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

            if (GameManager.single._gameStatus == GameManager.GameStatus.Playing || GameManager.single._gameStatus == GameManager.GameStatus.BossBattle)
            {
                timer += Time.deltaTime / mul;

                if (timer >= 20f)
                {
                    Save();
                    timer = 0;
                }
            }

            yield return null;
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int angelicEssence;
        public int[] countsOfStartedUpgrades;
        public int characterLevel;
        public SavedCharacteristics[] characteristics;
        public int enemiesLevel;
        public float enemiesSpawnerTimer;
        public float enemiesSpawnerDelay;
        public float gameTimer;
        public float[] attributesBonuses;
        public float levelPoints = 1;
    }
}

[System.Serializable]
public class SavedCharacteristics
{
    public int currentLevel;
    public int needLevel;
}