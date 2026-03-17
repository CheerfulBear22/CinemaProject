using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class Screen
    {
        private const int ROW = 10;
        private const int COL = 25;
        // attributes for this class
        private char[,] Seats;
        private int ScreenNumber;
        private string NextFilm;
        private List<Customer> Customers;

        // constuctor
        public Screen(int sn, string nf)
        {
            // create the Customers list when the screen object is instantiated
            Customers = new List<Customer>();

            ScreenNumber = sn;
            NextFilm = nf;
        }

        public void LoadScreen()
        {
            // loading file for the screen
        }

        public void SaveScreen()
        {
            // saving data for screen to file
        }

        public void SetSeat(char[,] newSeats)
        {
            // fills the character array of seats
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    // character '-' is used for an empty seat and if the seat is booked then it can be 'X'
                    Seats[i, j] = '-';
                }
            }
        }

        public int GetScreenNumber()
        {
            return ScreenNumber;
        }

        public void SetFilm(string film)
        {
            // setter for the next film
            NextFilm = film;
        }

        public string GetFilm()
        {
            // getter for the next film
            return NextFilm;
        }

        public decimal CalcScreenRevenue()
        {
            // check for VIP and OAP for each customer in list
            // increment a counter for the number of each
            // then have a base price that can be increased (VIP)
            // or decreased (OAP) in order then count the number of customers in the list
            // to calculate the revenue
            // returns a decimal price for the screen
            // rounded to 2dp
        }
    }
}
