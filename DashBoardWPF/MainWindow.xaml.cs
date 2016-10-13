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
            client.BaseAddress = new Uri("http://api-ratp.pierre-grimaud.fr/v2/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage respRERA = client.GetAsync("rers/A/stations/22?destination=2").Result;
            HttpResponseMessage respBus157 = client.GetAsync("bus/157/stations/ville-faidherbe-rer?destination=pont+de+neuilly").Result;
            HttpResponseMessage respBus160 = client.GetAsync("bus/160/stations/nanterre-ville+rer?destination=pont+de+sevres").Result;
            HttpResponseMessage respBus378g = client.GetAsync("bus/378/stations/nanterre-ville-rer?destination=les+courtilles+metro").Result;
            HttpResponseMessage respBus378j = client.GetAsync("bus/378/stations/jules+quentin?destination=les+courtilles+metro").Result;

            if (respRERA.IsSuccessStatusCode)
            {
                var jsonStringRERA = respRERA.Content.ReadAsStringAsync();
                var jsonStringBus157 = respBus157.Content.ReadAsStringAsync();
                var jsonStringBus160 = respBus160.Content.ReadAsStringAsync();
                var jsonStringBus378g = respBus378g.Content.ReadAsStringAsync();
                var jsonStringBus378j = respBus378j.Content.ReadAsStringAsync();

                jsonStringRERA.Wait();
                jsonStringBus157.Wait();
                jsonStringBus160.Wait();
                jsonStringBus378g.Wait();
                jsonStringBus378j.Wait();

                dynamic jsonResponseRERA = JsonConvert.DeserializeObject(jsonStringRERA.Result);
                dynamic jsonResponseBus157 = JsonConvert.DeserializeObject(jsonStringBus157.Result);
                dynamic jsonResponseBus160 = JsonConvert.DeserializeObject(jsonStringBus160.Result);
                dynamic jsonResponseBus378g = JsonConvert.DeserializeObject(jsonStringBus378g.Result);
                dynamic jsonResponseBus378j = JsonConvert.DeserializeObject(jsonStringBus378j.Result);

                rer_list.ItemsSource = jsonResponseRERA.response.schedules;
                bus157_list.ItemsSource = jsonResponseBus157.response.schedules;
                bus160_list.ItemsSource = jsonResponseBus160.response.schedules;
                bus378g_list.ItemsSource = jsonResponseBus378g.response.schedules;
                bus378j_list.ItemsSource = jsonResponseBus378j.response.schedules;
            }
            else
            {
                MessageBox.Show("Error Code" + respRERA.StatusCode + " : Message - " + respRERA.ReasonPhrase);
                MessageBox.Show("Error Code" + respBus157.StatusCode + " : Message - " + respBus157.ReasonPhrase);
                MessageBox.Show("Error Code" + respBus160.StatusCode + " : Message - " + respBus160.ReasonPhrase);
                MessageBox.Show("Error Code" + respBus378g.StatusCode + " : Message - " + respBus378g.ReasonPhrase);
                MessageBox.Show("Error Code" + respBus378j.StatusCode + " : Message - " + respBus378j.ReasonPhrase);
            }

        }
    }

}
