using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YalgaarNet;

namespace YalgaarWinFormExample
{
    public partial class YalgaarExample : Form
    {
        YalgaarClient yalgaar = null;
        public YalgaarExample()
        {
            InitializeComponent();
            Connect("ck-47046d75cf64402f", false);
        }

        public void Connect(string ClientKey, bool Secure)
        {
            yalgaar = new YalgaarClient(ClientKey, Secure, ConnectionCallBack);
            SubscribeMessage();
            PublishMessage();
            yalgaar.IsConnectionClose += YalgaarClient_IsConnectionClose;

        }
        private bool PublishMessage()
        {
            yalgaar.Publish("YourChannel", "This is Yalgaar .Net SDK Example");
            return true;
        }
        private bool SubscribeMessage()
        {
            yalgaar.Subscribe("YourChannel", SubscribeReturnMessage, SubErrorMessage);
            return true;
        }
        public void SubscribeReturnMessage(string Channel, string message)
        {
            MessageBox.Show(message);
            yalgaar.CloseConnection();
            Application.Exit();
        }
        public void SubErrorMessage(string message)
        {
            MessageBox.Show(message);
        }
        void YalgaarClient_IsConnectionClose(string ErrorMessage)
        {
            MessageBox.Show("Connection Close: " + ErrorMessage);

        }
        public void ConnectionCallBack(bool msg)
        {
            if (msg)
                MessageBox.Show("Connected");
        }
    }
}
