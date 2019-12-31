using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Controller : MonoBehaviour
{

    [Header("Controller")]
    [SerializeField]
    GameController gc;

    [Header("IAStats")]
    [SerializeField]
    byte health = 5;

    [Header("Camera")]
    [SerializeField]
    Transform IA_Camera;
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

    private void Start()
    {
        SpawnNewBlock();
        StartCoroutine("IAMoves");
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
        currentBlock.SetIAController(this);

        IA_Camera.Translate(Vector3.up * moveCamera);
    }

    public void BlockLost()
    {
        health--;
        IA_Camera.Translate(Vector3.down * moveCamera);
        if (health <= 0)
        {
            gc.win_Delegate();
        }
    }

    IEnumerator IAMoves()
    {
        while (true)
        {
            byte rand = (byte)Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    currentBlock.Move(Block.Direction.Right);
                    break;
                case 1:
                    currentBlock.Move(Block.Direction.Left);
                    break;
                case 2:
                    currentBlock.Rotate();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1f);
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
