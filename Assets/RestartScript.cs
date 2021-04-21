using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour
{
    public Button Restart; 
    // Start is called before the first frame update
    void Start()
    {
        Restart.onClick.AddListener(onRestart);
    }

    void onRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
