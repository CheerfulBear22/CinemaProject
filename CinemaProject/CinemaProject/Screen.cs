using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class Screen
    {
        private string FileName = "Screen"; // this needs to change for each screen but idk how to do that
        // otherwise each screen file will be called the same thing
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
            // loading file for the screen with check to see if the file exists
            if (File.Exists(FileName))
            {
                using (StreamReader sr = new StreamReader(FileName))
                {
                    // dont know how to do this
                    // will use ai to help another time
                }
            }
        }

        public void SaveScreen()
        {
            // saving the data to the file for the screen
            using (StreamWriter sw = new StreamWriter(FileName,false))
            {
                Console.WriteLine($"(Room number)| {ROW} x {COL}");

                for (int i = 0; i < ROW; i++)
                {
                    // new line at the end of each row
                    Console.WriteLine();

                    for (int j = 0; j < COL; j++)
                    {
                        // character '-' is used for an empty seat and if the seat is booked then it can be 'X'
                        sw.WriteLine($"{Seats[i, j]}\t");
                    }
                }

            }
        }

        public void SetSeat()
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
            int VIP = 0;
            int OAP = 0;
            int count = 0;
            // have a base price that can be increased (VIP)
            // or decreased (OAP) in order then count the number of customers in the list
            decimal basePrice = 20.00m;
            decimal totalProfit = 0;

            foreach (Customer c in Customers)
            {
                // check for VIP and OAP for each customer in list
                if (c.GetOAP() || c.GetVIP())
                {
                    if (c.GetOAP())
                    {
                        // increment a counter for the number of each
                        OAP++;
                        count++;
                    }
                    if (c.GetVIP())
                    {
                        VIP++;
                        count++;
                    }
                    if (c.GetVIP() && c.GetOAP())
                    {
                        OAP--;
                        VIP--;
                        count++;
                    }
                }
            }

            // to calculate the revenue
            decimal VIPtot = VIP * (basePrice * (decimal)1.2);
            decimal OAPtot = OAP * (basePrice * (decimal)0.8);

            totalProfit = VIPtot + OAPtot;

            // returns a decimal revenue for the screen
            // rounded to 2dp
            return Math.Round(totalProfit,2);
        }
    }
}
