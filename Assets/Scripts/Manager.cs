
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;


public class Manager: MonoBehaviour
{
    private List<IResourceLocation> remoteRefs;
    public AssetLabelReference _ref;

    public Camera AR_Camera;
    public ARRaycastManager raycastManager;
    public List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public Button Cube, Sphere, Cylinder;

    //Global variables
    [HideInInspector]
    public int prefabSelected = 0;
    private Pose pose;

    void CubeClicked()
    {
        prefabSelected = 0;
    }
    void SphereClicked()
    {
        prefabSelected = 1;
    }
    void CylinderClicked()
    {
        prefabSelected = 2;
    }

    private void Update()
    {
        Cube.onClick.AddListener(CubeClicked);
        Sphere.onClick.AddListener(SphereClicked);
        Cylinder.onClick.AddListener(CylinderClicked);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = AR_Camera.ScreenPointToRay(Input.mousePosition);
            if (raycastManager.Raycast(ray, hits))
            {
                pose = hits[0].pose;
                Spawn_prefab();
            }
        }        
    }

    private void Spawn_prefab()
    {
        Addressables.LoadResourceLocationsAsync(_ref.labelString).Completed += OnLoad;
    }

    private void OnLoad(AsyncOperationHandle<IList<IResourceLocation>> obj)
    {
        remoteRefs = new List<IResourceLocation>(obj.Result);

        StartCoroutine(SpawnRemoteRefs());
    }

    private IEnumerator SpawnRemoteRefs()
    {
        yield return new WaitForSeconds(1f);

        Addressables.InstantiateAsync(remoteRefs[prefabSelected], 
            pose.position, Quaternion.identity);
    }
}