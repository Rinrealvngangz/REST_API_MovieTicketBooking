using MovieTicketBookingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
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
        public static IEnumerable<Movie> Sort(this IEnumerable<Movie> movies, string orderByQueryString)
        {
            
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return movies.OrderBy(x => x.Name);

            var orderByParams = orderByQueryString.Trim().ToLower().Split(',');

            var propInfos = typeof(Movie).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var param in orderByParams)
            {
                if (string.IsNullOrWhiteSpace(param)) continue;

                var tempOrder = param.StartsWith("-") ? param.TrimStart('-') : param ;
                var objectProperty = propInfos.FirstOrDefault(x => x.Name.ToLower().Equals(tempOrder));

                if (objectProperty is null) continue;

                if (param.StartsWith("-"))
                {

                    movies = movies.OrderByDescending(x => objectProperty.GetValue(x, null));
          
                }
                else
                {

                    movies = movies.OrderBy(x => objectProperty.GetValue(x, null));
                }
  
            }
            return movies;
         
        }


    }
}
