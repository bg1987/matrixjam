using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    public Material effectMaterial;
    private RenderTexture renderTexture;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectMaterial);
        renderTexture = destination;
    }
    
}
