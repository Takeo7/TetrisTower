using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuController : MonoBehaviour
{
    

    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadTwoPlayer()
    {
        SceneManager.LoadScene(2);
    }
}
