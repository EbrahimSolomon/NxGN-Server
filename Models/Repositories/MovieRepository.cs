using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NxGN_Server.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NxGN_Server.Models.Repositories
{
   public class MovieRepository : IMovieRepository
{
    private readonly IConfiguration _configuration;

    public MovieRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ServiceResponse<IEnumerable<Movie>>> GetAllMoviesAsync()
    {
    var response = new ServiceResponse<IEnumerable<Movie>>();
    try
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            var movies = await dbConnection.QueryAsync<Movie>("SELECT movieId, movieName, movieCategory, movieRating FROM Movies");

            response.Data = movies;
            response.Success = movies.Any();
            response.Message = movies.Any() ? "Movies fetched successfully." : "No movies found.";
        }
    }
    catch (Exception ex)
    {
        response.Success = false;
        response.Message = $"An unexpected error occurred: {ex.Message}";
    }

    return response;
}


    public async Task<ServiceResponse<bool>> AddMovieAsync(Movie movie)
    {
    var response = new ServiceResponse<bool>();

    try
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();

            string sql = "INSERT INTO Movies(movieName, movieCategory, movieRating, movieDescription, movieImageUrl) VALUES(@movieName, @movieCategory, @movieRating, @movieDescription, @movieImageUrl)";
            var rowsAffected = await dbConnection.ExecuteAsync(sql, movie);

            response.Data = rowsAffected > 0;
            response.Message = response.Data ? "Movie successfully added." : "Movie could not be added.";
            response.Success = response.Data;
        }
    }
    catch (SqlException ex)
    {
        response.Data = false;
        response.Message = $"An error occurred while accessing the database: {ex.Message}";
        response.Success = false;
    }
    catch (Exception ex)
    {
        response.Data = false;
        response.Message = $"An unexpected error occurred: {ex.Message}";
        response.Success = false;
    }

    return response;
}
public async Task<Movie> GetByIdAsync(int id)
{
    var connectionString = _configuration.GetConnectionString("DefaultConnection");
    using (var dbConnection = new SqlConnection(connectionString))
    {
        await dbConnection.OpenAsync();
        return await dbConnection.QuerySingleOrDefaultAsync<Movie>("SELECT * FROM Movies WHERE movieId = @movieId", new { movieId = id });
    }
}

public async Task<ServiceResponse<bool>> UpdateMovieAsync(Movie movie)
{
    var response = new ServiceResponse<bool>();
    try
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            string sql = "UPDATE Movies SET movieName = @movieName, movieCategory = @movieCategory, movieRating = @movieRating, movieDescription = @movieDescription, movieImageUrl = @movieImageUrl WHERE movieId = @movieId";
            var rowsAffected = await dbConnection.ExecuteAsync(sql, movie);
            response.Data = rowsAffected > 0;
            response.Message = response.Data ? "Movie successfully updated." : "Movie could not be updated.";
            response.Success = response.Data;
        }
    }
    catch (Exception ex)
    {
        response.Data = false;
        response.Message = $"An unexpected error occurred: {ex.Message}";
        response.Success = false;
    }
    return response;
}

 public async Task<ServiceResponse<bool>> DeleteMovieByIdAsync(int id)
{
    var response = new ServiceResponse<bool>();
    
    try
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            
            var deletedRows = await dbConnection.ExecuteAsync("DELETE FROM Movies WHERE movieId = @Id", new { Id = id });
            
            if (deletedRows > 0)
            {
                response.Data = true;
                response.Message = "Movie successfully deleted.";
                response.Success = true;
                return response;
            }
            else
            {
                response.Data = false;
                response.Message = "Movie not found.";
                response.Success = false;
                return response;
            }
        }
    }
    catch (SqlException ex)
    {
        response.Data = false;
        response.Message = $"An error occurred while accessing the database: {ex.Message}";
        response.Success = false;
        return response;
    }
    catch (Exception ex)
    {
        response.Data = false;
        response.Message = $"An unexpected error occurred: {ex.Message}";
        response.Success = false;
        return response;
    }
}

}}