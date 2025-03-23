using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class APIKeyManager : MonoBehaviour
{
    public TMP_InputField apiKeyInput;
    public GameObject apiKeyPanel;
    public TMP_Text statusText; // Used for both error and success messages

    void Start()
    {
        if (PlayerPrefs.HasKey("TMDbAPIKey"))
        {
            string savedKey = PlayerPrefs.GetString("TMDbAPIKey");
            Debug.Log($"Saved API Key: {savedKey}"); //Log the stored key
            apiKeyPanel.SetActive(false); // Hide if key exists
        }
        else
        {
            apiKeyPanel.SetActive(true);
            statusText.text = "";
        }
    }


    public void ValidateAndSaveAPIKey()
    {
        string apiKey = apiKeyInput.text.Trim();
        if (!string.IsNullOrEmpty(apiKey))
        {
            StartCoroutine(CheckAPIKey(apiKey));
        }
        else
        {
            ShowMessage("API Key cannot be empty.", Color.red, 2f);
        }
    }

    private IEnumerator CheckAPIKey(string apiKey)
    {
        string testUrl = $"https://api.themoviedb.org/3/movie/550?api_key={apiKey}"; // Test request to validate API Key
        Debug.Log($"Checking API Key with URL: {testUrl}"); //Debug log to check request

        using (UnityWebRequest request = UnityWebRequest.Get(testUrl))
        {
            yield return request.SendWebRequest();

            Debug.Log($"API Response: {request.downloadHandler.text}"); //Log full API response

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API Key is valid!"); //Log success
                PlayerPrefs.SetString("TMDbAPIKey", apiKey);
                PlayerPrefs.Save();
                StartCoroutine(ShowSuccessAndClosePanel());
            }
            else
            {
                Debug.LogError($"Invalid API Key. Error: {request.error}");
                ShowMessage("Invalid API Key. Please try again.", Color.red, 2f);
            }
        }
    }


    private void ShowMessage(string message, Color color, float duration)
    {
        statusText.text = message;
        statusText.color = color;
        StartCoroutine(HideMessageAfterDelay(duration));
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        statusText.text = "";
    }

    private IEnumerator ShowSuccessAndClosePanel()
    {
        ShowMessage("API Key saved successfully!", Color.green, 1f);
        yield return new WaitForSeconds(1f);
        apiKeyPanel.SetActive(false);
    }
}
