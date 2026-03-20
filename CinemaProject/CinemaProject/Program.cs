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

        static void Main(string[] args)
        {
            if (!Login())
            {
                Console.WriteLine("Too many failed login attempts. Exiting...");
                return;
            }

            Menu();
        }

        static void Menu()
        {
            // need to edit this and decide what the manager and employees see and how to differentiate between the two
            // hopefully all the functions of the system should be here in the menu now
            // not all of them may be needed - some could be implemented as part of each other
            Console.Clear();
            Console.WriteLine("---- WELCOME TO CINEMA MANAGEMENT SYSTEM ----");
            Console.WriteLine();
            Console.WriteLine("ENTER YOUR OPTIONS");
            Console.WriteLine("1| Book Customer"); // this is definitely for employees
            Console.WriteLine("2| Seating Availability"); // the function for this needs to ask for the screen that is required
            Console.WriteLine("3| Calculate Revenue per Screen"); // the function for this needs to ask for the screen that is required
            Console.WriteLine("4| Calculate Total Revenue"); // needs to collate at the total revenue for all screens
            Console.WriteLine("5| Edit Film Schedule"); // managers
            Console.WriteLine("6| Save Everything");
            Console.WriteLine("7| Exit");
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
                Console.WriteLine("Log in as user or manager? (u/m)");
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
                Console.WriteLine("Enter username:");
                string username = Console.ReadLine();
                Console.WriteLine("Enter password:");
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
                                CurrentUser.UserName = username;
                                CurrentUser.IsManager = ManagerLogin;
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


        static void AddUser(User newUser)
        {
            // probably adds the passed user to a list of users
            // or saves them to a file
            // or loads them to a file
            // for David to do

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
            // saves the user credentials to the file if it is a new user
            // or just not do anything if the user is already saved
            // this means that Main will not call it if therer are no users to add
        }
    }
}
