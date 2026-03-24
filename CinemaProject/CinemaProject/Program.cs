using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace CinemaProject
{
    internal class Program
    {
        //User file paths and lists to load into
        static string baseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        static string MANAGER_FILE = Path.Combine(baseDirectory, "SaveData", "managers.txt");
        static string USER_FILE = Path.Combine(baseDirectory, "SaveData", "users.txt");

        static User CurrentUser;
        static Cinema CurrentCinema;

        static void Main(string[] args)
        {
            if (!Login())
            {
                Console.WriteLine("Too many failed login attempts. Exiting...");
                return;
            }

            CurrentCinema = new Cinema();

            Menu();
        }

        static void Menu()
        {
            // need to edit this and decide what the manager and employees see and how to differentiate between the two
            // hopefully all the functions of the system should be here in the menu now
            // not all of them may be needed - some could be implemented as part of each other
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---- WELCOME TO CINEMA MANAGEMENT SYSTEM ----");
                Console.WriteLine();
                Console.WriteLine("ENTER YOUR OPTIONS:");
                Console.WriteLine("1| Book Customer"); // this is definitely for employees
                Console.WriteLine("2| Seating Availability"); // the function for this needs to ask for the screen that is required
                Console.WriteLine("3| View Current Customers"); // needs to be done //////////////////////////////////////////////////////////////////////////////////////////////////////
                Console.WriteLine("4| Calculate Revenue per Screen"); // the function for this needs to ask for the screen that is required
                Console.WriteLine("5| Save Everything");
                if (CurrentUser.GetIsManager())
                {
                    Console.WriteLine();
                    Console.WriteLine("MANAGER'S OPTIONS:");
                    Console.WriteLine("+| Calculate Total Revenue"); // needs to collate at the total revenue for all screens
                    Console.WriteLine("#| Edit Film Schedule"); // managers
                    Console.WriteLine("@| Add User");
                }
                Console.WriteLine("X| Exit");
                Console.Write("Choice: ");

                string choice = Console.ReadLine();
                if (choice == null)
                {
                    Console.WriteLine("Input cannot be null.");
                    return;
                }
                if (choice == "x")
                {
                    choice = "X";
                }

                switch(choice)
                {
                    case "+":
                        if (CurrentUser.GetIsManager())
                        {
                            Console.WriteLine($"Current total profit is {CurrentCinema.CalculateAllProfit()}");
                        }
                        else Console.WriteLine("Invalid input");
                        break; // calculate total revenue

                    case "#":
                        if (CurrentUser.GetIsManager())
                        {
                            Console.WriteLine("Add or remove a film? (a/r)");
                            switch (Console.ReadLine())
                            {
                                case "a":
                                    Console.WriteLine("Enter film name: ");
                                    CurrentCinema.AddFilm(Console.ReadLine());
                                    break;
                                case "r":
                                    Console.WriteLine("Enter film name: ");
                                    if (CurrentCinema.RemoveFilm(Console.ReadLine()))
                                    {
                                        Console.WriteLine("Film removed");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error: Film not found");
                                    }
                                    break;
                            }
                        }
                        else Console.WriteLine("Invalid input");
                        break; // edit film schedule

                    case "@":
                        AddUser();
                        break; // add user

                    case "1":
                        // clears the console so it looks neater before the inputs
                        Console.Clear();

                        // titles the page so the user knows what they are doing
                        Console.WriteLine("BOOKING CUSTOMER:");

                        // validation for the screen integer
                        int screen = -1;
                        bool runningScreen = true;
                        while (runningScreen)
                        {
                            Console.Write("Enter the screen number: ");
                            string input = Console.ReadLine();
                            if (input == null || !int.TryParse(input, out screen) || screen < 0 || screen > 5)
                            {
                                Console.WriteLine("Enter a valid integer.");
                                continue;
                            }
                            int.TryParse(input, out screen);
                            runningScreen = false;
                        }

                        // validation for the name string
                        string name = "";
                        bool runningName = true;
                        while (runningName)
                        {
                            Console.Write("Enter the customer name: ");
                            name = Console.ReadLine();
                            if (name.Length == 0)
                            {
                                Console.WriteLine("Name cannot be null.");
                                continue;
                            }
                            runningName = false;
                        }

                        // validation for the seat integer
                        int seat = -1;
                        bool runningSeat = true;
                        Screen seatScreen = CurrentCinema.GetScreen(screen);
                        while (runningSeat)
                        {
                            Console.Write("Enter the seat number: ");
                            string input = Console.ReadLine();
                            if (input == null || !int.TryParse(input, out seat) || seat < 0 || seat >= (seatScreen.GetRows() * seatScreen.GetColumns()))
                            {
                                Console.WriteLine("Enter a valid integer.");
                                continue;
                            }
                            int.TryParse(input, out seat);
                            runningSeat = false;
                        }

                        // validation for the OAP bool
                        bool OAP = false;
                        bool runningOAP = true;
                        while (runningOAP)
                        {
                            Console.Write("Does the customer qualify for OAP? (Y/N): ");
                            string input = Console.ReadLine().ToLower();
                            if (input == null)
                            {
                                Console.WriteLine("Input cannot be null.");
                                continue;
                            }
                            if (input != "y" && input != "n")
                            {
                                Console.WriteLine("Input invalid - enter Y or N");
                                continue;
                            }
                            else if (input == "y")
                            {
                                OAP = true;
                                runningOAP = false;
                            }
                            else if (input == "n")
                            {
                                OAP = false;
                                runningOAP = false;
                            }
                        }

                        // validation for the VIP boolean
                        bool VIP = false;
                        bool runningVIP = true;
                        while (runningVIP)
                        {
                            Console.Write("Does the customer qualify for VIP? (Y/N): ");
                            string input = Console.ReadLine().ToLower();
                            if (input == null)
                            {
                                Console.WriteLine("Input cannot be null.");
                                continue;
                            }
                            if (input != "y" && input != "n")
                            {
                                Console.WriteLine("Input invalid - enter Y or N");
                                continue;
                            }
                            else if (input == "y")
                            {
                                VIP = true;
                                runningVIP = false;
                            }
                            else if (input == "n")
                            {
                                VIP = false;
                                runningVIP = false;
                            }
                        }

                        CurrentCinema.AddCustomer(screen, name, seat, OAP, VIP);
                        // int screen, string name, int seat, bool OAP, bool VIP

                        break; // book customer

                    case "2":
                        try
                        {
                            Console.WriteLine("Enter the screen you would like to see availability for: ");
                            int screen1 = Convert.ToInt32(Console.ReadLine());
                            CurrentCinema.DisplayScreen(screen1);
                        }
                        catch
                        {
                            Console.WriteLine("Error - Invalid input");
                        }
                        break; // seating availability

                    case "3":

                        break; // view customers that are currently booked

                    case "4":
                        Console.WriteLine("Enter screen number:");
                        try
                        {
                            int screenNum = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine($"The total revenue for screen number {screenNum} is £{CurrentCinema.CalculateScreenProfit(screenNum)}");
                        } 
                        catch
                        {
                            Console.WriteLine("Invalid input");
                        }
                        break; // calculate revenue per screen

                    case "5":
                        Save();
                        break; // save everything

                    case "X": 
                        Environment.Exit(0); 
                        break; // exit

                    default: 
                        Console.WriteLine("Invalid input");
                        break; // default error catch
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static bool Login()
        { 
            bool LoggedIn = false;
            bool ManagerLogin = false;
            string loginFile;

            //Loop to ensure that correct input is given for user or manager login
            bool valid = false;
            while (!valid)
            {
                Console.Write("Log in as user or manager? (u/m): ");
                string userChoice = Console.ReadLine();

                if (userChoice == "u")
                {
                    ManagerLogin = false;
                    valid = true;
                }
                else if (userChoice == "m")
                {
                    ManagerLogin = true;
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            loginFile = ManagerLogin ? MANAGER_FILE : USER_FILE; //One liner to decide which file to use

            //Username + password process
            int count = 0;

            while(!LoggedIn && count < 3)
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();
                string hashedPassword = SquareHash(password);
                if (File.Exists(loginFile))
                {
                    using (StreamReader sr = new StreamReader(loginFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 2 && parts[0] == username && parts[1] == hashedPassword)
                            {
                                LoggedIn = true;
                                CurrentUser = new User();
                                CurrentUser.SetUserName(username);
                                CurrentUser.SetIsManager(ManagerLogin);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("login file not found.");
                }
                if (!LoggedIn)
                {
                    count++;
                    Console.WriteLine($"Invalid credentials. {3 - count} attempts remaining.");
                }
            }

            return LoggedIn;
        }

        static string SquareHash(string s)
        {
            //Secure hashing function
            byte[] data = Encoding.UTF8.GetBytes(s);
            byte[] paddedData = new byte[((data.Length + 3) / 4) * 4];
            Array.Copy(data, paddedData, data.Length);

            BigInteger finalHash = 0;

            for (int i = 0; i < data.Length; i += 4)
            {
                BigInteger chunk = new BigInteger(paddedData.Skip(i).Take(4).ToArray());

                finalHash ^= (BigInteger.Pow(chunk, 8) << (i / 4)); //Left shift means there is no cancelling from xor
            }

            return finalHash.ToString("X64");
        }

        static void AddUser()
        {
            Console.Write("Enter the username: ");
            string userName = Console.ReadLine();
            Console.Write("Enter the password: ");
            string input1 = Console.ReadLine();
            Console.Write("Confirm password: ");
            string input2 = Console.ReadLine();

            string password;

            if (input1 == input2)
            {
                password = input2;
            }
            else
            {
                Console.WriteLine("Password in-valid");
                Console.WriteLine("Enter...");
                Console.ReadLine();
                return;
            }

            string hashPass = SquareHash(password);

            using (StreamWriter sr = new StreamWriter(USER_FILE, true))
            {
                sr.WriteLine($"{userName},{hashPass}");
            }
        }

        static void Save()
        {
            CurrentCinema.SaveCinema();
            //No need to save users as it already does when adding a new one
        }
    }
}
