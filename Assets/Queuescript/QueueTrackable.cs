using System.Collections;
using UnityEngine;
using Vuforia;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class QueueTrackable : MonoBehaviour, ITrackableEventHandler
{

    protected TrackableBehaviour mTrackableBehaviour;

    protected Transform ARCamera;
    protected Vector3 lostCamPos = new Vector3(0, 4f, 0);
    protected Vector3 lostTar = new Vector3(0, 0, 0);


    public GameObject Button;
    protected bool isFirstFound = false;

 
    protected void Awake() 
    {
        Button.SetActive(false);
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
            Button.SetActive(true);
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
                this.transform.position = lostTar;
                this.transform.rotation = Quaternion.Euler(0,0,0);
                ARCamera.localPosition = lostCamPos;
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
