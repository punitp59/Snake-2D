using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    public static GameAsset assetInstance;
    private void Awake()    {
        assetInstance=this;
    }
    public Sprite snakeHeadSprite;
}
