using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    internal class Customer
    {
        // this class is pretty much done

        //Attributes
        private int Seat;
        private string Name;
        private bool VIP;
        private bool OAP;

        //Setters
        public void SetSeat(int seat)
        {
            Seat = seat;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetVIP(bool vip)
        {
            VIP = vip;
        }

        public void SetOAP(bool oap)
        {
            OAP = oap;
        }


        //Getters
        public int GetSeat()
        {
            return Seat;
        }

        public string GetName()
        {
            return Name;
        }

        public bool GetVIP()
        {
            return VIP;
        }

        public bool GetOAP()
        {
            return OAP;
        }
    }
}
