using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private void Awake()
    {
        gridPosition = new Vector2Int(0,0);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(5,0);
        
    }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }
    private void HandleInput()
    {
        if(Input.GetKey(KeyCode.UpArrow))   {
            if (gridMoveDirection.y != -5)  {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +5;
            }
        }
        if(Input.GetKey(KeyCode.DownArrow))   {
            if (gridMoveDirection.y != +5)  {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -5;
            }
        }
        if(Input.GetKey(KeyCode.LeftArrow))   {
            if (gridMoveDirection.x != +5)  {
                gridMoveDirection.x = -5;
                gridMoveDirection.y = 0;
            }
        }
        if(Input.GetKey(KeyCode.RightArrow))   {
            if (gridMoveDirection.x != -5)  {
                gridMoveDirection.x = +5;
                gridMoveDirection.y = 0;
            }
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer >= gridMoveTimerMax) {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
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
}