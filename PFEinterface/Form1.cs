using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BMAclassique;
using SampleGrabberNET;
using abeilles;
using pfe;
using PSO_final;
using System.IO;
using AForge.Controls;
using System.Diagnostics;
using AForge.Video;
using AForge.Video.DirectShow;

namespace PFEinterface
{
    public partial class Form1 : Form
    {
        bool b = true;
        int choix_algo;
        public static FilterInfoCollection VideoCaptureDevices;
        public static VideoCaptureDevice FinalVideoSource;
        VideoSourcePlayer pla = new VideoSourcePlayer();
        FileVideoSource fileSource = new FileVideoSource();
        string chemin = "";
        int play_pause = 0;
        
        public Form1()
        {
            InitializeComponent();
           // webcamToolStripMenuItem.Text = "hello";
            pictureBox5.Visible = false;
            pictureBox4.Visible = false;
            chart1.Series[0].Points.AddXY(1, 8);
            chart1.Series[0].Points.AddXY(2, 12);
            chart1.Series[0].Points.AddXY(3, 5);
            chart1.Series[0].Points.AddXY(4, 9);
            chart1.Series[0].Points.AddXY(5, 2);
            chart1.Series[0].Points.AddXY(6, 10);
            chart1.Series[0].Points.AddXY(7, 6);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Image im1 = pictureBox5.Image;
            Image im2 = pictureBox4.Image;
            if (b) { pictureBox2.Image = im2; b = false; }
            else { pictureBox2.Image = im1; b = true; }
            //fileSource = new AForge.Video.DirectShow.FileVideoSource(chemin);
            fileSource.Source = chemin;
            OpenVideoSource(fileSource);
            fileSource.Start();
        //   fileSource.NewFrame += new AForge.Video.NewFrameEventHandler(fileSource_NewFrame);
            pla.NewFrame += new VideoSourcePlayer.NewFrameHandler(pla_NewFrame);
            
           
        }

        void pla_NewFrame(object sender, ref Bitmap image)
        {
            pictureBox1.Image = image;
            //throw new NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            numericUpDown4.Enabled = true;
            numericUpDown5.Enabled = true;
            button1.Enabled = true;
        }

        private void showResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 979;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void classicBMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choix_algo = 0;
            classicBMAToolStripMenuItem.Checked = true;
            geneticBMAToolStripMenuItem.Checked = false;
            pSOWithBMAToolStripMenuItem.Checked = false;
            beesWithBMAToolStripMenuItem.Checked = false;
        }

        private void geneticBMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choix_algo = 1;
            classicBMAToolStripMenuItem.Checked = false;
            geneticBMAToolStripMenuItem.Checked = true;
            pSOWithBMAToolStripMenuItem.Checked = false;
            beesWithBMAToolStripMenuItem.Checked = false;
        }

        private void pSOWithBMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choix_algo = 2;
            classicBMAToolStripMenuItem.Checked = false;
            geneticBMAToolStripMenuItem.Checked = false;
            pSOWithBMAToolStripMenuItem.Checked = true;
            beesWithBMAToolStripMenuItem.Checked = false;
        }

        private void beesWithBMAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            choix_algo = 3;
            classicBMAToolStripMenuItem.Checked = false;
            geneticBMAToolStripMenuItem.Checked = false;
            pSOWithBMAToolStripMenuItem.Checked = false;
            beesWithBMAToolStripMenuItem.Checked = true;
        }

        private void webcamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            video_source m = new video_source();
            m.Show();
            m.FormClosed += new FormClosedEventHandler(m_FormClosed);
        }

        void m_FormClosed(object sender, FormClosedEventArgs e)
        {
            FinalVideoSource.NewFrame += new AForge.Video.NewFrameEventHandler(FinalVideoSource_NewFrame);

            FinalVideoSource.Start();
        }

        void FinalVideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap image =(Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void localSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { if (fileSource.IsRunning) fileSource.Stop(); }
            catch { }
            try { if (FinalVideoSource.IsRunning) FinalVideoSource.Stop(); }
            catch { }
            openFileDialog1.DefaultExt = "avi";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Video file (*.avi)|*.avi";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
       //     if (openFileDialog1.FileName.Length != 0) textBox1.Text = openFileDialog1.FileName;
           // chemin = openFileDialog1.FileName;
            // open it
           // OpenVideoSource(fileSource);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            chemin = openFileDialog1.FileName;
           
        }

        //void fileSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        //{
        //    pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        //}

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (pla.IsRunning) pla.Stop();
            }
            catch { }

            try
            {
                if (FinalVideoSource.IsRunning) FinalVideoSource.Stop();
            }
            catch { }
        }

        
        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            //  CloseCurrentVideoSource();

            // start new video source
            //  videoSourcePlayer1.VideoSource = source;
            //videoSourcePlayer1.Start();
            pla.VideoSource = source;
            pla.Start();
            // reset stop watch
          //  stopWatch = null;

            // start timer
            //timer.Start();

            this.Cursor = Cursors.Default;
        }
        
    }
}
