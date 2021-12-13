using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()    {
        Debug.Log("GameHandler Starts");
        GameObject snakeHeadGameObject = new GameObject();
        SpriteRenderer snakeSpriteRenderer = snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAsset.assetInstance.snakeHeadSprite;
    }

    // Update is called once per frame
    void Update()   {
        
    }
}
