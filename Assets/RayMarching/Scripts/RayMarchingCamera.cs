using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Ray Marching Camera
	
	Author -  Cole Barach
	Created - 2022.02.11
	Updated - 2022.09.16
	
	Function
		- Rendering of RayMarchingObject.cs Monobehavior
		- Dispatching of RayMarchingRenderer.compute
        - Blitting of rendered image to screen
    Dependencies
        - RayMarchingRenderer.compute
        - RayMarchingRenderer.cs
    Notes
        - Z-Component of transform.localScale should be set to -1 due to render methods
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class RayMarchingCamera : MonoBehaviour {
    [Header("Shader")]
    public ComputeShader renderShader;                              // Shader responsible for generation of render
    public Vector3Int    renderThreads = new Vector3Int(16,16,1);   // Number of threads for shader execution (Do not modify without modifying globally)
    [Header("Render Parameters")]
    public int   rayDepth              = 64;                        // Number of ray steps
    public float iso                   = 0.01f;                     // Threshold of collision
    [ColorUsage(true,true)]
    public Color skybox                = Color.black;               // Skybox color
    public int   maxDistance           = 1024;                      // Max distance, for optimization

    RenderTexture         render;                                   // Result of rendering
    int                   renderKernel;                             // Identity of shader kernel
    RayMarchingRenderer[] renderStack;                              // Classes of objects to render
    Renderer[]            renderStructs;                            // Structs of objects to render
    ComputeBuffer         renderBuffer;                             // Buffer of objects to render

    Camera                cameraComponent;                          // Camera reference
    Matrix4x4             cameraTransform;                          // Camera transform matrix
    Matrix4x4             cameraInverseProjection;                  // Camera projection matrix

    // Object to be rendered (Correlates to an instance of RayMarchingRenderer.cs)
    struct Renderer {
        public Matrix4x4 transform;
        public int       renderIdentity;
        public int       materialIdentity;
        public Color     albedo;

        static public int GetStride() {
            return sizeof(float)*4*4  + 2*sizeof(int) + sizeof(float)*4;
        }
    };

    // Initialize variables on startup
    void Start() {
        cameraComponent = GetComponent<Camera>();
        renderKernel = renderShader.FindKernel("Render");
        render = new RenderTexture(Screen.width,Screen.height,24);
        render.enableRandomWrite = true;
    }
    // Render Image
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        // Initializations
        InitializeRenderTexture(render,source);
        InitializeStack();
        InitializeCamera();

        // Setting shader parameters
        renderShader.SetTexture(renderKernel, "render",            render);
        renderShader.SetBuffer( renderKernel, "stack",             renderBuffer);
        renderShader.SetInt(                  "stackCount",        renderStack.Length);
        renderShader.SetMatrix(               "transform",         cameraTransform);
        renderShader.SetMatrix(               "inverseProjection", cameraInverseProjection);
        renderShader.SetInt(                  "depth",             rayDepth);
        renderShader.SetFloat(                "iso",               iso);
        renderShader.SetVector(               "skybox",            skybox);
        renderShader.SetInt(                  "maxDistance",       maxDistance);

        // Execute Shader
        renderShader.Dispatch(renderKernel,render.width/renderThreads.x,render.height/renderThreads.y,renderThreads.z);
        
        // Render result to screen
        Graphics.Blit(render,destination);
    }
    // Dispose Buffers
    void OnApplicationQuit() {
        if(render != null) render.Release();
        if(renderBuffer != null) renderBuffer.Release();
    }

    // Check for resolution changes
    void InitializeRenderTexture(RenderTexture destination, RenderTexture source) {
        if(destination.width != source.width || destination.height != source.height) {
            destination.Release();
            destination = new RenderTexture(source.width,source.height,source.depth);
            destination.enableRandomWrite = true;
        }
    }
    // Get RayMarchingRenderer objects and create buffer
    void InitializeStack() {
        renderStack = GameObject.FindObjectsOfType<RayMarchingRenderer>();
        if(renderBuffer == null || renderStack.Length != renderBuffer.count) {
            if(renderBuffer != null) renderBuffer.Release();
            renderBuffer = new ComputeBuffer(renderStack.Length, Renderer.GetStride());
            renderStructs = new Renderer[renderStack.Length];
        }
        for(int x = 0; x < renderStructs.Length; x++) {
            renderStructs[x].transform        = renderStack[x].transform.worldToLocalMatrix;
            renderStructs[x].renderIdentity   = renderStack[x].renderIdentity;
            renderStructs[x].materialIdentity = renderStack[x].materialIdentity;
            renderStructs[x].albedo           = renderStack[x].albedo;
        }
        renderBuffer.SetData(renderStructs);
    }
    // Get transform and projection 
    void InitializeCamera() {
        cameraTransform = transform.localToWorldMatrix;
        cameraInverseProjection = Matrix4x4.Inverse(cameraComponent.projectionMatrix);
    }
}