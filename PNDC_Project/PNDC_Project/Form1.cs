using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNDC_Project
{
    public partial class chatgpt : Form
    {
        private const string ApiUrl = "http://localhost:5000/answer-gpt";
        public chatgpt()
        {
            InitializeComponent();
        }

        private async void sendBtn_Click(object sender, EventArgs e)
        {
            string textStr = inputText.Text;

            sendBtn.Text = "Sending...";

            string jsonString = $"{{ \"question\": \"{textStr}\" }}";

            string response = await CallApi(jsonString);
            outputText.Text += $"{Environment.NewLine}User: " + textStr;
            outputText.Text += $"{Environment.NewLine}AI: " + response;
            inputText.Text = "";
            sendBtn.Text = "Send";
        }
        private async Task<string> CallApi(string jsonData)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set the API endpoint URL
                    client.BaseAddress = new Uri(ApiUrl);

                    // Set the content type
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Create the content with the JSON data
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Make the POST request
                    HttpResponseMessage response = await client.PostAsync("", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Handle unsuccessful response (e.g., log or display an error)
                        return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log or display an error)
                return $"Exception: {ex.Message}";
            }
        }
    }
}