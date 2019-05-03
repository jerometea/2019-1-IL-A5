using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Algo
{
    public class RecoContext
    {
        public IReadOnlyList<User> Users { get; private set; }
        public IReadOnlyList<Movie> Movies { get; private set; }
        public int RatingCount { get; private set; }

        public bool LoadFrom(string folder)
        {
            string p = Path.Combine(folder, "users.dat");
            if (!File.Exists(p)) return false; 
            Users = User.ReadUsers(p);
            p = Path.Combine(folder, "movies.dat");
            if (!File.Exists(p)) return false;
            Movies = Movie.ReadMovies(p);
            p = Path.Combine(folder, "ratings.dat");
            if (!File.Exists(p)) return false;
            RatingCount = User.ReadRatings(Users, Movies, p);
            return true;
        }

    }
}
