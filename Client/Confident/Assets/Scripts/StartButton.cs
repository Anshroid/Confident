using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void LoadGamesMenu()
    {
        SceneManager.LoadScene(1);
    }
}
