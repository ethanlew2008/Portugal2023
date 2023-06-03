﻿using System;
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
            if (Connectivity.NetworkAccess == NetworkAccess.None && !popup) { DisplayAlert("Notice", "Your Device is offline\nSome App functions may be unavailable", "OK"); LONWeather.Text = "Offline"; LISWeather.Text = "Offline"; popup = true; }
        }
        public async void UniversialUpdate()
        {          
            if (clnt.intcarb == 0) { await clnt.GetWeather(); await clnt.GetWeatherLON(); };
            LONWeather.Text = "London: " + clnt.intcarbLON + "°C";
            LISWeather.Text = "Lisbon: " + clnt.intcarb + "°C";
        }

        private void StartFlight_Clicked(object sender, EventArgs e) 
        {
            if (flighttimer.IsRunning) { flighttimer.Reset(); StartFlight.Source = "PortugalStartIcon"; return; }            
            else { flighttimer.Start(); StartFlight.Source = "PortugalEndIcon"; UniversialUpdate(); UKTIME.Opacity = 1; PORTIME.Opacity = 1; LocTime.Opacity = 1; }                  
        }     
        private void StartSleep_Clicked(object sender, EventArgs e)
        {
            if (sleeptimer.IsRunning) { sleeptimer.Reset(); StartSleep.Source = "PortugalStartIcon"; return; }
            else { sleeptimer.Start(); StartSleep.Source = "PortugalEndIcon"; UniversialUpdate(); UKTimeSleep.Opacity = 1; PORTimeSleep.Opacity = 1; DayOfWeek.Opacity = 1; }         
        }
        private void SOS_Clicked(object sender, EventArgs e) { try { PhoneDialer.Open("112"); } catch (Exception) { } } 
        private void CurrencyEUR_TextChanged(object sender, TextChangedEventArgs e) { try { CurrencyGBP.Text = "£" + Math.Round(Convert.ToDouble(CurrencyEUR.Text) * Convert.ToDouble(clnt.varsyr), 2);  } catch (Exception) { } }            
    }
}