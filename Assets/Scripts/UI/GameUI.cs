using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        Debug.Log("Pause");
    }
}
