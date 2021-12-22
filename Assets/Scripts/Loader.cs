using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
         GameScene,
         Loading,
         MainMenu,
    }
    private static Action loaderCallBackAction;
    public static void Load(Scene scene)
    {
        //Set up callback action that will be triggered after the loading scene is loaded.
        loaderCallBackAction = () =>
        {
            //Load target scene when the loading scene is loaded
            SceneManager.LoadScene(scene.ToString());
        };
        // Load Loading Scene
        SceneManager.LoadScene(Scene.Loading.ToString());
    }
    public static void LoaderCallBack()
    {
        if(loaderCallBackAction != null)
        {
            loaderCallBackAction();
            loaderCallBackAction = null;
        }
    }
}
