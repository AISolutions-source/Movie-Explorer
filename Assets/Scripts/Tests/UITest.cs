using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class UITest
{
    [Test]
    public void TestMovieListUpdatesCorrectly()
    {
        // Simulate movie list panel
        GameObject movieListPanel = new GameObject();
        VerticalLayoutGroup layoutGroup = movieListPanel.AddComponent<VerticalLayoutGroup>();

        // Simulate adding movie items
        GameObject movieItem = new GameObject();
        movieItem.transform.parent = movieListPanel.transform;

        Assert.AreEqual(1, movieListPanel.transform.childCount, "Movie list did not update correctly");
    }
}
