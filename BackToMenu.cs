using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    public Button backToMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        backToMenuButton.onClick.AddListener(OnClickBackToMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickBackToMenu()
    {
        MenuControl.status = 0;
        SceneManager.LoadScene("MainMenu");
    }
}
