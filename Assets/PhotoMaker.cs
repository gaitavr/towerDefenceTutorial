using UnityEngine;
using System.Collections;
using System.IO;

public class PhotoMaker : MonoBehaviour
{
    private Camera _cam;
    private Texture2D _photo;
    private string _path;
    private int _attempt;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _path = Application.persistentDataPath;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            MakePhoto();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ShowFolder();
        }
    }

    private void MakePhoto()
    {
        var width = Screen.width;
        var height = Screen.height;
        _photo = new Texture2D(width, height, TextureFormat.RGBA32, false);

        RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        _cam.targetTexture = rt;
        _cam.Render();
        RenderTexture.active = _cam.targetTexture;
        _photo.ReadPixels(new Rect(0, 0, width, height), 0, 0);

        _cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);
        _photo.Apply();
        _attempt++;
        File.WriteAllBytes(_path + $"test{_attempt}.jpg", _photo.EncodeToJPG(100));
    }

    private void ShowFolder()
    {
        UnityEditor.EditorUtility.RevealInFinder(_path);
    }
}
