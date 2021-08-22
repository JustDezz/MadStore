using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class IconMaker : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject modelHolder;
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private RenderTexture texture;
    [SerializeField] private Color chromaKey;
    [Range(0, 1)] [SerializeField] private float prescision;

    public void MakeIcons()
    {
        Camera cam = Camera.main;
        cam.targetTexture = texture;
        Vector3 bottomLeft = cam.WorldToScreenPoint(
            background.transform.position - Vector3.right * background.transform.localScale.x / 2 - Vector3.up * background.transform.localScale.y / 2);
        Vector3 topRight = cam.WorldToScreenPoint(
            background.transform.position + Vector3.right * background.transform.localScale.x / 2 + Vector3.up * background.transform.localScale.y / 2);

        Rect rect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
        Texture2D icon = new Texture2D(Mathf.RoundToInt(topRight.x - bottomLeft.x), Mathf.RoundToInt(topRight.y - bottomLeft.y), TextureFormat.RGBA32, false);
        RenderTexture.active = cam.targetTexture;

        ComputeBuffer compareColor = new ComputeBuffer(1, sizeof(float) * 4);
        compareColor.SetData(new float[] { chromaKey.r, chromaKey.g, chromaKey.b, 1 });
        computeShader.SetBuffer(0, "ColorToCompare", compareColor);
        computeShader.SetFloat("Prescision", prescision);

        for (int i = 0; i < modelHolder.transform.childCount; i++)
        {
            var model = modelHolder.transform.GetChild(i);
            model.gameObject.SetActive(true);
            cam.Render();
            icon.ReadPixels(rect, 0, 0);

            Color[] colors = icon.GetPixels();
            ComputeBuffer colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
            colorsBuffer.SetData(colors);
            computeShader.SetBuffer(0, "input", colorsBuffer);

            int numOfXThreads = Mathf.RoundToInt(icon.width / 16 + 0.5f);
            int numOfYThreads = Mathf.RoundToInt(icon.height / 8 + 0.5f);
            computeShader.SetInt("numOfXThreads", numOfXThreads);
            computeShader.SetInt("numOfYThreads", numOfYThreads);

            computeShader.Dispatch(0, numOfYThreads, numOfYThreads, numOfYThreads);

            colorsBuffer.GetData(colors);
            icon.SetPixels(colors);
            icon.Apply();

            System.IO.File.WriteAllBytes(Application.dataPath + $"/Sprites/Icons/{model.name}Icon.png", icon.EncodeToPNG());
            model.gameObject.SetActive(false);

            colorsBuffer.Dispose();
        }
        compareColor.Dispose();
        cam.targetTexture = null;
    }
}
