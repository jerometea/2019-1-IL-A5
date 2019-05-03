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

        public double Distance( User u1, User u2 )
        {
            bool atLeastOne = false;
            int sum2 = 0;
            foreach( var movieR1 in u1.Ratings )
            {
                if( u2.Ratings.TryGetValue( movieR1.Key, out var r2 ) )
                {
                    atLeastOne = true;
                    sum2 += (movieR1.Value - r2) ^ 2;
                }
            }
            return atLeastOne ? Math.Sqrt( sum2 ) : double.PositiveInfinity;
        }

        public double Similarity( User u1, User u2 ) => 1.0 / (1.0 + Distance( u1, u2 ));

        public double SimilarityPearson( User u1, User u2 )
        {
            List<(int, int)> ratings = new List<(int, int)>();

            foreach( var movieR1 in u1.Ratings )
            {
                if( u2.Ratings.TryGetValue( movieR1.Key, out var r2 ) )
                {
                    ratings.Add( (movieR1.Value, r2) );
                }
            }

            return SimilarityPearson( ratings );
        }

        public double SimilarityPearson( IEnumerable<(int x, int y)> values )
        {
            int n = values.Count();

            int Zxy = 0;

            int Zx = 0;

            int Zy = 0;

            foreach( (int x, int y) rates in values )
            {
                Zxy += rates.x * rates.y;
                Zx += rates.x;
                Zy += rates.y;
            }

            double tempUp        =            (n * Zxy )   - (Zx * Zy);
            double tempDownLeft  = Math.Sqrt( (n * Zx ^ 2) - (Zx ^ 2) ) ;
            double tempDownRight = Math.Sqrt( (n * Zy ^ 2) - (Zy ^ 2) );

            double r = tempUp / (tempDownLeft * tempDownRight);

            return r;
        }

        public bool LoadFrom( string folder )
        {
            string p = Path.Combine( folder, "users.dat" );
            if( !File.Exists( p ) ) return false;
            Users = User.ReadUsers( p );
            p = Path.Combine( folder, "movies.dat" );
            if( !File.Exists( p ) ) return false;
            Movies = Movie.ReadMovies( p );
            p = Path.Combine( folder, "ratings.dat" );
            if( !File.Exists( p ) ) return false;
            RatingCount = User.ReadRatings( Users, Movies, p );
            return true;
        }

    }
}
