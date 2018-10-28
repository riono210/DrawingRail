using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class CostomARCameraMG : MonoBehaviour {

    public Camera m_camera;
    private UnityARSessionNativeInterface m_session;
    private Material savedClearMaterial;

    [Header("AR Config Options")]
    public UnityARAlignment startAlignment = UnityARAlignment.UnityARAlignmentGravity;
    public UnityARPlaneDetection planeDetection = UnityARPlaneDetection.Horizontal;
    public bool getPointCloud = true;
    public bool enableLightEstimation = true;

    public PlaneDetector planeDetector;
    private bool configFlg;

    // Use this for initialization
    void Start() {
        configFlg = true;

        m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

        Application.targetFrameRate = 60;
        ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration();
        config.planeDetection = planeDetection;
        config.alignment = startAlignment;
        config.getPointCloudData = getPointCloud;
        config.enableLightEstimation = enableLightEstimation;

        if (config.IsSupported) {
            m_session.RunWithConfig(config);
        }

        if (m_camera == null) {
            m_camera = Camera.main;
        }
    }

    public void SetCamera(Camera newCamera) {
        if (m_camera != null) {
            UnityARVideo oldARVideo = m_camera.gameObject.GetComponent<UnityARVideo>();
            if (oldARVideo != null) {
                savedClearMaterial = oldARVideo.m_ClearMaterial;
                Destroy(oldARVideo);
            }
        }
        SetupNewCamera(newCamera);
    }

    private void SetupNewCamera(Camera newCamera) {
        m_camera = newCamera;

        if (m_camera != null) {
            UnityARVideo unityARVideo = m_camera.gameObject.GetComponent<UnityARVideo>();
            if (unityARVideo != null) {
                savedClearMaterial = unityARVideo.m_ClearMaterial;
                Destroy(unityARVideo);
            }
            unityARVideo = m_camera.gameObject.AddComponent<UnityARVideo>();
            unityARVideo.m_ClearMaterial = savedClearMaterial;
        }
    }

    // planeの生成を止めるコンフィグ
    private void StopPlaneConfig(){
        m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

        Application.targetFrameRate = 60;
        ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration();
        //config.planeDetection = planeDetection;
        config.alignment = startAlignment;
        config.getPointCloudData = getPointCloud;
        config.enableLightEstimation = enableLightEstimation;

        if (config.IsSupported) {
            m_session.RunWithConfig(config);
        }

        configFlg = false;
    }



    // Update is called once per frame

    void Update() {
        int num = planeDetector.GetPlaneNum();
        Debug.Log("num:" + num);
        if (configFlg && num >= 1) {
            StopPlaneConfig();
        }
            //}else if(num <= 1){
        //    // startのconfigを実行
        //    //configFlg = false;
        


        if (m_camera != null) {
            // JUST WORKS!
            Matrix4x4 matrix = m_session.GetCameraPose();
            m_camera.transform.localPosition = UnityARMatrixOps.GetPosition(matrix);
            m_camera.transform.localRotation = UnityARMatrixOps.GetRotation(matrix);

            m_camera.projectionMatrix = m_session.GetCameraProjection();
        }

    }

}
