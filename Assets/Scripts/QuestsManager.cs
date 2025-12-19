using UnityEngine;

public class QuestsManager : MonoBehaviour
{
    public static QuestsManager single;
    public int _currentQuestPackIndex;
    public QuestsPack[] _questsPacks;
    public QuestsPack _currentQuestsPack;

    private void Awake()
    {
        single = this;
    }

    private void Start()
    {
        _currentQuestsPack = _questsPacks[_currentQuestPackIndex];
        _currentQuestsPack.SetCurrentQuest();
    }
}

[System.Serializable]
public class QuestsPack
{
    public int currentQuestIndex;
    public Quest[] quest;
    public Quest currentQuest;
    public GameObject arenaPrefab; //в будущем мб надо будет

    public void SetCurrentQuest()
    {
        currentQuest = quest[currentQuestIndex];
    }

    public void NextCurrentQuest()
    {
        currentQuest.isComplete = true;
        currentQuestIndex++;
        currentQuest = quest[currentQuestIndex];
        SetCurrentQuest();
    }

    public void ChangeValueCurrentQuest(float add)
    {
        if (currentQuest.questType == Quest.QuestType.EnemyCount)
        {
            currentQuest.value += add;
            CheckQuest();
        }

        InterfaceSys.single.UpdateQuests();
    }

    public void CheckQuest()
    {
        if (currentQuest.questType == Quest.QuestType.EnemyCount)
        {
            if (currentQuest.value >= currentQuest.needValue)
            {
                NextCurrentQuest();
            }
        }

        if (currentQuest.questType == Quest.QuestType.Boss)
        {
            if (!currentQuest.isComplete)
            {
                InterfaceSys.single.ShowBossBattleButton(true);
            }
            else
            {
                GameManager.single.ChangeGameStatus(GameManager.GameStatus.EndGame);
            }
        }
    }
}

[System.Serializable]
public class Quest
{
    public string name;
    public float needValue;
    public float value;
    public bool isComplete;
    public QuestType questType;
    public enum QuestType
    {
        Boss,
        EnemyCount
    }
}