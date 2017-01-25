using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OAuthDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XX("b22r76swj3m3qxqmc7rvgys7");
        }

        private async void XX(string authorizationCode)
        {
            var client_id = "8q3hybhe8xcsv48d868kmbh23ayfsdg8";
            var client_secret = "SgeD37MWvbWPtJUw34T8cKUUvz4gFzfH";
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(client_id + ":" + client_secret));

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            IList<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("redirect_uri", "https://my-diablo3.51vip.biz:29002/callback/authorizationrequestcallback"));
            param.Add(new KeyValuePair<string, string>("scope", ""));
            param.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            param.Add(new KeyValuePair<string, string>("code", authorizationCode));
            var httpContent = new FormUrlEncodedContent(param);

            var response = await httpClient.PostAsync("https://www.battlenet.com.cn/oauth/token", httpContent);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var accessToken = JObject.Parse(responseContent)["access_token"].ToString();
            }
            else
            {
            }
        }
    }
}
