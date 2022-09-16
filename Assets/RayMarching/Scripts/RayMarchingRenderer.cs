using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  Ray Marching Renderer
	
	Author -  Cole Barach
	Created - 2022.02.11
	Updated - 2022.09.16
	
	Function
		- Monobehavior for renderer recognized by RayMarchCamera.cs
    Notes
        - Render Identities/Materials are programmed via source, plan to work on implementing more generic solution
        - Class is analogous in form to Renderer struct of RayMarchingCamera.cs
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class RayMarchingRenderer : MonoBehaviour {
    [Header("Properties")]
    public int   renderIdentity;        // Shape to render
    public int   materialIdentity;      // Material to render with
    [ColorUsage(true,true)]
    public Color albedo;                // Material color
    [Header("Debug")]
    public bool drawGizmo;
    public bool drawAsWireframe;

    // Debug tool, not necessary for functionality
    void OnDrawGizmos() {
        if(drawGizmo) {
            switch(renderIdentity) {
                case 1:
                    Gizmos.color = albedo;
                    Gizmos.matrix = transform.localToWorldMatrix;
                    if(drawAsWireframe) {
                        Gizmos.DrawWireSphere(Vector3.zero,0.5f);
                    } else {
                        Gizmos.DrawSphere(Vector3.zero,0.5f);
                    }
                    break;
                case 2:
                    Gizmos.color = albedo;
                    Gizmos.matrix = transform.localToWorldMatrix;
                    if(drawAsWireframe) {
                        Gizmos.DrawWireCube(Vector3.zero,Vector3.one);
                    } else {
                        Gizmos.DrawCube(Vector3.zero,Vector3.one);
                    }
                    break;
            }
        }
    }
}
