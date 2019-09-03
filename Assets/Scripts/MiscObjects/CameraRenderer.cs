using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraRenderer : MonoBehaviour
{
    public Camera Camera;
    public Renderer Renderer;

    void Start()
    {
        RenderTexture renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();
        Camera.targetTexture = renderTexture;
        Renderer.material = new Material(Renderer.material);
        

    }

    void Update()
    {


        Renderer.material.mainTexture = GetCameraTexture();
        //Camera.Render();
        
    }

    Texture2D GetCameraTexture()
    {
        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;
        Camera.Render();
        Texture2D texture = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        texture.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = currentRenderTexture;
        return texture;
    }
}