using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class VRWeatherUI : MonoBehaviour
{
    public TMP_Text tempText;
    public TMP_Text timeText;
    public TMP_Text cityText;
    public TMP_Text weatherText;

    public string apiKey = "80c23f8c76ff1e86c6442aac90f07c7e"; 
    public string city = "Chicoutimi";
    
    void Start()
    {
        UpdateTime();
        InvokeRepeating("UpdateTime", 0f, 60f); 
        StartCoroutine(FetchWeather()); 
    }

    void UpdateTime()
    {
        timeText.text = "Heure: " + System.DateTime.Now.ToShortTimeString();
    }

    IEnumerator FetchWeather()
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=fr";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur API météo : " + request.error);
                tempText.text = "Erreur météo";
                weatherText.text = "-";
            }
            else
            {
                string json = request.downloadHandler.text;
                var data = JObject.Parse(json);

                float temp = data["main"]["temp"].Value<float>();
                string weatherDesc = data["weather"][0]["description"].Value<string>();
                string cityName = data["name"].Value<string>();

                tempText.text = "Temp: " + Mathf.RoundToInt(temp) + "°C";
                weatherText.text = weatherDesc;
                cityText.text = cityName;
            }
        }
    }
}