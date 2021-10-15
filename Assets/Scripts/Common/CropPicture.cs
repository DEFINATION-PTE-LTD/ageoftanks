using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 相机截图
/// </summary>
public class CropPicture : MonoBehaviour
{
    public Camera cropCamera; //待截图的目标摄像机
    RenderTexture renderTexture;
    Texture2D texture2D;

    void Start()
    {
        renderTexture = new RenderTexture(1000, 1000, 32);
        texture2D = new Texture2D(1000, 1000, TextureFormat.ARGB32, false);
        cropCamera.targetTexture = renderTexture;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = null;

            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "//pic//" + (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds + ".png", bytes);
        }
    }
}
