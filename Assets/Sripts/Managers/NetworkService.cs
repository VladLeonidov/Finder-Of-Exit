using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkService
{
    //url don't work
    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us";
    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }

    private bool IsResponseValid(WWW www)
    {
        if (www.error != null)
        {
            Debug.LogError("Bad connection");
            return false;
        }
        else if (string.IsNullOrEmpty(www.text))
        {
            Debug.LogError("Bad data");
            return false;
        }

        return true;
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        WWW www = new WWW(url);
        yield return www;

        if (IsResponseValid(www))
        {
            yield break;
        }

        callback(www.text);
    }
}