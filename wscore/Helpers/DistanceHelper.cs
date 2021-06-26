using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Helpers
{
    public class DistanceHelper
    {
        public DistanceHelper()
        {
        }

        public double Distance(
            string firstlat, string firstlon, 
            string secondlat, string secondlon)
        {
            double t1, n1, t2, n2, dlat, dlon, a, c, dm, dk, mi, km;
            var Rm = 3961; // mean radius of the earth (miles) at 39 degrees from the equator
            var Rk = 6373; // mean radius of the earth (km) at 39 degrees from the equator
                           // get values for lat1, lon1, lat2, and lon2
            t1 = Convert.ToDouble(firstlat);
            n1 = Convert.ToDouble(firstlon);
            t2 = Convert.ToDouble(secondlat);
            n2 = Convert.ToDouble(secondlon);

            // convert coordinates to radians
            double lat1 = deg2rad(t1);
            double lon1 = deg2rad(n1);
            double lat2 = deg2rad(t2);
            double lon2 = deg2rad(n2);

            // find the differences between the coordinates
            dlat = lat2 - lat1;
            dlon = lon2 - lon1;

            // here's the heavy lifting
            a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
            c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); // great circle distance in radians
            dm = c * Rm; // great circle distance in miles
            dk = c * Rk; // great circle distance in km

            // round the results down to the nearest 1/1000
            mi = round(dm);
            km = round(dk);

            // display the result
            return km;
        }

        private double deg2rad(double deg)
        {
            double rad = deg * Math.PI / 180; // radians = degrees * pi/180
            return rad;
        }

        private double round(double x)
        {
            return Math.Round(x * 1000) / 1000;
        }
    }
}
