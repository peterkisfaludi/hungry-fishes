using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static float maxScore = 100.0f;
    public static float Xmin = -7;
    public static float Xmax = -Xmin;
    public static float Ymin = -5;
    public static float Ymax = 4.39f;

    public GameObject enemyFish;

    // Start is called before the first frame update
    void Start()
    {
        //place to random location
        //TODO add time delay
        for (int i = 0; i < 7; ++i)
        {
            MakeEnemy();
        }
    }

    public void MakeEnemy()
    {
        bool leftOrRight = Random.value < 0.5f;
        Vector3 pos = new Vector3(leftOrRight ? Xmin : Xmax, Random.Range(Ymin, Ymax));
        GameObject newFish = Instantiate(enemyFish, pos, Quaternion.identity);
        newFish.layer = 0;
        float vel = Random.Range(0.3f, 4.0f);
        newFish.GetComponent<EnemyControl>().xvel = leftOrRight ? vel : -vel;
        newFish.GetComponent<EnemyControl>().score = Random.Range(0.3f, 3.0f);
        newFish.GetComponent<EnemyControl>().spriteIndex = Random.Range(0, newFish.GetComponent<EnemyControl>().sprites.Length);
    }

    public static void GameOver(bool won)
    {
        if (won)
        {
            MenuControl.status = 2;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            MenuControl.status = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
