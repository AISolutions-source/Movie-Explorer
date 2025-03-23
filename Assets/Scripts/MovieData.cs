using System.Collections.Generic;

/// <summary>
/// Represents a movie object retrieved from the API.
/// </summary>
[System.Serializable]
public class Movie
{
    public int id;
    public string title;
    public string overview;
    public string poster_path;
    public List<Genre> genres; // Stores genre information
    public List<CastMember> cast; // Stores cast information
}

/// <summary>
/// Represents a movie genre.
/// </summary>
[System.Serializable]
public class Genre
{
    public int id;
    public string name;
}

/// <summary>
/// Represents a cast member of a movie.
/// </summary>
[System.Serializable]
public class CastMember
{
    public int id;
    public string name;
    public string character;
    public string profile_path;
}

/// <summary>
/// Represents the structure of the movie search API response.
/// </summary>
[System.Serializable]
public class MovieSearchResult
{
    public List<Movie> results;
}

/// <summary>
/// Represents additional movie details (genres, credits).
/// </summary>
[System.Serializable]
public class MovieDetailsResponse
{
    public List<Genre> genres;
    public Credits credits;
}

[System.Serializable]
public class Credits
{
    public List<CastMember> cast;
}
