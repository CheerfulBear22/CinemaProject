using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class Screen
    {
        // this class is pretty much done
        // needs to have the loading of the screen done
        private string FileName;
        private string CustomerFileName;
        private int ROW = 10;
        private int COL = 25;
        // attributes for this class
        private char[,] Seats;
        private int ScreenNumber;
        private string NextFilm;
        private List<Customer> Customers;
        private List<string> CustomerNames; //Can then search this to get the index in the main list

        // constuctor
        public Screen(int sn)
        {
            // create the Customers list when the screen object is instantiated
            Customers = new List<Customer>();
            CustomerNames = new List<string>();
            ScreenNumber = sn;
            FileName = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "SaveData", $"screen{ScreenNumber}.txt");
            CustomerFileName = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "SaveData", $"screen{ScreenNumber}Customers.txt");
        }

        public void LoadScreen()
        {
            // loading file for the screen with check to see if the file exists
            if (File.Exists(FileName))
            {
                using (StreamReader sr = new StreamReader(FileName))
                {
                    NextFilm = sr.ReadLine();
                    ROW = Convert.ToInt32(sr.ReadLine());
                    COL = Convert.ToInt32(sr.ReadLine());

                    Seats = new char[ROW, COL];

                    for (int i = 0; i < ROW; i++)
                    {
                        for (int j = 0; j < COL; j++)
                        {
                            Seats[i, j] = (char)sr.Read();
                        }
                        sr.ReadLine();
                    }
                }
            }
            else
            {
                Seats = new char[ROW, COL];
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COL; j++)
                    {
                        Seats[i, j] = '-';
                    }
                }
            }

            if (File.Exists(CustomerFileName))
            {
                using (StreamReader sr = new StreamReader(CustomerFileName))
                {
                    while (!sr.EndOfStream)
                    {
                        Customer c = new Customer();
                        string[] line = sr.ReadLine().Split(',');
                        c.SetName(line[0]);
                        c.SetSeat(Convert.ToInt32(line[1]));
                        c.SetOAP(line[2] == "True");
                        c.SetVIP(line[3] == "True");
                        CustomerNames.Add(line[0]);
                        Customers.Add(c);
                    }
                }
            }
        }

        public void SaveScreen()
        {
            // saving the data to the file for the screen
            using (StreamWriter sw = new StreamWriter(FileName,false))
            {
                sw.Write($"{NextFilm}\n{ROW}\n{COL}");

                for (int i = 0; i < ROW; i++)
                {
                    // new line at the end of each row
                    sw.WriteLine();

                    for (int j = 0; j < COL; j++)
                    {
                        // character '-' is used for an empty seat and if the seat is booked then it can be 'X'
                        sw.Write($"{Seats[i, j]}");
                    }
                }

            }

            using (StreamWriter sw = new StreamWriter(CustomerFileName, false))
            {
                foreach (Customer c in Customers)
                {
                    sw.WriteLine($"{c.GetName()},{c.GetSeat()},{c.GetOAP()},{c.GetVIP()}");
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

        public int SetFilm(string film)
        {
            // setter for the next film
            try
            {
                NextFilm = film;
                return 1;
            }
            catch
            {
                return 0;
            }
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

        public void AddCustomer(string name, int seat, bool OAP, bool VIP)
        {
            int row = seat / ROW;
            int col = seat % ROW;

            Seats[row, col] = 'X';

            Customer c = new Customer();
            c.SetName(name);
            c.SetSeat(seat);
            c.SetVIP(VIP);
            c.SetOAP(OAP);

            if (Customers.Count == 0)
            {
                Customers.Add(c);
                return;
            }
            for (int i = 0; i < Customers.Count; i++)
            {
                if (String.Compare(c.GetName(), Customers[i].GetName()) < 1)
                {
                    Customers.Insert(i, c);
                    return;
                }
            }

            Customers.Add(c);
        }

        public void DisplayScreen()
        {
            for (int r = 0; r < ROW; r++)
            {
                for (int c = 0; c < COL; c++)
                {
                    Console.Write(Seats[r,c]);
                }
                Console.WriteLine();
            }
        }

        public List<Customer> GetCustomers()
        {
            return Customers;
        }

        public int GetRows()
        {
            return ROW;
        }

        public int GetColumns() 
        { 
            return COL; 
        }
    }
}
