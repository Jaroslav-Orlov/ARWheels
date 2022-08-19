using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgramManager : MonoBehaviour
{
    [SerializeField] private GameObject _planeMarkerPrefab;
    [SerializeField] private Camera _aRcamera;

    public GameObject _objectToSpawn;
    public GameObject _scrollViwe;

    public bool _chooseObject = false;

    private GameObject _selectObject;
    private ARRaycastManager aRRaycastManager;
    private Vector2 _touchPosition;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        _planeMarkerPrefab.SetActive(false);
        _scrollViwe.SetActive(false);
    }


    void Update()
    {
        //ShowMarker();
        if (_chooseObject)
        {
            ShowMarkerAndSetObject();
        }
        MoveObject();
    }

    void ShowMarkerAndSetObject()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);


        if (hits.Count > 0)
        {
            _planeMarkerPrefab.transform.position = hits[0].pose.position;
            _planeMarkerPrefab.SetActive(true);
        }

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Instantiate(_objectToSpawn, hits[0].pose.position, _objectToSpawn.transform.rotation);
            _chooseObject = false;
            _planeMarkerPrefab.SetActive(false);
        }
    }
    void MoveObject()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _touchPosition = touch.position;

           if(touch.phase == TouchPhase.Began)
            {
                Ray ray = _aRcamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.CompareTag("UnSelect"))
                    {
                        hitObject.collider.gameObject.tag = "Select";
                    }
                }
            }
           

            if (touch.phase == TouchPhase.Moved)
            {
                aRRaycastManager.Raycast(_touchPosition, hits, TrackableType.Planes);
                _selectObject = GameObject.FindWithTag("Select");
                _selectObject.transform.position = hits[0].pose.position;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                if (_selectObject.CompareTag("Select"))
                {
                    _selectObject.tag = "UnSelect";
                }
            }
        }
    }

}
