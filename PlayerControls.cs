using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    public float score;
    public GameObject gm;
    public AudioClip impact;
    private AudioSource audioSource;

    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    private EventTrigger upTrigger = null;
    private EventTrigger downTrigger = null;
    private EventTrigger leftTrigger = null;
    private EventTrigger rightTrigger = null;

    public Text scoreText;
    public Text timeText;

    private const float acc = 10.0f;
    private const float maxVel = 8.0f;

    private float timeElapsed;

    enum ButtonStatus
    {
        UP,
        DOWN
    };

    ButtonStatus leftStatus = ButtonStatus.UP;
    ButtonStatus rightStatus = ButtonStatus.UP;
    ButtonStatus upStatus = ButtonStatus.UP;
    ButtonStatus downStatus = ButtonStatus.UP;

    // Start is called before the first frame update
    void Start()
    {
        score = 1;
        timeElapsed = 0.0f;
        scoreText.text = "100";
        timeText.text = "0";

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        upTrigger = upButton.gameObject.AddComponent<EventTrigger>();
        downTrigger = downButton.gameObject.AddComponent<EventTrigger>();
        leftTrigger = leftButton.gameObject.AddComponent<EventTrigger>();
        rightTrigger = rightButton.gameObject.AddComponent<EventTrigger>();

        RegisterHandler(EventTriggerType.PointerDown, OnUpDown, upTrigger);
        RegisterHandler(EventTriggerType.PointerDown, OnDownDown, downTrigger);
        RegisterHandler(EventTriggerType.PointerDown, OnLeftDown, leftTrigger);
        RegisterHandler(EventTriggerType.PointerDown, OnRightDown, rightTrigger);

        RegisterHandler(EventTriggerType.PointerUp, OnUpUp, upTrigger);
        RegisterHandler(EventTriggerType.PointerUp, OnDownUp, downTrigger);
        RegisterHandler(EventTriggerType.PointerUp, OnLeftUp, leftTrigger);
        RegisterHandler(EventTriggerType.PointerUp, OnRightUp, rightTrigger);
    }

    private void RegisterHandler(EventTriggerType triggerType, UnityAction<BaseEventData> action, EventTrigger et)
    {
        // Create a new TriggerEvent and add a listener
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        // capture and pass the event data to the listener
        trigger.AddListener((eventData) => action(eventData));

        // Create and initialise EventTrigger.Entry using the created TriggerEvent
        EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

        // Add the EventTrigger.Entry to delegates list on the EventTrigger
        et.triggers.Add(entry);
    }

    void OnUpDown(BaseEventData data)
    {
        upStatus = ButtonStatus.DOWN;
    }
    void OnUpUp(BaseEventData data)
    {
        upStatus = ButtonStatus.UP;
    }


    void OnDownDown(BaseEventData data)
    {
        downStatus = ButtonStatus.DOWN;
    }
    void OnDownUp(BaseEventData data)
    {
        downStatus = ButtonStatus.UP;
    }


    void OnLeftDown(BaseEventData data)
    {
        leftStatus = ButtonStatus.DOWN;
    }
    void OnLeftUp(BaseEventData data)
    {
        leftStatus = ButtonStatus.UP;
    }


    void OnRightDown(BaseEventData data)
    {
        rightStatus = ButtonStatus.DOWN;
    }
    void OnRightUp(BaseEventData data)
    {
        rightStatus = ButtonStatus.UP;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        timeText.text = ((int)timeElapsed).ToString();

        Vector2 newPos = new Vector2(rb.position.x, rb.position.y);
        Vector2 newVel = new Vector2(rb.velocity.x, rb.velocity.y);

        float xacc = 0;
        float yacc = 0;

        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        //get key input //TODO use gyroscope / accelerometer
        if (Input.GetKey(KeyCode.LeftArrow) || leftStatus == ButtonStatus.DOWN)
        {
            left = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || rightStatus == ButtonStatus.DOWN)
        {
            right = true;
        }

        if (Input.GetKey(KeyCode.UpArrow) || upStatus == ButtonStatus.DOWN)
        {
            up = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || downStatus == ButtonStatus.DOWN)
        {
            down = true;
        }

        if (left)
        {
            xacc = -acc;
        }
        else if (right)
        {
            xacc = acc;
        }

        if (up)
        {
            yacc = acc;
        }
        else if (down)
        {
            yacc = -acc;
        }

        newVel.x += xacc * Time.deltaTime;
        newVel.y += yacc * Time.deltaTime;

        if (newVel.x > maxVel)
        {
            newVel.x = maxVel;
        } else if (newVel.x < -maxVel)
        {
            newVel.x = -maxVel;
        }

        if (newVel.y > maxVel)
        {
            newVel.y = maxVel;
        } else if (newVel.y < -maxVel)
        {
            newVel.y = -maxVel;
        }

        if(newPos.x < GameControl.Xmin)
        {
            newPos.x = GameControl.Xmax - 0.5f;
        } else if (newPos.x > GameControl.Xmax)
        {
            newPos.x = GameControl.Xmin+0.5f;
        }

        if (newPos.y < GameControl.Ymin)
        {
            newPos.y = GameControl.Ymin;
            newVel.y = 0;
        }
        else if (newPos.y > GameControl.Ymax)
        {
            newPos.y = GameControl.Ymax;
            newVel.y = 0;
        }

        //update position
        rb.position = newPos;

        //update velocity
        rb.velocity = newVel;

        //update facing left / right
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-score, score);
        }
        else
        {
            transform.localScale = new Vector3(score, score);
        }


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        EnemyControl ec = col.gameObject.GetComponent<EnemyControl>();
        if (ec.score > score)
        {
            //game over
            GameControl.GameOver(false);
        }
        else
        {
            score += Mathf.Sqrt(ec.score*100)/100;
            scoreText.text = ((int)(score*100)).ToString();
            ec.Kill();
            audioSource.PlayOneShot(impact, 0.7F);
            if (score >= 3.0f)
            {
                GameControl.GameOver(true);
            }
        }
    }
}
