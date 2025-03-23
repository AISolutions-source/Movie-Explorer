using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;

/// <summary>
/// Fetches and displays trending movies and TV series in separate panels.
/// </summary>
public class MovieSeriesManager : MonoBehaviour
{
    public Transform moviesContentPanel; // Movies ScrollView Content holder
    public Transform seriesContentPanel; // Series ScrollView Content holder
    public GameObject movieItemPrefab; // Prefab for both movies and series

    private string apiKey;

    private void Start()
    {
        apiKey = PlayerPrefs.GetString("TMDbAPIKey", "");
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing.");
            return;
        }
    }

    public void StartMovies()
    {
        StartCoroutine(FetchTrendingMovies());
    }

    public void StartSeries()
    {

        StartCoroutine(FetchTrendingSeries());
    }

    /// <summary>
    /// Fetches trending movies from TMDb API.
    /// </summary>
    IEnumerator FetchTrendingMovies()
    {
        List<Movie> allMovies = new List<Movie>();

        for (int page = 1; page <= 2; page++) //Fetch two pages
        {
            string url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}&language=en-US&page={page}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    MovieSearchResult result = JsonConvert.DeserializeObject<MovieSearchResult>(json);

                    if (result.results != null)
                    {
                        allMovies.AddRange(result.results);
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch trending movies: " + request.error);
                }
            }
        }

        PopulateMovieSuggestions(allMovies, moviesContentPanel);
    }


    /// <summary>
    /// Fetches trending TV series from TMDb API.
    /// </summary>
    IEnumerator FetchTrendingSeries()
    {
        List<Movie> allSeries = new List<Movie>();

        for (int page = 1; page <= 2; page++) //Fetch two pages
        {
            string url = $"https://api.themoviedb.org/3/tv/popular?api_key={apiKey}&language=en-US&page={page}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    MovieSearchResult result = JsonConvert.DeserializeObject<MovieSearchResult>(json);

                    if (result.results != null)
                    {
                        allSeries.AddRange(result.results);
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch trending series: " + request.error);
                }
            }
        }

        PopulateMovieSuggestions(allSeries, seriesContentPanel);
    }


    /// <summary>
    /// Populates a ScrollView with up to 6 movies or series.
    /// </summary>
    void PopulateMovieSuggestions(List<Movie> items, Transform contentPanel)
    {
        // Clear previous items
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        int maxItems = Mathf.Min(30, items.Count); 

        for (int i = 0; i < maxItems; i++)
        {
            GameObject newMovieItem = Instantiate(movieItemPrefab, contentPanel);
            newMovieItem.GetComponent<MovieItem>().Setup(items[i]); // Pass full movie/series data
        }
    }

}
