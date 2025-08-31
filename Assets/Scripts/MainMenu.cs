using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame() {
        SceneManager.LoadSceneAsync(1);
    }

    public void EndGame() { 
        Application.Quit();
    }
}
