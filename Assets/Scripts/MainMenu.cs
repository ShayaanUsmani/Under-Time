using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string mainGameSceneName = "MainGame";
    void PlayGame()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }
}
