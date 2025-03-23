using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the visibility of Home, Movies, and Series panels.
/// </summary>
public class PanelManager : MonoBehaviour
{
    public GameObject homePanel;  // Panel for Home
    public GameObject moviesPanel; // Panel for Movies
    public GameObject seriesPanel; // Panel for Series

    public Button homeButton;
    public Button moviesButton;
    public Button seriesButton;
    MovieSeriesManager msManager;

    void Start()
    {
        msManager = FindFirstObjectByType<MovieSeriesManager>();
        // Ensure only Home is active on start
        ShowHomePanel();

        // Assign button listeners
        homeButton.onClick.AddListener(ShowHomePanel);
        moviesButton.onClick.AddListener(ShowMoviesPanel);
        seriesButton.onClick.AddListener(ShowSeriesPanel);
    }

    /// <summary>
    /// Shows the Home Panel and hides the others.
    /// </summary>
    public void ShowHomePanel()
    {
        homePanel.SetActive(true);
        moviesPanel.SetActive(false);
        seriesPanel.SetActive(false);
        UpdateButtonColors(homeButton);
    }

    /// <summary>
    /// Shows the Movies Panel and hides the others.
    /// </summary>
    public void ShowMoviesPanel()
    {
        homePanel.SetActive(false);
        moviesPanel.SetActive(true);
        seriesPanel.SetActive(false);
        UpdateButtonColors(moviesButton);
        MovieSeriesManager msManager = FindFirstObjectByType<MovieSeriesManager>();
        msManager.StartMovies();
    }

    /// <summary>
    /// Shows the Series Panel and hides the others.
    /// </summary>
    public void ShowSeriesPanel()
    {
        homePanel.SetActive(false);
        moviesPanel.SetActive(false);
        seriesPanel.SetActive(true);
        UpdateButtonColors(seriesButton);
        MovieSeriesManager msManager = FindFirstObjectByType<MovieSeriesManager>();
        msManager.StartSeries();
    }

    /// <summary>
    /// Updates button colors to highlight the active one.
    /// </summary>
    private void UpdateButtonColors(Button activeButton)
    {
        homeButton.GetComponent<Image>().color = Color.white;
        moviesButton.GetComponent<Image>().color = Color.white;
        seriesButton.GetComponent<Image>().color = Color.white;
    }
}
