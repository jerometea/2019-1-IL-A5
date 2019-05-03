using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Algo
{
    public partial class Movie
    {
        static public Dictionary<int, Movie> ReadMovies( string path, out Dictionary<int, List<Movie>> duplicates )
        {
            var result = new Dictionary<int, Movie>();
            duplicates = new Dictionary<int, List<Movie>>();
            using( TextReader r = File.OpenText( path ) )
            {
                string line;
                while( (line = r.ReadLine()) != null )
                {
                    Movie exists, u = new Movie( line );
                    if( result.TryGetValue( u.MovieId, out exists ) )
                    {
                        List<Movie> list;
                        if( !duplicates.TryGetValue( u.MovieId, out list ) )
                        {
                            list = new List<Movie>();
                            duplicates.Add( u.MovieId, list );
                        }
                        list.Add( u );
                    }
                    else result.Add( u.MovieId, u );
                }
            }
            return result;
        }
    }

}
