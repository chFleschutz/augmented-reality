using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FloorChecker : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;

    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        arPlaneManager.trackablesChanged.AddListener(CheckFloor);
    }

    public void CheckFloor(ARTrackablesChangedEventArgs<ARPlane> eventArgs)
    {
        if (Physics.Raycast(transform.position, Vector3.down))
        {
            GetComponent<Rigidbody>().isKinematic = false;
            arPlaneManager.trackablesChanged.RemoveListener(CheckFloor);
        }
    }
}
