using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NxGN_Server.Models;
using NxGN_Server.Models.Repositories;

namespace NxGN_Server.Interfaces
{
public interface IMovieRepository
{
    Task<ServiceResponse<IEnumerable<Movie>>> GetAllMoviesAsync();
    Task<ServiceResponse<bool>> AddMovieAsync(Movie movie);
    Task<Movie> GetByIdAsync(int id);
    Task<ServiceResponse<bool>> UpdateMovieAsync(Movie movie);
    Task<ServiceResponse<bool>> DeleteMovieByIdAsync(int id);
}
}