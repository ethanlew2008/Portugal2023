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
using LocalNotifications.Plugin;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace Portugal_2023
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortugalTab : TabbedPage
    {
        public static Stopwatch flighttimer = new Stopwatch();
        public static Stopwatch sleeptimer = new Stopwatch();
        APIClient clnt = new APIClient();
        
       
        bool popup = false;
        public PortugalTab()
        {
            InitializeComponent();
            
            CurrentPage = Children[1];
            clnt.GetGBP();            
            UniversialUpdate();          
            BindingContext = new timeVM();
            AppVer.Text = Xamarin.Essentials.VersionTracking.CurrentVersion;          
        }


       

        public async void UniversialUpdate()
        {

            if (clnt.intcarb == 0) { await clnt.GetWeather(); await clnt.GetWeatherLON(); };

            string time = "";
            if (DateTime.UtcNow.Month > 10 || DateTime.UtcNow.Month < 3) { time = DateTime.UtcNow.ToString("HH:mm"); } else { time = DateTime.UtcNow.AddHours(1).ToString("HH:mm"); }

            LONWeather.Text = "London: " + clnt.intcarbLON + "°C";
            LISWeather.Text =  "Lisbon: " + clnt.intcarb + "°C";

            if (Connectivity.NetworkAccess == NetworkAccess.Internet && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi)) { DataOnHome.Text = "Data On"; } else { DataOnHome.Text = "Data Off"; }

            DateHome.Text = DateTime.Now.DayOfWeek.ToString();
            MonthHome.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);

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

            if (Connectivity.NetworkAccess == NetworkAccess.None && !popup) { DisplayAlert("Notice", "Your Device is offline\nSome App functions may be unavailable", "OK"); LONWeather.Text = "Offline"; LISWeather.Text = "Offline"; popup = true; }
        }

        private void StartFlight_Clicked(object sender, EventArgs e) 
        {
            if (flighttimer.IsRunning) { flighttimer.Reset(); StartFlight.Source = "PortugalStartIcon"; return; }            
            else { flighttimer.Start(); StartFlight.Source = "PortugalEndIcon"; UniversialUpdate(); UKTIME.Opacity = 1; PORTIME.Opacity = 1; LocTime.Opacity = 1; }                  
        }     
        private void StartSleep_Clicked(object sender, EventArgs e)
        {
            if (sleeptimer.IsRunning) { sleeptimer.Reset(); StartSleep.Source = "PortugalStartIcon"; return; }
            else { sleeptimer.Start(); StartSleep.Source = "PortugalEndIcon"; UniversialUpdate(); }         
        }
        private void UpdateSleep_Clicked(object sender, EventArgs e) { if (sleeptimer.IsRunning) { UniversialUpdate(); } }
        private void UpdteHome_Clicked(object sender, EventArgs e) { UniversialUpdate(); }       
        private void SOS_Clicked(object sender, EventArgs e) { try { PhoneDialer.Open("112"); } catch (Exception) { } } 
        private void CurrencyEUR_TextChanged(object sender, TextChangedEventArgs e) { try { CurrencyGBP.Text = "£" + Math.Round(Convert.ToDouble(CurrencyEUR.Text) * Convert.ToDouble(clnt.varsyr), 2);  } catch (Exception) { } }            
    }
}