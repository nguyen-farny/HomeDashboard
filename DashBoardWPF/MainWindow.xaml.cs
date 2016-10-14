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

            var links = new List<string>(System.Configuration.ConfigurationSettings.AppSettings["links"].Split(new char[] { ';' }));

            for(int i = 0; i < links.Count; i++)
            {
                HttpResponseMessage resp = client.GetAsync(links[i]).Result;
                if(resp.IsSuccessStatusCode)
                {
                    var jsonString = resp.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(jsonString.Result);
                    if (i == 0)
                        rer_list.ItemsSource = jsonResponse.response.schedules;
                    else if(i ==1)
                        bus157_list.ItemsSource = jsonResponse.response.schedules;
                    else if(i == 2)
                        bus160_list.ItemsSource = jsonResponse.response.schedules;
                    else if(i == 3)
                        bus378g_list.ItemsSource = jsonResponse.response.schedules;
                    else
                        bus378j_list.ItemsSource = jsonResponse.response.schedules;
                }
                else
                {
                    MessageBox.Show("Error Code" + resp.StatusCode + " : Message - " + resp.ReasonPhrase);
                }
            }         
        }
    }

}
