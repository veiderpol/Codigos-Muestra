using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using OscJack;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Screenshot : MonoBehaviour
{
    public RawImage imagenGlobo;
    public Texture2D maskTexture;
    public Texture2D sampleTexture;
    public int width, height, startX, startY;
    public TextMeshProUGUI uiTimer;
    byte[] byteArray;
    public static Screenshot instance;
    OscOut _oscOut;

    private void Awake()
    {
        instance = this;
        string dir = string.Format("{0}\\*******", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        dir = dir + @"\*****.txt";
        string text = File.ReadAllText(dir);
        _oscOut = gameObject.AddComponent<OscOut>();
        _oscOut.Open(7000, text);
    }

    public IEnumerator WaitAndDoAction()
    {
        int waitFor = 5;
        uiTimer.gameObject.SetActive(true);
        while (waitFor > 0)
        {
            uiTimer.text = waitFor.ToString();
            yield return new WaitForSeconds(1f);
            waitFor--;
        }
        uiTimer.gameObject.SetActive(false);
        StartCoroutine(ScreenshotFinal());
    }

    public IEnumerator ScreenshotFinal()
    {
        string dir = string.Format("{0}\\*******", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        yield return new WaitForEndOfFrame();

        Camera.main.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        RenderTexture renderTexture = Camera.main.targetTexture;
        Texture2D renderResult = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(startX, startY, width, height);

        renderResult.ReadPixels(rect, 0, 0);
        renderResult.Apply();
        byteArray = renderResult.EncodeToPNG();
        File.WriteAllBytes(string.Format("{0}\\*******\\********.png", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), byteArray);
        imagenGlobo.gameObject.SetActive(true);
        imagenGlobo.texture = renderResult;
        StartCoroutine(CutOutFace(renderResult));
        yield return null;
    }

    public IEnumerator CutOutFace(Texture2D result)
    {

         yield return new WaitForEndOfFrame();

         Texture2D destTexture = new Texture2D(result.width, result.height, TextureFormat.ARGB32, false);
         Color[] textureData = result.GetPixels();
         destTexture.SetPixels(textureData);
         destTexture.Apply();

         TextureScale.Bilinear(destTexture, maskTexture.width, maskTexture.height);
         textureData = destTexture.GetPixels();

         sampleTexture.SetPixels(textureData);

         sampleTexture.Apply();

         Color[] maskPixel = maskTexture.GetPixels();

         Color[] curPixel = sampleTexture.GetPixels();

         int index = 0;

         for (int i = 0; i < maskTexture.height; i++)
         {
             for (int j = 0; j < maskTexture.width; j++)
             {
                 if (maskPixel[index] == maskPixel[0])
                 {
                     curPixel[index] = Color.clear;
                 }
                 index++;
             }
         }
         sampleTexture.SetPixels(curPixel, 0);
         sampleTexture.Apply(false);
         imagenGlobo.gameObject.SetActive(true);
         imagenGlobo.texture = sampleTexture;
         byteArray = sampleTexture.EncodeToPNG();
         File.WriteAllBytes(string.Format("{0}\\******\\******.png", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), byteArray);
         yield return null;
     }

}
