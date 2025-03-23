using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;

/// <summary>
/// Manages displaying movie details in the UI.
/// </summary>
public class MovieDetailsManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text genreText;
    public TMP_Text categoryText;
    public TMP_Text castText;
    public Image posterImage;
    public static Movie selectedMovie;

    [Header("Related Movies")]
    public Transform relatedMoviesContentPanel; // Panel for related movies
    public GameObject relatedMovieItemPrefab; // Prefab for related movie items

    private string apiKey; //Add your API key here!


    void Start()
    {
        apiKey = PlayerPrefs.GetString("TMDbAPIKey", "");
        Debug.Log("Retrieved API Key: " + apiKey);

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing. Ensure you have set it in PlayerPrefs.");
            return; // Exit early if the key is missing
        }

        if (selectedMovie != null)
        {
            titleText.text = selectedMovie.title;
            descriptionText.text = selectedMovie.overview;
            genreText.text = FormatGenres(selectedMovie.genres);
            categoryText.text = FormatCategories(selectedMovie.genres);
            castText.text = FormatCast(selectedMovie.cast);
            StartCoroutine(LoadImage(selectedMovie.poster_path));

            if (selectedMovie.id > 0)
            {
                StartCoroutine(FetchRelatedMovies(selectedMovie.id));
            }
        }
    }


    private IEnumerator LoadImage(string posterPath)
    {
        string url = $"https://image.tmdb.org/t/p/w500{posterPath}";
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                posterImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
        }
    }

    private string FormatGenres(List<Genre> genres)
    {
        return genres != null && genres.Count > 0 ? string.Join(", ", genres.ConvertAll(g => g.name)) : "Unknown";
    }

    private string FormatCategories(List<Genre> genres)
    {
        if (genres == null || genres.Count == 0) return "Unknown";

        if (genres.Exists(g => g.name.Contains("Action"))) return "Action Movies";
        if (genres.Exists(g => g.name.Contains("Comedy"))) return "Comedy Films";
        if (genres.Exists(g => g.name.Contains("Horror"))) return "Horror Collection";
        return "Other";
    }

    private string FormatCast(List<CastMember> cast)
    {
        return cast != null && cast.Count > 0 ? string.Join(", ", cast.GetRange(0, Mathf.Min(5, cast.Count)).ConvertAll(c => $"{c.name} ({c.character})")) : "Unknown";
    }

    public void OnBackClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    /// <summary>
    /// Fetches related movies from TMDB API
    /// </summary>
    IEnumerator FetchRelatedMovies(int movieId)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing. Cannot fetch related movies.");
            yield break;
        }

        string url = $"https://api.themoviedb.org/3/movie/{movieId}/similar?api_key={apiKey}&language=en-US&page=1";
        Debug.Log("Fetching related movies from: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                MovieSearchResult result = JsonConvert.DeserializeObject<MovieSearchResult>(json);

                if (result.results != null && result.results.Count > 0)
                {
                    PopulateRelatedMovies(result.results);
                }
                else
                {
                    Debug.LogWarning("No related movies found!");
                }
            }
            else
            {
                Debug.LogError("Failed to fetch related movies: " + request.error);
            }
        }
    }


    /// <summary>
    /// Populates the Related Movies section
    /// </summary>
    void PopulateRelatedMovies(List<Movie> relatedMovies)
    {
        //Clear previous movies
        foreach (Transform child in relatedMoviesContentPanel)
        {
            Destroy(child.gameObject);
        }

        //Limit to 6 movies
        int maxItems = Mathf.Min(6, relatedMovies.Count);
        for (int i = 0; i < maxItems; i++)
        {
            GameObject newRelatedMovie = Instantiate(relatedMovieItemPrefab, relatedMoviesContentPanel);
            newRelatedMovie.SetActive(true);
            newRelatedMovie.GetComponent<MovieItem>().Setup(relatedMovies[i]);
        }
    }

}
