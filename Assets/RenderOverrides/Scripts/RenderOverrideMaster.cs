using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderOverrideMaster : MonoBehaviour {
    public Material material;
    public FilterMode filtering = FilterMode.Bilinear;
    public Vector2Int dimensionOverride;
    public int depthOverride = -1;

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Vector2Int dimensions = dimensionOverride;
        int depth = depthOverride;
        if(dimensions.x <= 0 || dimensions.y <= 0) dimensions = new Vector2Int(source.width,source.height);
        if(depth < 0) depth = source.depth;
        RenderTexture medium = RenderTexture.GetTemporary(dimensions.x, dimensions.y, depth);
        medium.filterMode = filtering;
        if(material != null) Graphics.Blit(source, medium, material); else Graphics.Blit(source, medium);
        Graphics.Blit(medium, destination);
        RenderTexture.ReleaseTemporary(medium);
    }
}
