using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    public void LoadGameScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}