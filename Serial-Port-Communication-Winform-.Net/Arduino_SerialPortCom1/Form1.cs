using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_SerialPortCom1
{
	public partial class Form1 : Form
	{
        private StringBuilder incomingData = new StringBuilder();


        public Form1()
		{
			InitializeComponent();
			findConnectedPorts();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				serialPort1.PortName = COMPort_cmbBox.Text;
				serialPort1.BaudRate = 115200;
				serialPort1.Parity = System.IO.Ports.Parity.None;
				serialPort1.StopBits = System.IO.Ports.StopBits.One;
				serialPort1.DtrEnable = true;
				serialPort1.Open();
				progressBar1.Value = 100;
			}
			catch (Exception exe)
			{
				MessageBox.Show(exe.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//throw;
			}
		}

		public void findConnectedPorts()
		{
			string[] ports = SerialPort.GetPortNames();
			foreach (string port in ports)
			{
				COMPort_cmbBox.Items.Add(port);
			}
		}

		private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
            string data = serialPort1.ReadExisting();
            incomingData.Append(data);
            ProcessIncomingData();
        }


        private void ProcessIncomingData()
        {
            int commaIndex = incomingData.ToString().IndexOf(',');
            while (commaIndex >= 0)
            {
                string dataToDisplay = incomingData.ToString().Substring(0, commaIndex + 1);
                incomingData.Remove(0, commaIndex + 1);

                WriteIncomingData(dataToDisplay);

                commaIndex = incomingData.ToString().IndexOf(',');
            }
        }

        private void WriteIncomingData(string text)
        {
            string[] words = text.Split(',');
            if (words.Length > 0)
            {
                string formattedText = string.Join(Environment.NewLine, words);
                BeginInvoke(new EventHandler(delegate
                {
                    ReceiverTextBox.AppendText(formattedText + Environment.NewLine);
                    ReceiverTextBox.ScrollToCaret();
                }));
            }
        }



        private void button2_Click_1(object sender, EventArgs e)
		{
			serialPort1.Close();
			progressBar1.Value = 0;
		}

        private void baudRate_cmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
