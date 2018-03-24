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
using System.Web.Script.Serialization;
using YalgaarNet;


namespace Yalgaar_DotNet_Client_TestApp
{
    public partial class Frm_Yalgaar_DotNet_SDK_Test : Form
    {

        public string Message;

        private YalgaarClient m_YalgaarClient;

        public Frm_Yalgaar_DotNet_SDK_Test()
        {
            InitializeComponent();
            cmb_AESType.SelectedIndex = 0;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                //m_YalgaarClient = new YalgaarClient(txt_ClientKey.Text, true, null);
                m_YalgaarClient = new YalgaarClient(txt_ClientKey.Text, false, txt_UUID.Text, null);
                //m_YalgaarClient = new YalgaarClient(txt_ClientKey.Text, true, txt_SecretKey.Text,Convert.ToInt32(cmb_AESType.SelectedItem.ToString()), null);
                //m_YalgaarClient = new YalgaarClient(txt_ClientKey.Text, true, txt_UUID.Text, txt_SecretKey.Text, Convert.ToInt32(cmb_AESType.SelectedItem.ToString()), null);

                if (m_YalgaarClient.IsClientConnected == true)
                {
                    MessageBox.Show("Connect");
                }
                else
                {
                    MessageBox.Show("error");
                }
                m_YalgaarClient.IsConnectionClose += m_YalgaarClient_IsConnectionClose;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void m_YalgaarClient_IsConnectionClose(string ErrorMessage)
        {
            MessageBox.Show("Connection Close: "+ ErrorMessage);
        }

        private void btn_Subscribe_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Channel.Text == "")
                    return;

                string l_strCh = txt_Channel.Text;

                if (l_strCh != "")
                {
                    m_YalgaarClient.Subscribe(l_strCh, SubscribeReturnMessage, SubErrorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SubErrorMessage(string message)
        {
            MessageBox.Show("Sub Error:" + message);
        }

        public void PresenceMessage(string message)
        {
            MessageBox.Show("Presence Message: " + message);
        }

        public void SubscribeReturnMessage(string Channel, string message)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    txt_Receive.AppendText(Environment.NewLine);
                    txt_Receive.AppendText(message);
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Publish_Click(object sender, EventArgs e)
        {
            try
            {
                //for (var i = 0; i < 1000; i++)
                //{
                //    for (var j = 0; j < 7; j++)
                //    {
                //        m_YalgaarClient.Publish("a" + j, txt_MsgSend.Text);
                //    }
                //}
                m_YalgaarClient.Publish(txt_Channel.Text, txt_MsgSend.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_History_Click(object sender, EventArgs e)
        {
            try
            {
                m_YalgaarClient.GetHistory(txt_Channel.Text, 10, HistoryReturmMessage, HistoryError);
            }
            catch (Exception ex)
            {
                MessageBox.Show("history"+ ex.Message);
            }
        }

        public void HistoryReturmMessage(object Message)
        {
            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(Message);
            
            if (serializedResult != null)
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    txt_Receive.AppendText(Environment.NewLine);
                    txt_Receive.AppendText(serializedResult);
                }));
                //MessageBox.Show(serializedResult);
            }

        }

        public void HistoryError(string Message)
        {
            MessageBox.Show("History Message Error:" + Message);
        }

        private void btn_Channellist_Click(object sender, EventArgs e)
        {
            try
            {

                m_YalgaarClient.GetChannelList(txt_UUID.Text, Channnelist, ChannellistError);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Channel list"+ ex.Message);
            }
        }

        public void Channnelist(object Message)
        {
            var serializer = new JavaScriptSerializer();
            var chlist = serializer.Serialize(Message);
            this.Invoke(new MethodInvoker(delegate()
            {
                txt_Receive.AppendText(Environment.NewLine);
                txt_Receive.AppendText(chlist);
            }));
            //string chlist = string.Join(",", Message);
            //this.Invoke(new MethodInvoker(delegate()
            //{
            //    txt_Receive.AppendText(Environment.NewLine);
            //    txt_Receive.AppendText(chlist);
            //}));
        }

        public void ChannellistError(string error)
        {
            MessageBox.Show("ChannelList Error:" + error);
        }

        private void btn_UserList_Click(object sender, EventArgs e)
        {
            try
            {

                m_YalgaarClient.GetUUIDList(txt_Channel.Text, Userlist, UserlistError);
            }
            catch (Exception ex)
            {
                MessageBox.Show("User list"+ ex.Message);
            }
        }

        public void Userlist(object Message)
        {
            var serializer = new JavaScriptSerializer();
            var chlist = serializer.Serialize(Message);
            this.Invoke(new MethodInvoker(delegate()
            {
                txt_Receive.AppendText(Environment.NewLine);
                txt_Receive.AppendText(chlist);
            }));
            //string uuidlist = string.Join(",", Message);
            //this.Invoke(new MethodInvoker(delegate()
            //{
            //    txt_Receive.AppendText(Environment.NewLine);
            //    txt_Receive.AppendText(uuidlist);
            //}));
        }

        public void UserlistError(string error)
        {
            MessageBox.Show("Userlist Error:" + error);
        }

        private void btn_Unscuscribe_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Channel.Text == "")
                    return;

                string l_strCh = txt_Channel.Text;

                if (l_strCh != "")
                {
                    m_YalgaarClient.UnSubscribe(l_strCh);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_SubWithPresence_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Channel.Text == "")
                    return;

                string l_strCh = txt_Channel.Text;

                if (l_strCh != "")
                {
                    m_YalgaarClient.Subscribe(l_strCh, SubscribeReturnMessage, PresenceMessage, SubErrorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Frm_Yalgaar_DotNet_SDK_Test_FormClosing(object sender, FormClosingEventArgs e)
        {
            //m_YalgaarClient.
           if(m_YalgaarClient != null && m_YalgaarClient.IsClientConnected!= false) m_YalgaarClient.CloseConnection();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            m_YalgaarClient.CloseConnection();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txt_Receive.Text = "";
        }

    }

    public class test 
    {
        public string msg { get; set; }
        public string time { get; set; }
    }
}
