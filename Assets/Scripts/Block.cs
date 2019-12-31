using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField]
    GameController gc;
    [SerializeField]
    IA_Controller iac;
    [SerializeField]
    bool isIA = false;

    [Header("Stats")]
    [SerializeField]
    float moveDistance = 1;
    [SerializeField]
    bool released = false;
    [SerializeField]
    [Range(0.01f,3f)]
    float fallingVel = 1f;
    [SerializeField]
    [Range(0.01f, 3f)]
    float fallingRefresh = 0.1f;
    [SerializeField]
    bool isAcc = false;
    

    [Header("Components")]
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    Transform block;
    [SerializeField]
    SpriteRenderer sr;

    [Header("Column")]
    [SerializeField]
    GameObject column;
    [SerializeField]
    [Range(0.1f, 6f)]
    float sizeColumn = 5f;

    public void SetController(GameController g)
    {
        gc = g;
    }

    public void SetIAController(IA_Controller ia)
    {
        iac = ia;
        isIA = true;
    }

    IEnumerator Start()
    {
        column.transform.localScale = new Vector3(sr.bounds.size.x/sizeColumn, column.transform.localScale.y);
        while (released.Equals(false))
        {
            transform.Translate(Vector3.down * fallingVel);
            yield return new WaitForSeconds(fallingRefresh);
        }
    }

    public void AccelerateFalling()
    {
        if (isAcc == false)
        {
            StartCoroutine("AccelerateFallingCoroutine");
        }      
    }

    IEnumerable AccelerateFallingCoroutine()
    {
        fallingRefresh = fallingRefresh * 2;
        yield return new WaitForSeconds(2f);
        fallingRefresh = fallingRefresh / 2;
        isAcc = false;
    }

    public void Rotate()
    {
        block.Rotate(new Vector3(0, 0, -90f));
        column.transform.localScale = new Vector3(sr.bounds.size.x/sizeColumn, column.transform.localScale.y);
    }

    public void Move(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right:
                transform.Translate(Vector3.right * moveDistance);
                break;
            case Direction.Left:
                transform.Translate(Vector3.left * moveDistance);
                break;
            default:
                break;
        }
    }

    public void Release()
    {
        column.SetActive(false);
        released = true;
        rb.gravityScale = 1;
        rb.velocity = Vector3.zero;
        if (isIA.Equals(false))
        {
            gc.CurrentBlockRelesaed();
        }
        else
        {
            iac.CurrentBlockRelesaed();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (released == false)
        {
            Release();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (released == true)
        {
            if (collision.CompareTag("KillBlock"))
            {
                if (isIA.Equals(false))
                {
                    gc.BlockLost();
                }
                else
                {
                    iac.BlockLost();
                }
                
                Destroy(gameObject, 1f);
            }
            else if (collision.CompareTag("Finish"))
            {
                Debug.Log("Win!!!");

            }
        }
        else
        {
            if (collision.CompareTag("KillBlock"))
            {
                if (transform.position.x > 0)
                {
                    gc.SetMoveRight(false);
                }
                else if (transform.position.x < 0)
                {
                    gc.SetMoveLeft(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (released == false)
        {
            if (collision.CompareTag("KillBlock"))
            {
                if (transform.position.x > 0)
                {
                    gc.SetMoveRight(true);
                }
                else if (transform.position.x < 0)
                {
                    gc.SetMoveLeft(true);
                }
            }
        }
    }

    public enum Direction
    {
        Right,
        Left
    }
}
