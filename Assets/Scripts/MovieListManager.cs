using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MovieListManager : MonoBehaviour
{
    public TMDbAPIManager apiManager;
    public TMP_InputField searchInput;
    public Transform contentPanel;
    public GameObject moviePrefab;

    private static string lastSearchQuery = "";
    private static List<Movie> lastSearchResults = new List<Movie>();

    void Start()
    {
        if (!string.IsNullOrEmpty(lastSearchQuery) && lastSearchResults.Count > 0)
        {
            // Restore previous search when returning from details page
            searchInput.text = lastSearchQuery;
            PopulateMovieList(lastSearchResults);
        }
    }

    public void OnSearchClicked()
    {
        string query = searchInput.text.Trim();
        if (!string.IsNullOrEmpty(query))
        {
            lastSearchQuery = query; // Store search query
            StartCoroutine(apiManager.SearchMovie(query, SaveAndPopulateMovieList));
        }
    }

    private void SaveAndPopulateMovieList(List<Movie> movies)
    {
        lastSearchResults = movies; // Store search results
        PopulateMovieList(movies);
    }

    private void PopulateMovieList(List<Movie> movies)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Movie movie in movies)
        {
            GameObject movieItem = Instantiate(moviePrefab, contentPanel);
            movieItem.GetComponent<MovieItem>().Setup(movie);
        }
    }
}
