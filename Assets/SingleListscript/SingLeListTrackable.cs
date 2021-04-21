using System.Collections;
using UnityEngine;
using Vuforia;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class SingLeListTrackable : MonoBehaviour, ITrackableEventHandler
{

    protected TrackableBehaviour mTrackableBehaviour;

    protected Transform ARCamera;
    protected Vector3 lostCamPos = new Vector3(0, 2.5f, 0);

   
    protected bool isFirstFound = false;

    public Button btnadd;
    public Button btndelete;
    protected void Awake() 
    {
        btnadd.gameObject.SetActive(false);
        btndelete.gameObject.SetActive(false);
        ARCamera = GameObject.Find("ARCamera").transform;
    }

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }


    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            isFirstFound = true;
            btnadd.gameObject.SetActive(true);
            btndelete.gameObject.SetActive(true);
            ShowThisModel();
        }
        //else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
        //         newStatus == TrackableBehaviour.Status.NO_POSE)
        //{
        //    Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

        //}
        else
        {
            if (isFirstFound)
            {
                ARCamera.localPosition = lostCamPos;
                ARCamera.localRotation = Quaternion.Euler(90f,90f,90f);
            }
        }
    }

    //显示当前的模型
    public void ShowThisModel()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
