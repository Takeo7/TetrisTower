using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    #region Singleton

    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [Header("UICOntroller")]
    [SerializeField]
    UIController uic;

    [Header("PlayerStats")]
    [SerializeField]
    byte health = 5;

    [Header("Camera")]
    [SerializeField]
    Transform camera_main;
    [SerializeField]
    [Range(0.01f, 3f)]
    float moveCamera = 0.1f;

    [Header("Blocks")]
    [SerializeField]
    GameObject[] blockPrefabs;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Block currentBlock;

    [Header("Input Stats")]
    [SerializeField]
    Vector3 clickPosition;
    [SerializeField]
    float inputDistX;
    [SerializeField]
    float inputDistY;
    [SerializeField]
    bool moveRight = true;
    [SerializeField]
    bool moveLeft = true;

    #region Delegates

    public delegate void GameOverDelegate();
    public GameOverDelegate gameOver_Delegate;

    public delegate void WinDelegate();
    public GameOverDelegate win_Delegate;

    #endregion

    private void Start()
    {
        uic.SetHearts(health);
        gameOver_Delegate += PauseTime;
        win_Delegate += PauseTime;
        SpawnNewBlock();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float distX = clickPosition.x - Input.mousePosition.x;
            

            if (distX <= inputDistX * -1 && moveRight)
            {
                currentBlock.Move(Block.Direction.Right);
                clickPosition = Input.mousePosition;
            }
            else if (distX >= inputDistX && moveLeft)
            {
                currentBlock.Move(Block.Direction.Left);
                clickPosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            float distY = clickPosition.y - Input.mousePosition.y;

            if (clickPosition.Equals(Input.mousePosition))
            {
                currentBlock.Rotate();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
        }

    }

    public void SetMoveRight(bool r)
    {
        moveRight = r;
    }

    public void SetMoveLeft(bool l)
    {
        moveLeft = l;
    }

    public void CurrentBlockRelesaed()
    {
        SpawnNewBlock();
    }


    void SpawnNewBlock()
    {
        int rand = Random.Range(0, blockPrefabs.Length);
        if (rand.Equals(blockPrefabs.Length))
        {
            rand = 0;
        }
        currentBlock = Instantiate(blockPrefabs[rand], spawnPoint.position, Quaternion.identity).GetComponent<Block>();
        currentBlock.SetController(instance);

        camera_main.Translate(Vector3.up * moveCamera);
    }

    public void BlockLost()
    {
        health--;
        uic.SetHearts(health);
        camera_main.Translate(Vector3.down * moveCamera);
        if (health <= 0)
        {
            gameOver_Delegate();
        }
    }

    #region Scene Management

    public void ResetScene()
    {
        ResumeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        ResumeTime();
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Time Management

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1;
    }

    #endregion
}
