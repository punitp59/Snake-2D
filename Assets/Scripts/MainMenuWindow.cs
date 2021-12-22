using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{
    private enum Sub
    {
        Main,
        HowToPlay,
    }
    private void Awake()
    {
        transform.Find("MenuButtonSub").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.Find("HowToPlaySub").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.Find("MenuButtonSub").Find("PlayButton").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.GameScene);
        transform.Find("MenuButtonSub").Find("ExitButton").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();
        transform.Find("MenuButtonSub").Find("HowToPlayButton").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.HowToPlay);
        transform.Find("HowToPlaySub").Find("BackButton").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.Main);
        ShowSub(Sub.Main);
    }
    private void ShowSub(Sub sub)
    {
        transform.Find("MenuButtonSub").gameObject.SetActive(false);
        transform.Find("HowToPlaySub").gameObject.SetActive(false);
        switch (sub)
        {
            case Sub.Main:
            transform.Find("MenuButtonSub").gameObject.SetActive(true);
            break;
            case Sub.HowToPlay:
            transform.Find("HowToPlaySub").gameObject.SetActive(true);
            break;
        }
    }
}
