using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level;
    private int lives;
    private int score;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        NewGame();
    }


    // Lors de chaque nouvelle partie
    private void NewGame()
    {
        lives = 3;
        score = 0;
    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;
        
        if (camera != null) {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete()
    {
        score += 1000;

        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        } else {
            LoadLevel(2);
        }
    }

    // Lorsqu'on echoue dans le level
    public void LevelFailed()
    {
        lives--;

        if (lives <= 0) {
            NewGame();
        } else {
           LoadLevel(level);
        }
    }

}
