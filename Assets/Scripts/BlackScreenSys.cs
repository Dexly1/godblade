using UnityEngine;

public class BlackScreenSys : MonoBehaviour
{
    public static BlackScreenSys single;

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

    public void Show(bool value)
    {
        if (value)
        {
            GetComponent<Animation>().Play("showBlackPanel");
        }
        else
        {
            GetComponent<Animation>().Play("hideBlackPanel");
        }
    }
}
