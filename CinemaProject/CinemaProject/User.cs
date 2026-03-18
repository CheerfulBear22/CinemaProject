using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class User
    {
        //Initialising

        public bool IsManager;
        public string UserName;
        
        //Getters

        public bool GetIsManager()
        {
            return IsManager;
        }

        public string GetUserName()
        {
            return UserName;
        }

        //Setters
        public void SetIsManager(bool isManager)
        {
            IsManager = isManager;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
        }

        //Functions

        public void ChangeIsManager(bool newStatus)
        {
            if (newStatus == true)
            {
                IsManager = true;
            }
            else if (newStatus == false)
            {
                IsManager = false;
            }
        }

        public int AccessManager()
        {
            if (IsManager == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
