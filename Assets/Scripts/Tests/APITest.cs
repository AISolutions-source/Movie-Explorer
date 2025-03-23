using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using UnityEngine.Networking;

public class APITest
{
    private string apiKey;
    private string testQuery = "Inception";

    [SetUp] // Runs before each test
    public void Setup()
    {
        if (!PlayerPrefs.HasKey("TMDbAPIKey"))
        {
            Assert.Fail("No API Key found. Make sure APIKeyManager has saved a valid key.");
        }
        else
        {
            apiKey = PlayerPrefs.GetString("TMDbAPIKey");
        }
    }

    [UnityTest]
    public IEnumerator TestAPIResponseIsValid()
    {
        string url = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={testQuery}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Assert.Fail($"API request failed: {request.error}");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Assert.IsNotEmpty(responseText, "API response is empty");
            }
        }
    }
}
