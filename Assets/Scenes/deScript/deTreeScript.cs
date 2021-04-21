using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deTreeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Button deButton;
    void Start()
    {
        deButton.onClick.AddListener(ToSence);
    }

    // Update is called once per frame
    void ToSence()
    {
	UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
