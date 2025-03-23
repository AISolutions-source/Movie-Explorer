using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for fetching movie data from different sources (API, cache, mock).
/// </summary>
public interface IMovieService
{
    /// <summary>
    /// Searches for movies based on a query.
    /// </summary>
    /// <param name="query">The movie title to search for.</param>
    /// <param name="callback">Callback function to handle search results.</param>
    IEnumerator SearchMovie(string query, System.Action<List<Movie>> callback);
}
