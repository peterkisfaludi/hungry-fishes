using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public Button startButton;
    public Button helpButton;
    public Button creditsButton;

    public Text headingText;
    public Text winLoseText;
    public GameObject winning;
    public GameObject losing;

    public static int status=0;

    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        startButton.onClick.AddListener(StartOnClick);
        helpButton.onClick.AddListener(HelpOnClick);
        creditsButton.onClick.AddListener(CreditsOnClick);

        if(status == 1)
        {
            headingText.text = "Game Over";
            winLoseText.text = "You have been eaten by a bigger fish...";
            losing.SetActive(true);
            winning.SetActive(false);
        } else if (status == 2)
        {
            headingText.text = "You Win!";
            winLoseText.text = "You are the biggest fish in the pond";
            losing.SetActive(false);
            winning.SetActive(true);
        } else
        {
            headingText.text = "Main Menu";
            winLoseText.text = "";
            losing.SetActive(false);
            winning.SetActive(false);
        }
    }

    void StartOnClick()
    {
        SceneManager.LoadScene("Game");
    }

    void HelpOnClick()
    {
        SceneManager.LoadScene("Help");
    }

    void CreditsOnClick()
    {
        SceneManager.LoadScene("Credits");
    }

}
