using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using OscJack;
using UnityEngine.Networking;

public class OSCRecivier : MonoBehaviour {
    List<Sprite> sprites = new List<Sprite>();
    public List<SpriteRenderer> screenshots = new List<SpriteRenderer>();

    Sprite Globos_temp;
    
    int i = 0;
    int a = 0;
    OscIn _oscIn;
    private void Start()
    {
        _oscIn = gameObject.AddComponent<OscIn>();
        _oscIn.Open(7000);
        _oscIn.MapString("/test", OSCReader);
    }
    public void OSCReader(string text) {
        StartCoroutine(getFile());
    }
    IEnumerator getFile()
    {
        if (i == 9)
        {
           i = 0;
            sprites.Clear();
        }
        a++;
        string[] filePath = Directory.GetFiles(string.Format(@"{0}\ChileMall\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), "*.png");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + filePath[0]);
        yield return www.SendWebRequest();
        Texture2D newTexture = new Texture2D(512,512);
        newTexture = DownloadHandlerTexture.GetContent(www);
        Rect rect = new Rect(0,0,newTexture.width, newTexture.height);
        Globos_temp = Sprite.Create(newTexture,rect, new Vector2(0.5f,0.5f));
        sprites.Insert(0,Globos_temp);
        for (int j = 0; j < sprites.Count; j++)
        {
            screenshots[j].sprite = sprites[j];
        }
        File.Delete(filePath[0]);
        i++;
    }
}
