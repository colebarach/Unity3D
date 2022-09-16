using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  A* Visualizer
	
	Author -  Cole Barach
	Created - 2022.02.08
	Updated - 2022.02.08
	
	Function
		- Visualization of A* Algorithm using Debug Gizmos

    Dependencies
        - AStarLibrary.cs
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class AStarGizmo : MonoBehaviour {
    public Texture2D obstructionMap;
    public Vector3 start;
    public Vector3 end;
    public bool drawPath;
    public bool drawObstruction;
    public bool drawStart;
    public bool drawEnd;

    void OnDrawGizmos() {
        if(drawObstruction) {
            Gizmos.color = Color.red;
            for(int x = 0; x < obstructionMap.width; x++) {
                for(int y = 0; y < obstructionMap.height; y++) {
                    if(obstructionMap.GetPixel(x,y) != Color.white) {
                        Vector3 point = transform.TransformPoint(new Vector3(x,0,y));
                        Gizmos.DrawWireCube(point,transform.localScale);
                    }
                }
            }
        }
        if(drawPath) {
            Gizmos.color = Color.green;
            Vector3[] path = AStarLibrary.FindPath(obstructionMap,start,end,transform);
            foreach(Vector3 point in path) {
                Gizmos.DrawWireCube(point,transform.localScale);
            }
        }
        if(drawStart) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(start,Mathf.Min(transform.localScale.x,transform.localScale.y,transform.localScale.z)/2.0f);
        }
        if(drawEnd) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(end,Mathf.Min(transform.localScale.x,transform.localScale.y,transform.localScale.z)/2.0f);
        }
    }
}
