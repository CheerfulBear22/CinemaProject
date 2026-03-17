using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class User
    {
        public bool IsManager;
        public string UserName;
        
        public bool GetIsManager(bool status)
        {
            IsManager = status
        }

        public string GetUserName(string user)
        {
            UserName = user;
        }

        public void ChangeIsManager(bool newStatus)
        {
            if (newStatus == 1)
            {
                IsManager = true;
            }
            else if (newStatus == 0)
            {
                IsManager = false;
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
