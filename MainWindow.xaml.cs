using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace weatherAPINutzen
{
    public partial class MainWindow : Window
    {
        private const string ApiKey = "d6a69634defed9faf379c6543444b8d6";
        private readonly HttpClient _httpClient;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await GetWeatherAsync();
        }

        private async Task GetWeatherAsync()
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={Stadt.Text}&appid={ApiKey}";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                    if (weatherInfo != null && weatherInfo.main != null)
                    {
                    double tempKelvin = weatherInfo.main.temp;
                    double tempCelsius = tempKelvin - 273.15;

                    Temp.Text = tempCelsius.ToString("0.00");
                    }

                    else
                    {
                        MessageBox.Show("Wetter Informationen nicht gegeben");
                    }
                }

                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Stadt nicht gefunden");
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
