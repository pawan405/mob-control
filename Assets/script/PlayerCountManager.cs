
using UnityEngine;
using TMPro;

public class PlayerCountManager : MonoBehaviour
{
    public static PlayerCountManager Instance;

    public TMP_Text countText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerCount(int count)
    {
        if (countText != null)
        {
            countText.text = count.ToString("00");
        }
    }
}
