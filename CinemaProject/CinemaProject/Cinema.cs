using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace CinemaProject
{
    internal class Cinema
    {
        List<string> Films = new List<string>();
        List<Screen> Screens;
        List<Customer> Customers;
        private string FilmsFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "SaveData", "Films.txt");
        private int NumScreens = 5;

        public Cinema()
        {
            // immediately loads the cinema from file when the cinema object is instantiated
            LoadCinema();
            Films = new List<string>();
            Customers = new List<Customer>();
        }

        public void LoadCinema()
        {
            //Load films into the film list
            Films = new List<string>();
            if (File.Exists(FilmsFilePath))
            {
                using (StreamReader sr = new StreamReader(FilmsFilePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        AddFilm(line);
                    }
                }
            }

            //Loading screens into the screen array
            Screens = new List<Screen>(NumScreens);
            for (int i = 0; i < NumScreens; i++)
            {
                Screens.Add(new Screen(i + 1));
                Screens[i].LoadScreen();
            }
        }

        public void SaveCinema()
        {
            foreach (Screen screen in Screens)
            {
                screen.SaveScreen();
            }

            using (StreamWriter sr = new StreamWriter(FilmsFilePath, false))
            {
                foreach (string f in Films)
                {
                    sr.WriteLine(f);
                }
            }
        }

        public List<Customer> GetCustomers()
        {
            foreach (Screen screen in Screens)
            {
                foreach (Customer c in screen.GetCustomers())
                {
                    Customers.Add(c);
                }
            }
            return Customers;
        }
        

        /*
         * FILM MANAGEMENT
         */

        public void AddFilm(string film)
        {
            //Adding a film to the list in alphabetical order
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

        public bool RemoveFilm(string film) //bool so the menu knows if the function worked
        {
            //Remove a film from the list using the FindFilm function
            int index = FindFilm(film);

            if (index == -1) return false;

            Films.RemoveAt(index);
            return true;
        }

        public string GetNextFilm(int screenNum)
        {
            Screen screen = Screens[screenNum];
            return screen.GetFilm();
        }

        // set next film
        public int SetNextFilm(int screen, string name)
        {
            Screen screen = Screens[screen];
            int result = screen.SetFilm(name);
            return result;
        }

        public int FindFilm(string film)
        {
            // calls a binary search function to find the index of the film in the list
            return _BinarySearch(Films, film, 0, Films.Count);
        }

        private int _BinarySearch(List<string> data, string value, int left, int right)
        {
            if (left > right)
            {
                return -1;
            }

            int mid = (left + right) / 2;

            // binary search to find the index of the film in the list
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

        public decimal CalculateAllProfit()
        {
            decimal profit = 0;
            foreach(var screen in Screens)
            {
                profit += screen.CalcScreenRevenue();
            }
            return profit;
        }

        public decimal CalculateScreenProfit(int screen)
        {
            return Screens[screen-1].CalcScreenRevenue();
        }

        public void AddCustomer(int screen, string name, int seat, bool OAP, bool VIP)
        {
            Screens[screen - 1].AddCustomer(name, seat, OAP, VIP);
        }

        public void DisplayScreen(int screen)
        {
            Screens[screen - 1].DisplayScreen();
        }

        public Screen GetScreen(int screen)
        {
            return Screens[screen];
        }
    }
}
