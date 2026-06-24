using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackScreen;
    private static string GAME_SCENE_NAME = "MainScene";
    void Awake()
    {
        
    }

    public void StartNewGame()
    {
        if (blackScreen != null)
        {
            LeanTween.alphaCanvas(blackScreen, 1.0f, 2f).setOnComplete(() => SceneManager.LoadScene(GAME_SCENE_NAME));
        } else
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);    
        }
        
    }
}
