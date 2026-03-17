using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class Cinema
    {
        List<string> Films;
        List<Screen> Screens;

        public Cinema()
        {
            LoadCinema();
            Films = new List<string>();
        }

        public void LoadCinema()
        {
            //Loading screens into the screen array
        }

        public void AddFilm(string film)
        {
            //Adding a film
            if (Films.Count == 0) Films.Add(film);
        }
    }
}
