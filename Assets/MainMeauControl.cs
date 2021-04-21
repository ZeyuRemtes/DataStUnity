using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMeauControl : MonoBehaviour
{

    public Button SingleListButton;
    public Button QueueButton;
    public Button BinaryTreeButton;
    public Button GraphButton;
    // Start is called before the first frame update
    void Start()
    {
        SingleListButton.onClick.AddListener(ToSingleListSence);
        QueueButton.onClick.AddListener(ToQueueSence);
        BinaryTreeButton.onClick.AddListener(ToBinaryTreeSence);
        GraphButton.onClick.AddListener(ToGraphSence);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ToSingleListSence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(8);
    }
    void ToQueueSence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }
    void ToBinaryTreeSence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
    void ToGraphSence()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }
}
