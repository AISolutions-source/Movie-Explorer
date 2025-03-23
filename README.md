
Download the APK
You can download the **APK build** from the link below:

[Download APK](https://github.com/AISolutions-source/Movie-Explorer/releases/tag/release-build)

**Movie Explorer**
A Unity-based application that fetches movie details from The Movie Database (TMDb) API and displays them dynamically.

**Features**
Fetch and display movie details (title, overview, genre, cast)
Display up to 6 related movies for each selected movie
Asynchronous API requests using UnityWebRequest
Simple and scalable UI architecture

**Setup Instructions**
**Prerequisites**
Unity 2020+ (Recommended)
Newtonsoft.Json (for JSON parsing)
TMDb API Key (Free signup at TMDb)

**
Install Dependencies**
Install Newtonsoft.Json
Open Unity Package Manager (Window → Package Manager)
Click Add package from git URL
Enter:
 https://github.com/jilleJr/Newtonsoft.Json-for-Unity.git
Set Up TMDb API Key
Get an API Key from TMDb (API Docs)
Store it using PlayerPrefs in Unity:
 PlayerPrefs.SetString("TMDbAPIKey", "your_api_key_here");
PlayerPrefs.Save();
The application will retrieve it automatically.
2️Running the Project
Clone this repository:
 git clone https://github.com/your-repo/unity-movie-app.git


Open the project in Unity
Click Play in the Unity Editor


Data Flow
The user selects a movie
MovieDetailsManager.cs fetches movie details and related movies
Data is parsed via Newtonsoft.Json
UI is updated dynamically via PopulateRelatedMovies()
Up to 6 related movies are displayed
API Endpoints Used
Feature
API Endpoint
Movie Details
/movie/{movie_id}
Related Movies
/movie/{movie_id}/similar
Movie Posters
https://image.tmdb.org/t/p/w500/{poster_path}


**Design Decisions & Trade-offs**
**Why Use UnityWebRequest?**
Pros: Native to Unity, supports async operations
Cons: More verbose than external HTTP libraries
Why Limit Related Movies to 6?
Performance: Reduces UI lag
Better UX: Prevents overwhelming users with too many choices
API Rate Limits: TMDb enforces request limits
**Trade-offs:**
**Design Decision**
Pros
Cons
Using PlayerPrefs for API Key
Easy to store/retrieve
Not encrypted (better stored in a config file)
Limiting Related Movies to 6
Improves UI clarity
Users might want more results
JSON Parsing with Newtonsoft.Json
Reliable, well-documented
Requires an additional package


**Known Issues & Possible Improvements**
**Known Issues**
Issue
Cause
Possible Fix
API Key Missing Error
API key not set in PlayerPrefs
Ensure it's saved before running
Slow Image Loading
UnityWebRequest loads images asynchronously
Implement caching
No Related Movies Found
TMDb doesn't always return results
Add a fallback message




**Credits**
The Movie Database (TMDb) API - Providing movie data
Unity - Game engine powering this app
Newtonsoft.Json - JSON handling



