using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Manages API calls to TMDb, fetching and caching movie data.
/// Implements IMovieService to allow for testing and modularity.
/// </summary>
public class TMDbAPIManager : MonoBehaviour, IMovieService
{
    private const string baseUrl = "https://api.themoviedb.org/3/search/movie";
    private string cacheFilePath;

    private void Awake()
    {
        cacheFilePath = Application.persistentDataPath + "/movie_cache.json";
    }

    /// <summary>
    /// Searches for movies from TMDb API and retrieves additional details (genres, cast, etc.).
    /// </summary>
    public IEnumerator SearchMovie(string query, System.Action<List<Movie>> callback)
    {
        string apiKey = PlayerPrefs.GetString("TMDbAPIKey", "");
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing.");
            callback(null);
            yield break;
        }

        // Check cache first
        if (TryLoadFromCache(query, out List<Movie> cachedMovies))
        {
            Debug.Log("Loaded from cache.");
            callback(cachedMovies);
            yield break;
        }

        string url = $"{baseUrl}?api_key={apiKey}&query={UnityWebRequest.EscapeURL(query)}&include_adult=false";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 10;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                MovieSearchResult result = null;

                try
                {
                    result = JsonConvert.DeserializeObject<MovieSearchResult>(json);
                }
                catch (JsonException e)
                {
                    Debug.LogError("JSON Parsing Error: " + e.Message);
                    callback(null);
                    yield break;
                }

                if (result != null && result.results != null && result.results.Count > 0)
                {
                    foreach (var movie in result.results)
                    {
                        yield return FetchMovieDetails(movie.id, movie);
                    }

                    SaveToCache(query, result.results);
                    callback(result.results);
                }
                else
                {
                    Debug.LogWarning("No movies found.");
                    callback(new List<Movie>());
                }
            }
            else
            {
                Debug.LogError("API Error: " + request.error);
                callback(null);
            }
        }
    }



    /// <summary>
    /// Fetches full movie details, including genres and cast.
    /// </summary>
    private IEnumerator FetchMovieDetails(int movieId, Movie movie)
    {
        string apiKey = PlayerPrefs.GetString("TMDbAPIKey", "");
        string url = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}&append_to_response=credits";

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
    /// Saves search results to cache.
    /// </summary>
    private void SaveToCache(string query, List<Movie> movies)
    {
        Dictionary<string, List<Movie>> cache = LoadCache();
        cache[query.ToLower()] = movies;

        string json = JsonConvert.SerializeObject(cache);
        File.WriteAllText(cacheFilePath, json);
    }

    /// <summary>
    /// Loads cached search results if available.
    /// </summary>
    private bool TryLoadFromCache(string query, out List<Movie> movies)
    {
        if (File.Exists(cacheFilePath))
        {
            string json = File.ReadAllText(cacheFilePath);
            Dictionary<string, List<Movie>> cache = JsonConvert.DeserializeObject<Dictionary<string, List<Movie>>>(json);

            if (cache.ContainsKey(query.ToLower()))
            {
                movies = cache[query.ToLower()];
                return true;
            }
        }

        movies = null;
        return false;
    }

    private Dictionary<string, List<Movie>> LoadCache()
    {
        if (File.Exists(cacheFilePath))
        {
            string json = File.ReadAllText(cacheFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, List<Movie>>>(json);
        }
        return new Dictionary<string, List<Movie>>();
    }
}



