using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class MovieItem : MonoBehaviour
{
    public TMP_Text titleText;
    public Image posterImage;
    private Movie movieData;

    public void Setup(Movie movie)
    {
        gameObject.SetActive(true); //Ensure the GameObject is active
        movieData = movie;
        titleText.text = movie.title;
        StartCoroutine(LoadImage(movie.poster_path)); //Now safe to start coroutine
    }


    private IEnumerator LoadImage(string posterPath)
    {
        string url = $"https://image.tmdb.org/t/p/w500{posterPath}";
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                posterImage.sprite = Sprite.Create(((DownloadHandlerTexture)request.downloadHandler).texture, new Rect(0, 0, 500, 750), Vector2.zero);
            }
        }
    }

    public void OnMovieClicked()
    {
        MovieDetailsManager.selectedMovie = movieData;
        SceneManager.LoadScene("MovieDetailsScene");
    }
}
