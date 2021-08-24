using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Extension
{
   public static class RepositoryMovieExtension
    {
        public static IEnumerable<Movie> Search(this IEnumerable<Movie> movies ,string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName)) return movies;
            var lowerCaseSearch = searchName.Trim().ToLower();
           return  movies.Where(x => x.Name.ToLower().Contains(lowerCaseSearch));
            
        }
        public static IEnumerable<Movie> Filter(this IEnumerable<Movie> movies, string minDate  , string maxDate)

        {
            if (string.IsNullOrWhiteSpace(minDate) || string.IsNullOrWhiteSpace(maxDate)) return movies;
             if( DateTime.Parse(minDate).Date.CompareTo(DateTime.Parse(maxDate).Date) > 0)
            {
                throw new ArgumentException($"{nameof(maxDate)} less than {nameof(minDate)}");
            }
           return movies.Where(x => x.PublishedYear.Date.CompareTo(DateTime.Parse(minDate).Date) >0);
        }


    }
}
