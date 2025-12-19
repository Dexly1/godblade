using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager single;
    public Language[] _languages;
    public Language _currentLanguage;

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

        _currentLanguage = _languages[0];
    }

    public void SetLanguage(string lang)
    {
        for (int i = 0; i < _languages.Length; i++)
        {
            if (lang == _languages[i].name)
            {
                _currentLanguage = _languages[i];
                break;
            }
        }
    }

    void Start()
    {
    }
}

[System.Serializable]
public class Language
{
    public string name;
    [TextArea(3, 3)]
    public string[] uiElements;
    public QuestPackInfo[] questPacksInfo;
    [TextArea(3, 3)]
    public string[] startUpgradesDescription;
}

[System.Serializable]
public class QuestPackInfo
{
    [TextArea(3, 3)]
    public string[] questDescription;
    public string[] enemyName;
}
