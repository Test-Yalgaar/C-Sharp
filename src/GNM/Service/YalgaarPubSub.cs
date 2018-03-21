using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YalgaarNet;

namespace GNM.Service
{
    public static class YalgaarPubSub
    {
        public static void SendMessage(string ChannelName, string msg)
        {
            YalgaarClient yalgaar = new YalgaarClient(System.Configuration.ConfigurationManager.AppSettings["YalgaarClientKey"].ToString(), false,YalgaarPubSub.ConnectionCallBack);
            yalgaar.IsConnectionClose += YalgaarClient_IsConnectionClose;
            if (yalgaar != null)
            {
                var i=yalgaar.Publish(ChannelName, msg);

                if(i)
                {
                    yalgaar.CloseConnection();
                }
            }
        }

        public static void ConnectionCallBack(bool msg)
        {

        }
        static void YalgaarClient_IsConnectionClose(string ErrorMessage)
        {
            Console.Write(ErrorMessage);
        }
    }
}