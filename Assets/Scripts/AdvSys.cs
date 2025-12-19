using TMPro;
using UnityEngine;
using YG;

public class AdvSys : MonoBehaviour
{
    public static AdvSys single;

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
        YG2.onOpenAnyAdv += OpenAdv;
    }

    public void InterAdv()
    {
        YG2.InterstitialAdvShow();
    }

    public void OpenAdv()
    {
        
    }
}
