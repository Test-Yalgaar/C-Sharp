using GNM.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GNM.Service
{
    public class NodeService : INodeService
    {
        NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GNMConnection"].ConnectionString);

        /// <summary>
        /// GetAllNodeData mthod used to Get All Node Data Form table
        /// </summary>
        /// <returns></returns>
        //public IList<Node> GetAllNodeData()
        //{
        //    try
        //    {
        //        IList<Node> model = new List<Node>();
        //        conn.Open();
        //        NpgsqlCommand cmd = new NpgsqlCommand();
        //        NpgsqlDataReader dr;
        //        cmd.CommandText = "SELECT nodeid,nodename,gatewayname FROM node_mst join gateway_mst on node_mst.gatewayid = gateway_mst.gatewayid order by nodeid";
        //        cmd.Connection = conn;
        //        dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            Node node = new Node();
        //            node.NodeId = Convert.ToInt32(dr["nodeid"].ToString());
        //            node.NodeName = dr["nodename"].ToString();
        //            node.GatewayName = dr["gatewayname"].ToString();
        //            model.Add(node);
        //        }
        //        conn.Close();
        //        return model;
        //        //return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataTable GetAllNodeData()
        {
            try
            {
                IList<Node> model = new List<Node>();
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                // NpgsqlDataReader dr;
                cmd.CommandText = "SELECT nodeid,nodename,gatewayname FROM node_mst join gateway_mst on node_mst.gatewayid = gateway_mst.gatewayid order by nodeid";
                cmd.Connection = conn;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<Gateway> GetNodeList()
        {
            DataTable dt;
            dt = GetAllNodeData();
            List<Gateway> nodeData = (from dtNode in dt.AsEnumerable()
                                      group dtNode by new
                                          {
                                              gatewayName = dtNode.Field<string>("gatewayname"),
                                          } into g
                                      select new Gateway
                                      {
                                          GatewayName = g.Key.gatewayName,
                                          NodeData = (from DataRow drapp in dt.Rows
                                                      where (drapp["gatewayname"].ToString() == g.Key.gatewayName)
                                                      select new Node()
                                                      {
                                                          NodeId = Convert.ToInt32(drapp["nodeid"].ToString()),
                                                          NodeName = drapp["nodename"].ToString()
                                                      }).ToList(),
                                      }).ToList();

            return nodeData;
            //var nodeData = (from vardt in dt.AsEnumerable()
            //                group vardt by new { placeCol = vardt["gatewayname"] } into g
            //                select new Gateway
            //                {
            //                    GatewayName = g.Key.placeCol.ToString(),
            //                }).ToList();
            //foreach (var i in nodeData)
            //{
            //    var nodelist = (from vardt in dt.AsEnumerable()
            //                    where vardt["gatewayname"].ToString() == i.GatewayName
            //                    select new Node
            //                    {
            //                        NodeId = Convert.ToInt32(vardt["nodeid"].ToString()),
            //                        NodeName = vardt["nodename"].ToString()
            //                    });
            //    i.NodeData = nodelist.ToList();
            //}
            //return nodeData;
        }

        /// <summary>
        /// AddNode method is used to add New Node
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddNode(Node model)
        {
            try
            {
                conn.Open();
                IFormatProvider culture = new CultureInfo("en-US", true);
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "Insert into node_mst(nodename,gatewayid,createdby,createddate,is_start) values(@nodename,@gatewayId,@createdby,@createddate,@IsStart)";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayId", NpgsqlDbType.Integer, model.GatewayId);
                cmd.Parameters.AddWithValue("@nodename", NpgsqlDbType.Text, model.NodeName);
                cmd.Parameters.AddWithValue("@createdby", NpgsqlDbType.Integer, model.CreatedBy);
                cmd.Parameters.AddWithValue("@createddate", NpgsqlDbType.Timestamp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture));
                cmd.Parameters.AddWithValue("@IsStart", NpgsqlDbType.Boolean, false);
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
        /// UpdateNode method is used to update Node
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateNode(Node model)
        {
            try
            {
                conn.Open();
                IFormatProvider culture = new CultureInfo("en-US", true);
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "update node_mst set nodename=@nodename,gatewayid=@gatewayId,updatedby=@createdby,updateddate=@updateddate where nodeid=@nodeId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@nodeId", NpgsqlDbType.Integer, model.NodeId);
                cmd.Parameters.AddWithValue("@gatewayId", NpgsqlDbType.Integer, model.GatewayId);
                cmd.Parameters.AddWithValue("@nodename", NpgsqlDbType.Text, model.NodeName);
                cmd.Parameters.AddWithValue("@createdby", NpgsqlDbType.Integer, model.CreatedBy);
                cmd.Parameters.AddWithValue("@updateddate", NpgsqlDbType.Timestamp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture));
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
        /// DeleteNode method is used to Delete Node From table
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public int DeleteNode(int nodeId)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "delete FROM node_mst where nodeid=@nodeId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@nodeId", NpgsqlDbType.Integer, nodeId);
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
        /// IsNodeExist method is used to Check Node Name is exist or Not
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="nodename"></param>
        /// <returns></returns>
        public bool IsNodeExist(int nodeid, string nodename)
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select * from node_mst where UPPER(nodename)=UPPER(@nodename)and nodeid!=@nodeId";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@nodeId", NpgsqlDbType.Integer, nodeid);
            cmd.Parameters.AddWithValue("@nodename", NpgsqlDbType.Text, nodename);
            var i = cmd.ExecuteReader();
            if (i.Read())
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        /// <summary>
        /// GetNodeByNodeId methos is used to Get Node By NodeId
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public Node GetNodeByNodeId(int nodeId)
        {
            try
            {
                Node model = new Node();
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                NpgsqlDataReader dr;
                cmd.CommandText = "SELECT * FROM node_mst where nodeid=@nodeId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@nodeId", NpgsqlDbType.Integer, nodeId);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    model.GatewayId = Convert.ToInt32(dr["gatewayid"].ToString());
                    model.NodeName = dr["nodename"].ToString();
                    model.NodeId = Convert.ToInt32(dr["nodeid"].ToString());
                    model.IsStart = Convert.ToBoolean(dr["is_start"].ToString());
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
        /// GetNodeDataByGatewayId method is used to Get Node Date By GatewayId
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        public IList<Node> GetNodeDataByGatewayId(int gatewayId)
        {
            try
            {
                IList<Node> model = new List<Node>();
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                NpgsqlDataReader dr;
                cmd.CommandText = "select * from node_mst where gatewayid=@gatewayId order by nodeid";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@gatewayId", NpgsqlDbType.Integer, gatewayId);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Node node = new Node();
                    node.NodeId = Convert.ToInt32(dr["nodeid"].ToString());
                    node.NodeName = dr["nodename"].ToString();
                    node.IsStart = Convert.ToBoolean(dr["is_start"]);
                    model.Add(node);
                }
                conn.Close();
                return model;
            }
            catch (Exception rx)
            {
                throw rx;
            }
        }

        #region Node State
        /// <summary>
        /// AddNodeLogs method used to add Node State History
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="createdBy"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int AddNodeLogs(int nodeId, int createdBy, string status)
        {
            try
            {
                conn.Open();
                IFormatProvider culture = new CultureInfo("en-US", true);
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "insert into node_logs(nodeid,createdby,createddate,status) values (@nodeId,@createdby,@createddate,@status)";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@nodeId", NpgsqlDbType.Integer, nodeId);
                cmd.Parameters.AddWithValue("@status", NpgsqlDbType.Text, status);
                cmd.Parameters.AddWithValue("@createdby", NpgsqlDbType.Integer, createdBy);
                cmd.Parameters.AddWithValue("@createddate", NpgsqlDbType.Timestamp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture));
                int j = cmd.ExecuteNonQuery();
                conn.Close();
                return j;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetNodeLogByFromToDate method used to getNode State History
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        //public IList<Node> GetNodeLogByFromToDate(DateTime fromdate, DateTime todate)
        //{
        //    try
        //    {
        //        IList<Node> model = new List<Node>();
        //        conn.Open();
        //        NpgsqlCommand cmd = new NpgsqlCommand();
        //        NpgsqlDataReader dr;
        //       // DateTime fdate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        //DateTime tdate = DateTime.ParseExact(todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        cmd.CommandText = "SELECT nodename,gatewayname,status,node_logs.createddate,user_mst.username FROM node_logs join node_mst on node_logs.nodeid = node_mst.nodeid join gateway_mst on node_mst.gatewayid = gateway_mst.gatewayid join user_mst on node_logs.createdby=user_mst.userid where node_logs.createddate>='" + fromdate + "' and node_logs.createddate<='" + todate + "' order by nodelogid";
        //        cmd.Connection = conn;
        //        dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            Node node = new Node();
        //           // node.Createddate = Convert.ToDateTime(dr["createddate"].ToString()).ToString("dd/MM/yyyy HH:MM", CultureInfo.InvariantCulture);
        //            IFormatProvider culture = new CultureInfo("en-US", true);
        //            node.Createddate = Convert.ToDateTime(dr["createddate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss", culture);
        //            node.NodeName = dr["nodename"].ToString();
        //            node.GatewayName = dr["gatewayname"].ToString();
        //            node.Status = dr["status"].ToString();
        //            node.UserName = dr["username"].ToString();
        //            model.Add(node);
        //        }
        //        conn.Close();
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetNodeLogByFromToDate(DateTime fromdate, DateTime todate)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                IFormatProvider culture = new CultureInfo("en-US", true);
                cmd.CommandText = "SELECT nodename,gatewayname,status,node_logs.createddate,user_mst.username,user_mst.userid FROM node_logs join node_mst on node_logs.nodeid = node_mst.nodeid join gateway_mst on node_mst.gatewayid = gateway_mst.gatewayid join user_mst on node_logs.createdby=user_mst.userid where node_logs.createddate>=@fromdate and node_logs.createddate<=@todate order by nodelogid desc";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@fromdate", NpgsqlDbType.Timestamp, fromdate.ToString("yyyy-MM-dd HH:mm:ss", culture));
                cmd.Parameters.AddWithValue("@todate", NpgsqlDbType.Timestamp, todate.ToString("yyyy-MM-dd HH:mm:ss", culture));
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList<User> GetNodeHistoryList(DateTime fromdate, DateTime todate)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            DataTable dt;
            dt = GetNodeLogByFromToDate(fromdate, todate);
            var userdata = (from DataRow dtuser in dt.AsEnumerable()
                            group dtuser by new
                            {
                                UserId = dtuser.Field<int>("userid"),
                                UserName = dtuser.Field<string>("username"),
                            } into g
                            select new User
                            {
                                UserId = g.Key.UserId,
                                UserName = g.Key.UserName,
                                GatewayData = (from dtgetway in dt.AsEnumerable()
                                               where (Convert.ToInt32(dtgetway["userid"].ToString()) == g.Key.UserId)
                                               group dtgetway by new
                                               {
                                                   GatewayName = dtgetway.Field<string>("gatewayname"),
                                               } into f
                                               select new Gateway
                                               {
                                                   GatewayName = f.Key.GatewayName,
                                                   NodeData = (from dtNode in dt.AsEnumerable()
                                                               where (dtNode["gatewayname"].ToString() == f.Key.GatewayName && Convert.ToInt32(dtNode["userid"].ToString())==g.Key.UserId)
                                                               select new Node
                                                               {
                                                                   NodeName = dtNode["nodename"].ToString(),
                                                                   Createddate = Convert.ToDateTime(dtNode["createddate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss", culture),
                                                                   Status = dtNode["status"].ToString(),
                                                               }).ToList(),
                                               }).ToList(),
                            }).ToList();

            //var userdata = (from vardt in dt.AsEnumerable()
            //                group vardt by new { placeCol = vardt["userid"], name = vardt["username"] } into g
            //                select new User
            //                {
            //                    UserId = Convert.ToInt32(g.Key.placeCol.ToString()),
            //                    UserName = g.Key.name.ToString(),
            //                }).ToList();
            //foreach (var i in userdata)
            //{
            //    var gatewayData = (from vardt in dt.AsEnumerable()
            //                       where Convert.ToInt32(vardt["userid"].ToString()) == i.UserId
            //                       group vardt by new { placeCol = vardt["gatewayname"] } into g
            //                       select new Gateway
            //                       {
            //                           GatewayName = g.Key.placeCol.ToString(),
            //                       }).ToList();
            //    i.GatewayData = gatewayData;
            //    //}
            //    //foreach (var i in userdata)
            //    //{
            //    foreach (var j in i.GatewayData)
            //    {
            //        var nodelist = (from vardt in dt.AsEnumerable()
            //                        where vardt["gatewayname"].ToString() == j.GatewayName && Convert.ToInt32(vardt["userid"].ToString()) == i.UserId
            //                        orderby vardt["createddate"].ToString() descending
            //                        select new Node
            //                        {
            //                            //NodeId = Convert.ToInt32(vardt["nodeid"].ToString()),
            //                            NodeName = vardt["nodename"].ToString(),
            //                            Createddate = Convert.ToDateTime(vardt["createddate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss", culture),
            //                            Status = vardt["status"].ToString(),
            //                        });
            //        j.NodeData = nodelist.ToList();
            //    }
            //}
            return userdata;
        }
        /// <summary>
        /// /// UpdateNodeStatus method is used to update Node Status(On/Off)
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        public int UpdateNodeStatus(int nodeId, bool isStart)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "update node_mst set is_start=" + isStart + " where nodeid=" + nodeId;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@IsStart", NpgsqlDbType.Boolean, isStart);
                cmd.Parameters.AddWithValue("@nodeId", NpgsqlDbType.Integer, nodeId);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}