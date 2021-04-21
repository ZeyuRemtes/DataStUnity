using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;

public class PointControl : MonoBehaviour
{

    public GameObject firstpointer;
    public GameObject rearpointer;
    public Button firstpointbutton;
    public Button rearpointbutton;

    private Vector3 yline = new Vector3(0, 1, 0);
    private int f = 0;
    private int r = 0;
    // Start is called before the first frame update
    void Start()
    {
        firstpointbutton.onClick.AddListener(delegate () {
            FirstpointerGo();
        });
        rearpointbutton.onClick.AddListener(delegate () {
            RearpointerGo();
        });

       
    }

    // Update is called once per frame
    void FirstpointerGo()
    {
        if (r!=(f+1)%12)
        {
            Debug.Log("EnQuene");
            firstpointer.transform.RotateAround(yline, Vector3.down, 30);
            f = (f + 1) % 12;
        }
        else
        {
            #if UNITY_EDITOR
                  UnityEditor.EditorUtility.DisplayDialog("提示", "亲~，这边队列已经满了呢，没法儿入队了", "确认", "取消");
            #endif

        }
    }

    void RearpointerGo()
    {
        if (r!=f)
        {
            Debug.Log("DeQuene");
            rearpointer.transform.RotateAround(yline, Vector3.down, 30);
            r = (r + 1) % 12;
        }
        else
        {

            #if UNITY_EDITOR
                UnityEditor.EditorUtility.DisplayDialog("提示", "亲~，这边队列已经空了呢，没法儿出队了", "确认", "取消");
            #endif

        }
    }
}
