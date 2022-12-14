using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncDemo
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cancellationTokenSource;
        public Form1()
        {
            InitializeComponent();
        }

        async Task<string> GetString()
        {
            //instatiate cancelation token
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            string s = "";

            //start a background thread
            //await - waits until that thread is done to exeecite Run?
            await Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        //break out of loop
                        break;
                    }

                    s += i;

                    //Run code on UI thread
                    Invoke((Action)(() =>
                    {
                        progressBar1.Value += 10;
                    }));

                    progressBar1.Value += 10;
                    Thread.Sleep(500);
                }
            });
            return s;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            label1.Text = "Working...";
            string s = await GetString();
            label1.Text = s;
            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //cancel the background thread
            //cancelation token
            //.Cancel() method will set token.IsCancellationRequested) to true
            cancellationTokenSource.Cancel();
        }
    }
}
