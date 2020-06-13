using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float xvel;
    public float yvel;
    public float score;
    public GameObject gm;

    public Sprite[] sprites;
    public int spriteIndex;

    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(xvel, yvel);
        GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1*score, score);
        }
        else
        {
            transform.localScale = new Vector3(1*score, score);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.x < GameControl.Xmin)
        {
            Kill();
        } else if (rb.position.x > GameControl.Xmax)
        {
            Kill();
        }

        if (rb.position.y<GameControl.Ymin)
        {
            Kill();
        }
        else if (rb.position.y > GameControl.Ymax)
        {
            Kill();
        }

    }

    public void Kill()
    {
        gm.GetComponent<GameControl>().MakeEnemy(); 
        Destroy(gameObject);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
