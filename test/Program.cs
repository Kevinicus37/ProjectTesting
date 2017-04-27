using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

public static class Utils
{
    public static bool IsAny<T>(this IEnumerable<T> data)
    {
        return data != null && data.Any();
    }
}

class Program
{
    static void Main(string[] args)
    {
        
        TMDbClient client = new TMDbClient("9950b6bfd3eef8b5c9b7343ead080098");
        Console.Write("Search for a movie:");
        string title = Console.ReadLine();
        SearchContainer<SearchMovie> results = client.SearchMovieAsync(title, 1).Result;

        Console.WriteLine($"\nShowing {results.Results.Count:N0} of {results.TotalResults:N0} results\n");
        int i = 1;
        foreach (SearchMovie result in results.Results)
        {
            Console.WriteLine($"{i}  {result.Title} ({result.ReleaseDate:yyyy})");
            //Movie movie2 = client.GetMovieAsync(result.Id, MovieMethods.Credits | MovieMethods.Videos).Result;
            //Console.WriteLine($"{i}  {movie2.Credits.Cast[0].Name} : {movie2.Credits.Cast[0].Character}");
            i++;
        }

        Console.Write("\nSelect a Movie from the list (Using the number):");
        int Id = int.Parse(Console.ReadLine());

        Movie movie3 = client.GetMovieAsync(results.Results[Id-1].Id, MovieMethods.Credits | MovieMethods.Videos | MovieMethods.Images).Result;

        Console.WriteLine($"\nYou selected the movie: {movie3.Title}");

        Console.WriteLine("\nStarring:\n");
        
        for (int j = 0; j < 5 && j < movie3.Credits.Cast.Count; j++)
         {
            
            Console.WriteLine($"{movie3.Credits.Cast[j].Name} : {movie3.Credits.Cast[j].Character}");
         }

        Console.WriteLine();

        foreach (Video video in movie3.Videos.Results)
            //video.Key is the Youtube address.  So http://www.youtube.com/watch?v={video.Key} links to the youtube video.
            Console.WriteLine($"Trailer: {video.Type} ({video.Site}), {video.Name} {video.Iso_3166_1} {video.Iso_639_1} {video.Id} {video.ToString()} {video.Key}");

        foreach (ImageData image in movie3.Images.Posters)
            Console.WriteLine($"{image.FilePath} {image.AspectRatio}");
        Console.Read();


    }
    
}