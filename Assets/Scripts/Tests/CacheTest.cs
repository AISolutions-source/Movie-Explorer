using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Unit test for testing movie search caching functionality.
/// Uses Unity's JsonUtility for serialization.
/// </summary>
public class CacheTest
{
    private string cachePath;

    [SetUp] // Runs before each test
    public void Setup()
    {
        cachePath = Application.persistentDataPath + "/movie_cache.json";
    }

    [Test]
    public void TestCacheSaveAndLoad()
    {
        // Create sample movie data
        List<Movie> testMovies = new List<Movie>
        {
            new Movie { id = 1, title = "Inception", overview = "A mind-bending thriller.", poster_path = "/poster.jpg" }
        };

        // Create a test cache wrapper object
        CacheWrapper testCache = new CacheWrapper
        {
            movies = testMovies,
            query = "inception"
        };

        // Save to cache file
        string json = JsonUtility.ToJson(testCache);
        File.WriteAllText(cachePath, json);

        // Load from cache file
        string loadedJson = File.ReadAllText(cachePath);
        CacheWrapper loadedCache = JsonUtility.FromJson<CacheWrapper>(loadedJson);

        // Assertions
        Assert.IsNotNull(loadedCache, "Cache file is empty or corrupted.");
        Assert.AreEqual("inception", loadedCache.query, "Query string does not match.");
        Assert.IsNotNull(loadedCache.movies, "Cached movie list is null.");
        Assert.IsNotEmpty(loadedCache.movies, "Cached movie list is empty.");
        Assert.AreEqual("Inception", loadedCache.movies[0].title, "Movie title does not match.");
    }

    [TearDown] // Runs after each test to clean up test files
    public void Cleanup()
    {
        if (File.Exists(cachePath))
        {
            File.Delete(cachePath);
        }
    }

    /// <summary>
    /// Wrapper class for serializing movie cache using Unity's JsonUtility.
    /// </summary>
    [System.Serializable]
    private class CacheWrapper
    {
        public string query; // Store the search query
        public List<Movie> movies; // List of cached movies
    }

    /// <summary>
    /// Represents a movie object retrieved from the API.
    /// </summary>
    [System.Serializable]
    private class Movie
    {
        public int id;
        public string title;
        public string overview;
        public string poster_path;
    }
}
