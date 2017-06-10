using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "音频文件|*.mp3";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = openFileDialog.FileName;
                label1.Text = "正在播放:" + openFileDialog.FileName;
                pictureBox1.ImageLocation = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s=textBox1.Text;
            if (s != null&&s!="")
            {

                getUrl(s);
            }
        }

        public string getUrl(string s)
        {
            string url = String.Format("http://s.music.163.com/search/get/?type=1&limit=3&s={0}",s);
            //label1.Text = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            //label1.Text = retString;
            jsonParser(retString);
            return null;
        }

        public void jsonParser(string jsonText)
        {
            try
            {
                JObject o = (JObject)JsonConvert.DeserializeObject(jsonText);
                JObject result = (JObject)o["result"];
                JArray songs = (JArray)result["songs"];
                string audio = (string)songs[0]["audio"];
                string name = (string)songs[0]["name"];
                JArray artists = (JArray)songs[0]["artists"];
                string aname = (string)artists[0]["name"];
                JObject album = (JObject)songs[0]["album"];
                string picUrl = (string)album["picUrl"];
                label1.Text = "正在播放:" + aname + " - " + name;
                pictureBox1.ImageLocation = picUrl;
                axWindowsMediaPlayer1.URL = audio;
            }
            catch (Exception e)
            {

            }

        }
    }
}
