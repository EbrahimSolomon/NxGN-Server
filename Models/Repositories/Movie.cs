using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NxGN_Server.Models.Repositories
{
    public class Movie
    {
        public int movieId { get; set; }
        public string movieName { get; set; }
        public string movieCategory { get; set; }
        public decimal movieRating { get; set; }
        public string movieDescription { get; set; }
        public string movieImageUrl { get; set; }
    }
}