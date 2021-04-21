using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFordControl : MonoBehaviour
{
    public GameObject Canves;
    public GameObject UIshow;
    public GameObject UIBack;
    // Start is called before the first frame update
    void Start()
    {
        Canves.SetActive(false);
        UIshow.SetActive(true);
        UIBack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        Debug.Log("点击事件嘿嘿嘿");
        Canves.SetActive(true);
        UIshow.SetActive(false);
        UIBack.SetActive(true);
    }
}
