using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;

public class MovieSuggestionManager : MonoBehaviour
{
    public Transform contentPanel; // ScrollView Content holder
    public GameObject movieItemPrefab; // Prefab for suggested movie items
    private string apiKey;

    void Start()
    {
        apiKey = PlayerPrefs.GetString("TMDbAPIKey", "");
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing.");
            return;
        }

        // Fetch trending movies for the homepage
        StartCoroutine(FetchTrendingMovies());
    }

    /// <summary>
    /// Fetches trending/popular movies (basic details) from TMDb API.
    /// Then fetches full details (genres, cast) for each movie.
    /// </summary>
    IEnumerator FetchTrendingMovies()
    {
        string url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}&language=en-US&page=1";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                MovieSearchResult result = JsonConvert.DeserializeObject<MovieSearchResult>(json);

                if (result.results != null && result.results.Count > 0)
                {
                    // Fetch full details for the first 6 movies
                    List<Movie> fullMovies = new List<Movie>();
                    int maxMovies = Mathf.Min(6, result.results.Count);

                    for (int i = 0; i < maxMovies; i++)
                    {
                        yield return StartCoroutine(FetchMovieDetails(result.results[i], fullMovies));
                    }

                    PopulateMovieSuggestions(fullMovies);
                }
            }
            else
            {
                Debug.LogError("Failed to fetch trending movies: " + request.error);
            }
        }
    }

    /// <summary>
    /// Fetches full movie details (genres, cast).
    /// </summary>
    IEnumerator FetchMovieDetails(Movie movie, List<Movie> fullMovies)
    {
        string url = $"https://api.themoviedb.org/3/movie/{movie.id}?api_key={apiKey}&append_to_response=credits";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                try
                {
                    MovieDetailsResponse details = JsonConvert.DeserializeObject<MovieDetailsResponse>(json);
                    movie.genres = details.genres;
                    movie.cast = details.credits.cast;
                    fullMovies.Add(movie);
                }
                catch (JsonException e)
                {
                    Debug.LogError("Error fetching movie details: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Failed to fetch movie details: " + request.error);
            }
        }
    }

    /// <summary>
    /// Populates the ScrollView with up to 6 trending movies with full details.
    /// </summary>
    void PopulateMovieSuggestions(List<Movie> movies)
    {
        // Clear previous suggestions
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < movies.Count; i++)
        {
            GameObject newMovieItem = Instantiate(movieItemPrefab, contentPanel);
            newMovieItem.GetComponent<MovieItem>().Setup(movies[i]); // Pass full movie data
        }
    }
}
