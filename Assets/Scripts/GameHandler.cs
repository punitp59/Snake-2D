using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private LevelGrid levelGrid;
    [SerializeField] private Snake snake;
    private void Start()    {
        Debug.Log("GameHandler Starts");
        levelGrid = new LevelGrid(20,20);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }
}
