using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{

    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();

    //[SerializeField]
    //private int maxPrefabSpawnCount = 0;
    //private int placedPrefabCount;

    [SerializeField]
    private GameObject placeablePrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {

        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {          
            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }

            if(raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;
                SpawnPrefab(hitPose);
            }
        }
    }

    public void SetPrefabType (GameObject prefabType)
    {
        placeablePrefab = prefabType;
    }

    private void SpawnPrefab (Pose hitPose)
    {
        CleanPlacedPrefabs();

        spawnedObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
        placedPrefabList.Add(spawnedObject);
        //placedPrefabCount++;
    }

    private void CleanPlacedPrefabs()
    {
        foreach (GameObject gameObject in placedPrefabList)
        {
            Object.Destroy(gameObject);
        }
        placedPrefabList.Clear();
        //placedPrefabCount = 0;
    }
}
