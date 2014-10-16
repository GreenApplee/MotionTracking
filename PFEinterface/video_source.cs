using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using PFEinterface;

namespace PFEinterface
{
    public partial class video_source : Form
    {
        public video_source()
        {
            InitializeComponent();
        }

        private void video_source_Load(object sender, EventArgs e)
        {
            Form1.VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCpatureDevice in Form1.VideoCaptureDevices)
            {
                vid_source.Items.Add(VideoCpatureDevice.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

           Form1.FinalVideoSource = new VideoCaptureDevice(Form1.VideoCaptureDevices[vid_source.SelectedIndex].MonikerString);
           this.Close();
        }
    }
}
