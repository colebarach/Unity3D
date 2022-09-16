using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMaster : MonoBehaviour {
    [Header("References")]
    public PortalMaster   pairedPortal;
    public GameObject     surface;
    public MeshRenderer[] surfaceRenderers;
    public Shader         surfaceShader;
    
    class Subject {
        public GameObject gameObject;
        public Transform transform;
        public GameObject root;
        public Camera camera;
        public CharacterController rootController;
        public int entryDotSign;
        
        public Subject() {}
        public Subject(GameObject constructorGameObject) {
            gameObject = constructorGameObject;
            transform = gameObject.transform;
            root = transform.root.gameObject;
            camera = gameObject.GetComponent<Camera>();
            rootController = root.GetComponent<CharacterController>();
        }
    }
    
    Subject               mainCamera;
    GameObject            mainMedium;
    GameObject            subjectMedium;
    
    BoxCollider           surfaceCollider;
    RenderTexture         surfaceTexture;
    Camera                surfaceTextureRenderer;
    Material              surfaceMaterial;
    
    void Start() {
        mainCamera = new Subject(Camera.main.gameObject);
        Camera.onPreRender += OnCameraPreRender;
        
        if(surface == null) surface = gameObject;
        
        mainMedium = new GameObject(gameObject.name+"RenderMedium");
        mainMedium.transform.parent = transform;
        
        subjectMedium = new GameObject(gameObject.name+"SubjectMedium");
        subjectMedium.transform.parent = transform;
        
        surfaceCollider = surface.GetComponent<BoxCollider>();
        
        surfaceTexture = new RenderTexture(mainCamera.camera.pixelWidth, mainCamera.camera.pixelHeight, 24);
        surfaceTextureRenderer = new GameObject(gameObject.name + "Renderer").AddComponent<Camera>();
        surfaceTextureRenderer.transform.parent = pairedPortal.transform;
        surfaceTextureRenderer.targetTexture = surfaceTexture;
        surfaceTextureRenderer.enabled = false;
        
        surfaceTextureRenderer.cullingMask = -1 & ~(1 << surfaceRenderers[0].gameObject.layer); //improper fix but it works
        
        surfaceMaterial = new Material(surfaceShader);
        surfaceMaterial.SetTexture("_MainTex", surfaceTexture);
        foreach(MeshRenderer renderer in surfaceRenderers) renderer.material = surfaceMaterial;
    }
    public void OnApplicationQuit() {
        Camera.onPreRender -= OnCameraPreRender;
    }
    
    public void OnCameraPreRender(Camera camera) {
        if(camera == mainCamera.camera) {
            mainMedium.transform.position = mainCamera.transform.position;
            mainMedium.transform.rotation = mainCamera.transform.rotation;
            surfaceTextureRenderer.transform.localPosition = mainMedium.transform.localPosition;
            surfaceTextureRenderer.transform.localRotation = mainMedium.transform.localRotation;
            
            surfaceTextureRenderer.projectionMatrix = mainCamera.camera.CalculateObliqueMatrix(CalculateNearClipPlane(surfaceTextureRenderer, pairedPortal.surface.transform));
            
            if(surfaceCollider.bounds.Contains(mainCamera.transform.position)) {
                Vector3 cameraSurfaceVector = mainCamera.transform.position - surface.transform.position;
                int entryDotSign = (int)Mathf.Sign(Vector3.Dot(cameraSurfaceVector, surface.transform.forward));
                if(mainCamera.entryDotSign == 0) mainCamera.entryDotSign = entryDotSign;
                if(mainCamera.entryDotSign != entryDotSign) {
                    mainCamera.entryDotSign = 0;
                    TransferSubject(mainCamera);
                    pairedPortal.ReceiveSubject(mainCamera);
                } else {
                    MaskSurfaceRenderers(entryDotSign);
                    pairedPortal.MaskSurfaceRenderers(-entryDotSign);
                }
            } else if(mainCamera.entryDotSign != 0) {
                mainCamera.entryDotSign = 0;
                EnableSurfaceRenderers();
                pairedPortal.EnableSurfaceRenderers();
            }
            
            //bool[] rendererEnabledArray;
            //pairedPortal.DisableSurfaceRenderers(out rendererEnabledArray);
            surfaceTextureRenderer.Render();
            // pairedPortal.surfaceRenderers[0].enabled = false;
            // pairedPortal.surfaceRenderers[0].enabled = true;
            //pairedPortal.EnableSurfaceRenderers();
        }
    }
    
    Vector4 CalculateNearClipPlane(Camera camera, Transform plane) {
        Vector3 planeCameraVector = plane.position - camera.transform.position;
        int     planeCameraDotSign = (int)Mathf.Sign(Vector3.Dot(plane.forward, planeCameraVector));
        Vector3 cameraSpacePosition = camera.worldToCameraMatrix.MultiplyPoint(plane.position);
        Vector3 cameraSpaceNormal   = camera.worldToCameraMatrix.MultiplyVector(plane.forward)*planeCameraDotSign;
        float   cameraSpaceDistance = -Vector3.Dot(cameraSpacePosition,cameraSpaceNormal);
        Vector4 cameraSpaceClipPlane = new Vector4(cameraSpaceNormal.x,cameraSpaceNormal.y,cameraSpaceNormal.z,cameraSpaceDistance);
        return  cameraSpaceClipPlane;
    }
    
    //recursion

    public void EnableSurfaceRenderers() {
        foreach(MeshRenderer renderer in surfaceRenderers) renderer.enabled = true;
    }
    public void EnableSurfaceRenderers(bool[] enabledArray) {
        for(int x = 0; x < surfaceRenderers.Length; x++) {
            surfaceRenderers[x].enabled = enabledArray[x];
        }
    }
    public void DisableSurfaceRenderers() {
        foreach(MeshRenderer renderer in surfaceRenderers) renderer.enabled = false;
    }
    public void DisableSurfaceRenderers(out bool[] enabledArray) {
        enabledArray = new bool[surfaceRenderers.Length];
        for(int x = 0; x < surfaceRenderers.Length; x++) {
            enabledArray[x] = surfaceRenderers[x].enabled;
            surfaceRenderers[x].enabled = false;
        }
    }
    public void MaskSurfaceRenderers(int entryDotSign) {
        foreach(MeshRenderer renderer in surfaceRenderers) renderer.enabled = (renderer.transform.localPosition.z < 0) == (entryDotSign > 0);
    }

    void TransferSubject(Subject subject) {
        if(subject.rootController != null) subject.rootController.enabled = false;
        
        subjectMedium.transform.position = subject.root.transform.position;
        subjectMedium.transform.rotation = subject.root.transform.rotation;
        Vector3    subjectLocalPosition = subjectMedium.transform.localPosition;
        Quaternion subjectLocalRotation = subjectMedium.transform.localRotation;
        
        subjectMedium.transform.parent = pairedPortal.transform;
        
        subjectMedium.transform.localPosition = subjectLocalPosition;
        subjectMedium.transform.localRotation = subjectLocalRotation;
        subject.root.transform.position = subjectMedium.transform.position;
        subject.root.transform.rotation = subjectMedium.transform.rotation;
        
        subjectMedium.transform.parent = transform;
        
        if(subject.rootController != null) subject.rootController.enabled = true;
    }
    void ReceiveSubject(Subject subject) {
        if(subject.gameObject == mainCamera.gameObject) {
            mainCamera.entryDotSign = -subject.entryDotSign;
        }
    }
}