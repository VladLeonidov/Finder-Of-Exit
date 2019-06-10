using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using MiniJSON;

public class WeatherManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    public float CloudValue { get; private set; }

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager starting...");

        _network = service;

        StartCoroutine(_network.GetWeatherJSON(OnJSONDataLoaded));

        Status = ManagerStatus.Initializing;
    }

    private void OnXmlDataLoaded(string data)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);
        XmlNode root = doc.DocumentElement;

        XmlNode nodeCloud = root.SelectSingleNode("clouds");
        string value = nodeCloud.Attributes["Value"].Value;

        CloudValue = Convert.ToUInt32(value) / 100f;
        Debug.Log("Value=" + CloudValue);
        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        Status = ManagerStatus.Started;
    }

    private void OnJSONDataLoaded(string data)
    {
        Dictionary<string, object> dic;

        dic = Json.Deserialize(data) as Dictionary<string, object>;

        Dictionary<string, object> clouds = (Dictionary<string, object>) dic["clouds"];
        CloudValue = (long) clouds["All"] / 100f;

        Debug.Log("Value=" + CloudValue);
        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        Status = ManagerStatus.Started;
    }
}