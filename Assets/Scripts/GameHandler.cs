using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    private static int score;
    private LevelGrid levelGrid;
    [SerializeField] private Snake snake;
    private void Awake() 
    {
        instance = this;
        InitializeStatic();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsGamePaused())
            {
                GameHandler.ResumeGame();
            } else {
                GameHandler.PauseGame();
            }
        }
    }
    private void Start()    {
        Debug.Log("GameHandler Starts");
        levelGrid = new LevelGrid(20,20);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
        /*CMDebug.ButtonUI(Vector2.zero, "Reload Scene", () =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });*/
    }
    private static void InitializeStatic()
    {
        score = 0;
    }
    public static int GetScore() 
    {
        return score;
    }
    public static void AddScore() 
    {
        score += 10;
    }
    public static void SnakeDied()
    {
        GameOverWindow.ShowStatic();
    }
    public static void ResumeGame()
    {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
    }
    public static void PauseGame()
    {
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
    }
    public static bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }
}
