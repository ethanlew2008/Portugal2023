using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Essentials;


namespace Portugal_2023
{
    
    public class timeVM : BindableBase
    {
        public timeVM()
        {
            // Start the time updater thread when the ViewModel is created
            StartUpdaterThread();
            FlightUpdateThread();
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

        public string Fltm
        {
            get { return fltm; }
            set
            {
                if (fltm != value)
                {
                    fltm = value;
                    OnPropertyChange("Fltm");
                }
            }
        }
        private static string fltm;

        public string Airmd
        {
            get { return airmd; }
            set
            {
                if (airmd != value)
                {
                    airmd = value;
                    OnPropertyChange("Airmd");
                }
            }
        }
        private static string airmd;

        public string Aircg
        {
            get { return aircg; }
            set
            {
                if (aircg != value)
                {
                    aircg = value;
                    OnPropertyChange("Aircg");
                }
            }
        }
        private static string aircg;

        public string Bckclr
        {
            get { return bckclr; }
            set
            {
                if (bckclr != value)
                {
                    bckclr = value;
                    OnPropertyChange("Bckclr");
                }
            }
        }
        private static string bckclr;

        public string Cbn
        {
            get { return cbn; }
            set
            {
                if (cbn != value)
                {
                    cbn = value;
                    OnPropertyChange("Cbn");
                }
            }
        }
        private static string cbn;

        public string Day
        {
            get { return day; }
            set
            {
                if (day!= value)
                {
                    day = value;
                    OnPropertyChange("Day");
                }
            }
        }
        private static string day;

        public string Month
        {
            get { return month; }
            set
            {
                if (month != value)
                {
                    month = value;
                    OnPropertyChange("Month");
                }
            }
        }
        private static string month;

        public string Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    OnPropertyChange("Data");
                }
            }
        }
        private static string data;

        public string Sleeptime
        {
            get { return sleeptime; }
            set
            {
                if (sleeptime != value)
                {
                    sleeptime = value;
                    OnPropertyChange("Sleeptime");
                }
            }
        }
        private static string sleeptime;

        public string Pecsleep
        {
            get { return pecsleep; }
            set
            {
                if (pecsleep != value)
                {
                    pecsleep = value;
                    OnPropertyChange("Pecsleep");
                }
            }
        }
        private static string pecsleep;

        public string Batt
        {
            get { return batt; }
            set
            {
                if (batt != value)
                {
                    batt = value;
                    OnPropertyChange("Batt");
                }
            }
        }
        private static string batt;

        public string Cals
        {
            get { return batt; }
            set
            {
                if (cals != value)
                {
                    cals = value;
                    OnPropertyChange("Cals");
                }
            }
        }
        private static string cals;

        public string Breath
        {
            get { return breath; }
            set
            {
                if (breath != value)
                {
                    breath = value;
                    OnPropertyChange("Breath");
                }
            }
        }
        private static string breath;


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
                    if ((AppInfo.RequestedTheme == AppTheme.Dark)) { Bckclr = "DarkRed"; } else { Bckclr = "White"; }
                    Day = DateTime.Now.DayOfWeek.ToString();
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi)) { Data = "Data On"; } else { Data = "Data Off"; }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
        public void FlightUpdateThread()
        {
            // Start a new thread that updates the Time property every second
            new Thread(() =>
            {
                while (true)
                {
                    if (PortugalTab.flighttimer.IsRunning)
                    {
                        Pcta = Convert.ToInt32(100 - (PortugalTab.flighttimer.Elapsed.TotalMinutes / 85) * 100) + "% left";
                        Fltm = Convert.ToString(Convert.ToInt32(85 - PortugalTab.flighttimer.Elapsed.TotalMinutes)) + " Mins";
                        if (Connectivity.NetworkAccess == NetworkAccess.None) { Airmd = "Airmode: On"; } else { Airmd = "Airmode: Off"; }
                        Aircg = Battery.State.ToString();
                        Cbn= Convert.ToString(Math.Round(PortugalTab.flighttimer.Elapsed.TotalMinutes * 5.529, 1)) + "KG CO2";
                        

                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }

        public void SleepUpdateThread()
        {
            // Start a new thread that updates the Time property every second
            new Thread(() =>
            {
                
                while (true)
                {
                    if (PortugalTab.sleeptimer.IsRunning)
                    {
                        Sleeptime = string.Format("{0}:{1:00}", (int)(TimeSpan.FromMinutes(PortugalTab.sleeptimer.Elapsed.TotalMinutes)).TotalHours, (TimeSpan.FromMinutes(PortugalTab.sleeptimer.Elapsed.TotalMinutes)).Minutes);
                        Pecsleep = Math.Round((PortugalTab.sleeptimer.Elapsed.TotalMinutes / 540) * 100, 0) + "%";
                        Breath = "Breaths: " + Math.Round(15 * PortugalTab.sleeptimer.Elapsed.TotalMinutes, 0);
                        Cals = Math.Round(0.77 * PortugalTab.sleeptimer.Elapsed.TotalMinutes, 0) + " Kcal";
                        Batt = "Battery: " + (Battery.ChargeLevel * 100) + "%";
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
