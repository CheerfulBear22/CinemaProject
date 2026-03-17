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
            //Load films into the film list
            Films = new List<string>();
            if (File.Exists("Films.txt"))
            {
                using (StreamReader sr = new StreamReader("Films.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        AddFilm(line);
                    }
                }
            }

            //Loading screens into the screen array
        }


        /*
         * FILM MANAGEMENT
         */
        public void AddFilm(string film)
        {
            //Adding a film
            if (Films.Count == 0)
            {
                Films.Add(film);
                return;
            }
            for (int i = 0; i < Films.Count; i++)
            {
                if (String.Compare(film, Films[i]) < 1)
                {
                    Films.Insert(i, film);
                    return;
                }
            }

            Films.Add(film);
        }

        public void RemoveFilm(string film)
        {
            //Remove a film from the list using the FindFilm function
            int index = FindFilm(film);

            if (index == -1) return;

            Films.RemoveAt(index);
        }

        static int FindFilm(string film)
        {
            return _BinarySearch(Films, film, 0, Films.Count);
        }

        static int _BinarySearch(List<string> data, string value, int left, int right)
        {
            if (left > right)
            {
                return -1;
            }

            int mid = (left + right) / 2;

            if (data[mid].CompareTo(value) < 0)
            {
                return _BinarySearch(data, value, mid + 1, right);
            }
            else if (data[mid].CompareTo(value) > 0)
            {
                return _BinarySearch(data, value, left, mid - 1);
            }
            else
            {
                return mid;
            }
        }
    }
}
