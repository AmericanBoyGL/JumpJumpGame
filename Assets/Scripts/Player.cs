using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D col;

    [HideInInspector] public Vector3 pos { get { return transform.position; } }

    public GameObject Zone;

    public bool DragChecker = false; // если true то у нас есть возможность прыжка 

    [HideInInspector] public int JumpCount; // к-ство прыжков в игре на данный момент
    public int CountFroJump; // можем задать к-ство прыжков с инспектора

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        JumpCount = CountFroJump;
    }

    //----------------------------------------------------------------------
    public void PushPlayer (Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
        rb.gravityScale = 1f;
    }

    public void DesactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

    //доделать ф-цию проверки зоны -----------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SaveZone")
        {
            //gameObject.transform.parent = null; // объект будет в корне иерархии инспектора, то есть не иметь родителя
            //gameObject.transform.parent = otherGameObject.transform.parent; // объект будет иметь другого родителя
            //gameObject.transform.parent = other.transform.parent;

            rb.gravityScale = 0; // что бы игрока не выкид. с сейв-зоны убираем гравитацию
            DragChecker = false;
            gameObject.transform.parent = other.transform.parent;
            JumpCount = CountFroJump;
        }

        if (other.gameObject.tag == "Toxic" || other.gameObject.tag == "trap_die")
        {
            GameManager.Instance.toxicPlatMove = false;

            /* LastSceneToPlay переменная запоминает последний уровень который мы играли */
            PlayerPrefs.SetString("LastSceneToPlay", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("UI_after_die");
        }

        if (other.gameObject.tag == "SpawnMap")
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.MapSpawner();
            
        }

        if (other.gameObject.tag == "finishPoint")
        {
            Debug.Log("FINISH");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        gameObject.transform.parent = null;
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
        if (DragChecker == false)
            gameObject.transform.position = other.transform.position;
    }
}
