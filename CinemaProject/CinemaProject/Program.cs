using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User CurrentUser = new User();
            // needs a menu system for logging in
        }

        public void Menu()
        {
            Console.Clear();
            Console.WriteLine("---- WELCOME TO CINEMA MANAGEMENT SYSTEM ----");
            Console.WriteLine();
            Console.WriteLine("ENTER YOUR OPTIONS");
        }

        public bool Login()
        {
            // handles the login and returns true if the user
            // manages to sign in with correct credentials
            // within 'x' number of tries
        }

        public void AddUser(User newUser)
        {
            // probably adds the passed user to a list of users
            // or saves them to a file
            // or loads them to a file
            // for David to do
        }

        public void Save()
        {
            // saves the user credentials to the file if it is a new user
            // or just not do anything if the user is already saved
            // this means that Main will not call it if therer are no users to add
        }
    }
}
