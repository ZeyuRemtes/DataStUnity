using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackControl : MonoBehaviour
{
    public GameObject Canves;
    public GameObject UIshow;
    public GameObject UIBack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseUp()
    {
        Debug.Log("点击事件喂喂喂");
        Canves.SetActive(false);
        UIshow.SetActive(true);
        UIBack.SetActive(false);
    }
}
