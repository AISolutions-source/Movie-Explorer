using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways] // Ensures script runs in edit mode
public class ResponsiveUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform searchBar;
    public RectTransform movieList;
    public RectTransform backButton;

    [Header("Grid Layout Settings")]
    public GridLayoutGroup movieGrid;
    public int portraitColumns = 2;
    public int landscapeColumns = 4;
    public Vector2 portraitCellSize = new Vector2(300, 450);
    public Vector2 landscapeCellSize = new Vector2(200, 300);
    public Vector2 portraitSpacing = new Vector2(10, 10);
    public Vector2 landscapeSpacing = new Vector2(15, 15);

    [Header("Text Scaling")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public int portraitFontSize = 28;
    public int landscapeFontSize = 36;

    private ScreenOrientation lastOrientation;

    void Start()
    {
        AdjustUILayout(); // Initial layout setup
        lastOrientation = Screen.orientation;
    }

    void Update()
    {
        if (Application.isPlaying && Screen.orientation != lastOrientation) // Detect orientation change in play mode
        {
            AdjustUILayout();
            lastOrientation = Screen.orientation;
        }
    }

    // Automatically updates positions when modified in Inspector
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            AdjustUILayout();
        }
    }

    void AdjustUILayout()
    {
        if (Screen.width < Screen.height) // Portrait Mode
        {
            AdjustForPortrait();
        }
        else // Landscape Mode
        {
            AdjustForLandscape();
        }
    }

    void AdjustForPortrait()
    {
        // Adjust Grid Layout
        if (movieGrid != null)
        {
            movieGrid.constraintCount = portraitColumns;
            movieGrid.cellSize = portraitCellSize;
            movieGrid.spacing = portraitSpacing;
        }

        // Adjust Text Size
        if (titleText != null) titleText.fontSize = portraitFontSize;
        if (descriptionText != null) descriptionText.fontSize = portraitFontSize;

        // Adjust UI Elements Positions
        if (searchBar != null) searchBar.anchoredPosition = new Vector2(0, -50);
        if (movieList != null) movieList.anchoredPosition = new Vector2(0, -200);
        if (backButton != null) backButton.anchoredPosition = new Vector2(50, -50);
    }

    void AdjustForLandscape()
    {
        // Adjust Grid Layout
        if (movieGrid != null)
        {
            movieGrid.constraintCount = landscapeColumns;
            movieGrid.cellSize = landscapeCellSize;
            movieGrid.spacing = landscapeSpacing;
        }

        // Adjust Text Size
        if (titleText != null) titleText.fontSize = landscapeFontSize;
        if (descriptionText != null) descriptionText.fontSize = landscapeFontSize;

        // Adjust UI Elements Positions
        if (searchBar != null) searchBar.anchoredPosition = new Vector2(0, -30);
        if (movieList != null) movieList.anchoredPosition = new Vector2(0, -150);
        if (backButton != null) backButton.anchoredPosition = new Vector2(80, -30);
    }
}
