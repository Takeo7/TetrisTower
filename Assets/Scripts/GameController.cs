using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("PlayerStats")]
    [SerializeField]
    byte health = 3;

    [Header("Camera")]
    [SerializeField]
    Transform camera;
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

    private void Start()
    {
        SpawnNewBlock();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float distX = clickPosition.x - Input.mousePosition.x;
            

            if (distX <= inputDistX * -1)
            {
                currentBlock.Move(Block.Direction.Right);
                clickPosition = Input.mousePosition;
            }
            else if (distX >= inputDistX)
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

        camera.Translate(Vector3.up * moveCamera);
    }

    public void BlockLost()
    {
        health--;
        if (health <= 0)
        {
            //GameOver
            Debug.Log("GameOver");
        }
    }

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
