using UnityEngine;
using Vuforia;


public class BinaryTreeTraceScript : MonoBehaviour, ITrackableEventHandler
{

    public GameObject ARCamera;
    public GameObject Canvas;

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    protected virtual void Start()
    {
        Canvas.SetActive(false);
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }


    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Vector3 pos = new Vector3(0, -2.5f, -20f);
            ARCamera.transform.position = pos;

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            Canvas.SetActive(true);
        }
    }
}
