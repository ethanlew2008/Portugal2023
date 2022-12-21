using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Battery = Xamarin.Essentials.Battery;
using static Xamarin.Essentials.Permissions;
using System.Globalization;

namespace Portugal_2023
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortugalTab : TabbedPage
    {
        Stopwatch flighttimer = new Stopwatch();
        Stopwatch sleeptimer = new Stopwatch();
        APIClient clnt = new APIClient();
        public PortugalTab()
        {
            InitializeComponent();
            if ((AppInfo.RequestedTheme == AppTheme.Dark)) { BackgroundColor = Color.DarkRed; } else { BackgroundColor = Color.White; }
            CurrentPage = Children[1];
            clnt.GetGBP();
            UniversialUpdate();
            AppVer.Text = Xamarin.Essentials.VersionTracking.CurrentVersion;
        }

        public void UniversialUpdate()
        {
            string time = "";
            if (DateTime.UtcNow.Month > 10 || DateTime.UtcNow.Month < 3) { time = DateTime.UtcNow.ToString("HH:mm"); } else { time = DateTime.UtcNow.AddHours(1).ToString("HH:mm"); }
            UKTimeHome.Text = "London: " + time; PORTimeHome.Text = "Lisbon: " + time;

            BattLev.Text = "Battery: " + Convert.ToString(Battery.ChargeLevel * 100) + "%";
            Charging.Text = Convert.ToString(Battery.State);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi)) { DataOnHome.Text = "Data On"; } else { DataOnHome.Text = "Data Off"; }
            LOCTimeHome.Text = "LOC: " + DateTime.Now.ToString("HH:mm");

            DateHome.Text = DateTime.Now.DayOfWeek.ToString();
            MonthHome.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);

            if (flighttimer.IsRunning)
            {
                
                if (!flighttimer.IsRunning) { flighttimer.Start(); StartFlight.Source = "PortugalEndIcon"; }
                try { if (Convert.ToInt32(TimeLeft.Text) <= 0) { flighttimer.Reset(); return; } } catch (Exception) { }

                if (Connectivity.NetworkAccess == NetworkAccess.None) { Airmode.Text = "Airmode: On"; } else { Airmode.Text = "Airmode: Off"; }
                ChargingAir.Text = Battery.State.ToString();
                LocTime.Text = "LOC: " + DateTime.Now.ToString("HH:mm");
                UKTIME.Text = "London: " + time; PORTIME.Text = "Lisbon: " + time;
                PercentLeft.Text = "" + Convert.ToInt32(100 - (flighttimer.Elapsed.TotalMinutes / 147) * 100) + "% left";
                TimeLeft.Text = Convert.ToString(Convert.ToInt32(85 - flighttimer.Elapsed.TotalMinutes)) + " Mins";
                Co2.Text = Convert.ToString(Math.Round(flighttimer.Elapsed.TotalMinutes * 5.529, 1)) + "KG CO2";
            }

            if (sleeptimer.IsRunning)
            {
                UKTimeSleep.Text = "London: " + time; PORTimeSleep.Text = "Lisbon: " + time;

                TimeSlept.Text = string.Format("{0}:{1:00}", (int)(TimeSpan.FromMinutes(sleeptimer.Elapsed.TotalMinutes)).TotalHours, (TimeSpan.FromMinutes(sleeptimer.Elapsed.TotalMinutes)).Minutes);
                PercentOfSleep.Text = Math.Round((sleeptimer.Elapsed.TotalMinutes / 540) * 100, 0) + "%";

                DayOfWeek.Text = DateTime.Now.DayOfWeek.ToString();
                BattPerSleep.Text = "Battery: " + (Battery.ChargeLevel * 100) + "%";

                CaloriesBurnt.Text = Math.Round(0.77 * sleeptimer.Elapsed.TotalMinutes, 0) + " Kcal";
                BreathsTaken.Text = "Breaths: " + Math.Round(15 * sleeptimer.Elapsed.TotalMinutes,0);
            }


        }

        private void StartFlight_Clicked(object sender, EventArgs e) 
        {
            if (flighttimer.IsRunning) { flighttimer.Reset(); StartFlight.Source = "PortugalStartIcon"; return; }            
            else { flighttimer.Start(); StartFlight.Source = "PortugalEndIcon"; UniversialUpdate(); }                  
        }
        private void UpdateFlight_Clicked(object sender, EventArgs e) { if (flighttimer.IsRunning) { UniversialUpdate(); } }     
        private void StartSleep_Clicked(object sender, EventArgs e)
        {
            if (sleeptimer.IsRunning) { sleeptimer.Reset(); StartSleep.Source = "PortugalStartIcon"; return; }
            else { sleeptimer.Start(); StartSleep.Source = "PortugalEndIcon"; UniversialUpdate(); }         
        }
        private void UpdateSleep_Clicked(object sender, EventArgs e) { if (sleeptimer.IsRunning) { UniversialUpdate(); } }
        private void UpdteHome_Clicked(object sender, EventArgs e) { UniversialUpdate(); }       
        private void SOS_Clicked(object sender, EventArgs e) { try { PhoneDialer.Open("112"); } catch (Exception) { } } 
        private void CurrencyEUR_TextChanged(object sender, TextChangedEventArgs e) { try { CurrencyGBP.Text = "€" + Math.Round(Convert.ToDouble(CurrencyEUR.Text) / Convert.ToDouble(clnt.varsyr), 2); } catch (Exception) { } }            
    }
}