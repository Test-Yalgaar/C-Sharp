using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GNM.Models;
using System.Web.Mvc;
using System.Globalization;
using NpgsqlTypes;

namespace GNM.Service
{
    public class GatewayService : IGatewayService
    {
        NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GNMConnection"].ConnectionString);


        public IList<Gateway> GetAllGatewayData()
        {
            try
            {
                IList<Gateway> model = new List<Gateway>();
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand();
                NpgsqlDataReader dr;
                cmd.CommandText = "SELECT * FROM gateway_mst order by gatewayid";
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Gateway gw = new Gateway();
                    gw.GatewayId = Convert.ToInt32(dr["gatewayid"].ToString());
                    gw.GatewayName = dr["gatewayname"].ToString();
                    model.Add(gw);
                }
                conn.Close();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// AddGateway method used to all Gateway
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddGateway(Gateway model)
        {
            try
            {
                conn.Open();
                IFormatProvider culture = new CultureInfo("en-US", true);
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "Insert into gateway_mst(gatewayname,createdby,createddate) values(@gatewayname,@createdby,@createddate)";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayname",NpgsqlDbType.Text,model.GatewayName);
                cmd.Parameters.AddWithValue("@createdby", NpgsqlDbType.Integer, model.CreatedBy);
                cmd.Parameters.AddWithValue("@createddate", NpgsqlDbType.Timestamp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture));
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// UpdateGateway method used to Update Gateway
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateGateway(Gateway model)
        {
            try
            {
                conn.Open();
                IFormatProvider culture = new CultureInfo("en-US", true);
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "update gateway_mst set gatewayname=@gatewayname,updatedby=@createdby,updateddate=@createddate where gatewayid=@gatewayId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayname", NpgsqlDbType.Text, model.GatewayName);
                cmd.Parameters.AddWithValue("@createdby", NpgsqlDbType.Integer, model.CreatedBy);
                cmd.Parameters.AddWithValue("@createddate", NpgsqlDbType.Timestamp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture));
                cmd.Parameters.AddWithValue("@gatewayId",NpgsqlDbType.Integer,model.GatewayId);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// IsGatewayNameExist method used to check Gateway name is Exist or not
        /// </summary>
        /// <param name="gatewayid"></param>
        /// <param name="gatewayname"></param>
        /// <returns></returns>
        public bool IsGatewayNameExist(int gatewayid, string gatewayname)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "select * from gateway_mst where UPPER(gatewayname)=UPPER(@gatewayname) and gatewayid!=@gatewayId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayId", NpgsqlDbType.Integer, gatewayid);
                cmd.Parameters.AddWithValue("@gatewayname", NpgsqlDbType.Text, gatewayname);
                var i = cmd.ExecuteReader();
                if (i.Read())
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetGatewayByGatewayId mthod used to get Gateway data by GatewayId
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        public Gateway GetGatewayByGatewayId(int gatewayId)
        {
            try
            {
                conn.Open();
                Gateway model = new Gateway();
                NpgsqlCommand cmd = new NpgsqlCommand();
                NpgsqlDataReader dr;
                cmd.CommandText = "SELECT * FROM gateway_mst where gatewayid=" + gatewayId;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayId", NpgsqlDbType.Integer, model.GatewayId);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    model.GatewayId = Convert.ToInt32(dr["gatewayid"].ToString());
                    model.GatewayName = dr["gatewayname"].ToString();
                }
                conn.Close();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete Gateway method used to delete gateway
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteGateway(int id)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "delete FROM gateway_mst where gatewayid=@gatewayId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayId", NpgsqlDbType.Integer, id);
                var i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetGatewayList methos used for dropdown menu
        /// </summary>
        /// <returns></returns>
        public IList<SelectListItem> GetGatewayList()
        {
            try
            {
                IList<SelectListItem> model = new List<SelectListItem>();
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                NpgsqlDataReader dr;
                cmd.CommandText = "select gatewayid,gatewayname from gateway_mst";
                cmd.Connection = conn;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = dr["gatewayname"].ToString();
                    item.Value = dr["gatewayid"].ToString();
                    model.Add(item);
                }
                conn.Close();
                return model;
            }
            catch (Exception rx)
            {
                throw rx;
            }
        }

        /// <summary>
        /// GatewayByGatewayName method used to Get Gateway Information by name
        /// </summary>
        /// <param name="gatewayname"></param>
        /// <returns></returns>
        public Gateway GatewayByGatewayName(string gatewayname)
        {
            try
            {
                conn.Open();
                Gateway model = new Gateway();
                NpgsqlDataReader dr;
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "select * from gateway_mst where gatewayname=@gatewayname";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayname", NpgsqlDbType.Text, gatewayname);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    model.GatewayId = Convert.ToInt32(dr["gatewayid"].ToString());
                    model.GatewayName = dr["gatewayname"].ToString();
                }
                conn.Close();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}