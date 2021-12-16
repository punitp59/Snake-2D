using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    private static int score;
    private LevelGrid levelGrid;
    [SerializeField] private Snake snake;
    private void Awake() 
    {
        instance = this;
    }
    private void Start()    {
        Debug.Log("GameHandler Starts");
        levelGrid = new LevelGrid(20,20);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }
    public static int GetScore() 
    {
        return score;
    }
    public static void AddScore() 
    {
        score += 10;
    }
}
