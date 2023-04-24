using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Portugal_2023
{
    
    public class timeVM : BindableBase
    {
        public timeVM()
        {
            // Start the time updater thread when the ViewModel is created
            StartUpdaterThread();
            FlightUpdateBind();
        }


        public string Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChange("Time");
                }
            }
        }
        private static string time;

        public string TimeL
        {
            get { return timel; }
            set
            {
                if (timel != value)
                {
                    timel = value;
                    OnPropertyChange("TimeL");
                }
            }
        }
        private static string timel;

        public string LOC
        {
            get { return loc; }
            set
            {
                if (loc != value)
                {
                    loc = value;
                    OnPropertyChange("LOC");
                }
            }
        }
        private static string loc;

        public string Pcta
        {
            get { return pcta; }
            set
            {
                if (pcta != value)
                {
                    pcta = value;
                    OnPropertyChange("Pcta");
                }
            }
        }
        private static string pcta;


        private void StartUpdaterThread()
        {
            // Start a new thread that updates the Time property every second
            new Thread(() =>
            {
                while (true)
                {
                    if (DateTime.UtcNow.Month > 10 || DateTime.UtcNow.Month < 3) { Time = "London: " + DateTime.UtcNow.ToString("HH:mm"); } else { Time = "London: " + DateTime.UtcNow.AddHours(1).ToString("HH:mm"); }
                    if (DateTime.UtcNow.Month > 10 || DateTime.UtcNow.Month < 3) { TimeL = "Lisbon: " + DateTime.UtcNow.ToString("HH:mm"); } else { TimeL = "Lisbon: " + DateTime.UtcNow.AddHours(1).ToString("HH:mm"); }
                    LOC = "LOC: " + DateTime.Now.ToString("HH:mm");
                    Thread.Sleep(1000);
                }
            }).Start();
        }
        public void FlightUpdateBind()
        {
            // Start a new thread that updates the Time property every second
            new Thread(() =>
            {
                while (true)
                {
                    if(PortugalTab.flighttimer.IsRunning) { Pcta= "" + Convert.ToInt32(100 - (PortugalTab.flighttimer.Elapsed.TotalMinutes / 147) * 100) + "% left"; }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
