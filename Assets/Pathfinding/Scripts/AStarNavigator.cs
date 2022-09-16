using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  A* Navigator
	
	Author -  Cole Barach
	Created - 2022.02.07
	Updated - 2022.04.10
	
	Function
		- Transformation of GameObject based on A* Algorithm

    Dependencies
        - AStarLibrary.cs
    
    Notes
        - Work on this has been put aside for an alternative method used in another project
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class AStarNavigator : MonoBehaviour {
    public Texture2D pathOclusion;
    public Vector3 pathScale;
    public Vector3 pathTransform;
    public Vector2Int A;
    public Vector2Int B;
    public GameObject cube;

    void Start() {
        
    }
}
