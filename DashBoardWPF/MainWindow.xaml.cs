﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DashBoardWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //actualize info every 1 day
            System.Windows.Threading.DispatcherTimer timer1d = new System.Windows.Threading.DispatcherTimer();
            timer1d.Tick += GetWeather;
            timer1d.Interval = new TimeSpan(24, 0, 0); //à modifier en 24h
            timer1d.Start();

            //actualize info every 30 sec
            System.Windows.Threading.DispatcherTimer timer30s = new System.Windows.Threading.DispatcherTimer();
            timer30s.Tick += GetData;
            timer30s.Interval = new TimeSpan(0, 0, 30); //à modifier en 30s
            timer30s.Start();

            //actualize info every 1 sec
            System.Windows.Threading.DispatcherTimer timer1s = new System.Windows.Threading.DispatcherTimer();
            timer1s.Tick += GetHours;
            timer1s.Interval = new TimeSpan(0, 0, 1);
            timer1s.Start();

            // get data on launch
            GetWeather(this, new EventArgs());
            GetData(this, new EventArgs());
        }

        private void OnGotWeather(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode)
            {
                var jsonString = resp.Content.ReadAsStringAsync();
                jsonString.Wait();
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonString.Result);
        
                Dispatcher.Invoke(() =>
                {
                    Weather.DataContext = jsonResponse.current.condition;
                    WeatherDegree.DataContext = jsonResponse.current;

                    ImageBrush myBrush = new ImageBrush();

                    if (jsonResponse.current.humidity > 95)
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri("../../rain.jpg", UriKind.Relative));
                        isSunny.Background = myBrush;
                    }
                    else
                    {
                        myBrush.ImageSource = new BitmapImage(new Uri("../../sunny.jpg", UriKind.Relative));
                        isSunny.Background = myBrush;
                    }
                });
            }
            else
            {
                Dispatcher.Invoke(() => MessageBox.Show("Error Code" + resp.StatusCode + " : Message - " + resp.ReasonPhrase));
            }
        }

        private void GetWeather(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.apixu.com/v1/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string link = System.Configuration.ConfigurationSettings.AppSettings["linkWeather"];

            client.GetAsync(link).ContinueWith(x => OnGotWeather(x.Result));
        }

        private void GetHours(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["isVnTime"]))
                Lbl_VnTime.Visibility = Visibility.Hidden;
            lbl_hourFr.DataContext = new Hours();
            lbl_hourVn.DataContext = new Hours();
        }

        private void OnGotData(HttpResponseMessage resp, int i)
        {
            if (resp.IsSuccessStatusCode)
            {
                var jsonString = resp.Content.ReadAsStringAsync();
                jsonString.Wait();
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonString.Result);

                foreach (dynamic n in jsonResponse.response.schedules)
                {
                    n.message = n.message.ToString().Substring(0, 2);
                }

                // execute on GUI thread
                Dispatcher.Invoke(() =>
                {
                    if (i == 0)
                        rer_list.ItemsSource = jsonResponse.response.schedules;
                    else if (i == 1)
                        bus157_list.ItemsSource = jsonResponse.response.schedules;
                    else if (i == 2)
                        bus160_list.ItemsSource = jsonResponse.response.schedules;
                    else if (i == 3)
                        bus378g_list.ItemsSource = jsonResponse.response.schedules;
                    else
                        bus378j_list.ItemsSource = jsonResponse.response.schedules;
                });
            }
            else
            {
                Dispatcher.Invoke(() => MessageBox.Show("Error Code" + resp.StatusCode + " : Message - " + resp.ReasonPhrase));
            }
        }

        private void GetData(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api-ratp.pierre-grimaud.fr/v2/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var links = new List<string>(System.Configuration.ConfigurationSettings.AppSettings["linksRATP"].Split(new char[] { ';' }));

            for (int i = 0; i < links.Count; i++)
            {
                int index = i;
                client.GetAsync(links[i]).ContinueWith(x => OnGotData(x.Result, index));
            }
        }
    }

}
