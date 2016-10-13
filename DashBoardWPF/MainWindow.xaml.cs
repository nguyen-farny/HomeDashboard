using Newtonsoft.Json;
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
            //DataGridTextColumn schedules_col = new DataGridTextColumn();
            //schedules_col.Header = "Schedules";

            //rer_grid.Columns.Add(schedules_col);

            //actualize info every 2 sec
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += GetData;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();


        }
        private void GetData(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api-ratp.pierre-grimaud.fr/v2/rers/A/stations/22?destination=2");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("22?destination=2").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                dynamic jsonResponse = JsonConvert.DeserializeObject(jsonString.Result);

                rer_grid.ItemsSource = jsonResponse.response.schedules;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }

        }
    }

}
