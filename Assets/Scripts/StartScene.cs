using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using YG;

public class StartScene : MonoBehaviour
{
    bool _isPressed;
    public GameObject _startGameButton;
    public TextMeshProUGUI _startGameText;
    public TextMeshProUGUI _infoGameText;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
    }

    IEnumerator Start()
    {
        YG2.GameReadyAPI();
        YG2.GameplayStop();
        AdvSys.single.InterAdv();
        BlackScreenSys.single.Show(false);
        AnimationState anim = _startGameText.GetComponent<Animation>()["warning"];
        AudioManager.single.Play("rain", true);
        AudioManager.single.Play("start_music", true);
        anim.speed = 1f / Time.timeScale / 4;
        yield return new WaitForEndOfFrame();
        SetLanguage();
        yield return new WaitForSeconds(1.3f);
        _startGameButton.GetComponent<Button>().interactable = true;
    }

    public void SetLanguage()
    {
        _startGameText.text = LanguageManager.single._currentLanguage.uiElements[12];
        _infoGameText.text = $"{LanguageManager.single._currentLanguage.uiElements[13]}\n";
        _infoGameText.text += $"{LanguageManager.single._currentLanguage.uiElements[14]}\n";
        _infoGameText.text += $"{LanguageManager.single._currentLanguage.uiElements[15]} {Application.version}";
    }

    public void StartGame()
    {
        if (!_isPressed)
        {
            AudioManager.single.Stop("start_music", true);
            AudioManager.single.Stop("rain", true);
            GameManager.single.ChangeGameStatus(GameManager.GameStatus.PreStart);
            _isPressed = true;
        }
    }

    public void Unselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
