using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("GameController")]
    [SerializeField]
    GameController gc;

    [Header("Hearts")]
    [SerializeField]
    Text hearts_Text;

    [Header("Canvas")]
    [SerializeField]
    GameObject inGame_Canvas;
    [SerializeField]
    GameObject pause_Canvas;
    [SerializeField]
    GameObject gameOver_Canvas;

    private void Start()
    {
        gc.gameOver_Delegate += GameOverUI;
    }

    public void SetHearts(byte h)
    {
        hearts_Text.text = "x " + h;
    }

    public void GameOverUI()
    {
        inGame_Canvas.SetActive(false);
        gameOver_Canvas.SetActive(true);
    }
}
