using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    public void Setup(LevelGrid levelGrid) 
    {
        this.levelGrid = levelGrid;
    }
    private void Awake()
    {
        gridPosition = new Vector2Int(0,0);
        gridMoveTimerMax = 0.1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;
    }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }
    private void HandleInput()
    {
        if(Input.GetKey(KeyCode.UpArrow))   {
            if (gridMoveDirection.y != -1)  {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
        }
        if(Input.GetKey(KeyCode.DownArrow))   {
            if (gridMoveDirection.y != +1)  {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        if(Input.GetKey(KeyCode.LeftArrow))   {
            if (gridMoveDirection.x != +1)  {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        if(Input.GetKey(KeyCode.RightArrow))   {
            if (gridMoveDirection.x != -1)  {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;
            snakeMovePositionList.Insert(0,gridPosition);
            
            gridPosition += gridMoveDirection;

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if(snakeAteFood) {
                snakeBodySize ++;
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one *.5f,Color.white);
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }

            transform.position = new Vector3(gridPosition.x,gridPosition.y);
            transform.eulerAngles = new Vector3(0,0, GetAngleFromVector(gridMoveDirection)-90);
           
        }
    }
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        if(n<0) n += 360;
        return n;
    }
    public Vector2Int GetGridPosition()
    {
       return gridPosition;
    }
    public List<Vector2Int> GetFullSnakePositionList() 
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }
}