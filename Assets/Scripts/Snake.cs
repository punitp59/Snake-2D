using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{
    private enum Direction {
        Left, 
        Right, 
        Up, 
        Down
    }
    private enum State {
        Alive,
        Dead
    }
    private State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    public void Setup(LevelGrid levelGrid) 
    {
        this.levelGrid = levelGrid;
    }
    private void Awake()
    {
        gridPosition = new Vector2Int(0,0);
        gridMoveTimerMax = 0.1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }
    private void Update()
    {
        switch (state) {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
        
    }
    private void HandleInput()
    {
        if(Input.GetKey(KeyCode.UpArrow))   {
            if (gridMoveDirection != Direction.Down)  {
                gridMoveDirection = Direction.Up;
            }
        }
        if(Input.GetKey(KeyCode.DownArrow))   {
            if (gridMoveDirection != Direction.Up)  {
                gridMoveDirection = Direction.Down;
            }
        }
        if(Input.GetKey(KeyCode.LeftArrow))   {
            if (gridMoveDirection != Direction.Right)  {
                gridMoveDirection = Direction.Left;
            }
        }
        if(Input.GetKey(KeyCode.RightArrow))   {
            if (gridMoveDirection != Direction.Left)  {
                gridMoveDirection = Direction.Right;
            }
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0,snakeMovePosition);
            
            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection) {
                default:
                case Direction.Right:   gridMoveDirectionVector = new Vector2Int(+1,0); break;
                case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1,0); break;
                case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0,+1); break;
                case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0,-1); break;
            }
            
            gridPosition += gridMoveDirectionVector;
            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if(snakeAteFood) {
                snakeBodySize ++;
                CreateSnakeBody();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) //ThisLineIssue
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    CMDebug.TextPopup("Dead", transform.position);
                    state = State.Dead;
                }
            }

            transform.position = new Vector3(gridPosition.x,gridPosition.y);
            transform.eulerAngles = new Vector3(0,0, GetAngleFromVector(gridMoveDirectionVector)-90);
        }
    }
    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
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
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }
    private class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;
        private SnakeMovePosition snakeMovePosition;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAsset.assetInstance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }
        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:
                            angle = 0;         //Going up
                            break;
                        case Direction.Left:
                            angle = 0 + 45;         //Angular turn from left
                            break;
                        case Direction.Right:
                            angle = 0 - 45;        //Angular turn from right
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:
                            angle = 180;         //Going down
                            break;
                        case Direction.Left:
                            angle = 180 + 45;         //Angular turn from left
                            break;
                        case Direction.Right:
                            angle = 180 - 45;        //Angular turn from right
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:
                            angle = -90;         //Going left
                            break;
                        case Direction.Down:
                            angle = -45;         //Angular turn from down
                            break;
                        case Direction.Up:
                            angle = 45;            //Angular turn from up
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        default:
                            angle = 90;         //Going right
                            break;
                        case Direction.Down:
                            angle = 45;         //Angular turn from down
                            break;
                        case Direction.Up:
                            angle = -45;        //Angular turn from up
                            break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0,0,angle);
        }
        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }
    private class SnakeMovePosition 
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction) 
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }
        public Direction GetDirection()
        {
            return direction;
        }
        public Direction GetPreviousDirection() 
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }   else    {
                    return previousSnakeMovePosition.direction;
            }
        }
    }
}