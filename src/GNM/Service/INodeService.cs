using GNM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNM.Service
{
    interface INodeService
    {

        /// <summary>
        /// GetAllNodeData mthod used to Get All Node Data Form table
        /// </summary>
        /// <returns></returns>
        //IList<Node> GetAllNodeData();
        DataTable GetAllNodeData();
        IList<Gateway> GetNodeList();
        /// <summary>
        /// AddNode method is used to add New Node
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddNode(Node model);

        /// <summary>
        /// UpdateNode method is used to update Node
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateNode(Node model);

        /// <summary>
        /// GetNodeByNodeId methos is used to Get Node By NodeId
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Node GetNodeByNodeId(int nodeId);

        /// <summary>
        /// DeleteNode method is used to Delete Node From table
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        int DeleteNode(int nodeId);

        /// <summary>
        /// IsNodeExist method is used to Check Node Name is exist or Not
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="nodename"></param>
        /// <returns></returns>
        bool IsNodeExist(int nodeid, string nodename);

        /// <summary>
        /// GetNodeDataByGatewayId method is used to Get Node Date By GatewayId
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        IList<Node> GetNodeDataByGatewayId(int gatewayId);

        /// <summary>
        /// AddNodeLogs method used to add Node State History
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="createdBy"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int AddNodeLogs(int nodeId, int createdBy, string status);

        /// <summary>
        /// GetNodeLogByFromToDate method used to getNode State History
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
       // IList<Node> GetNodeLogByFromToDate(DateTime fromdate, DateTime todate);
        IList<User> GetNodeHistoryList(DateTime fromdate, DateTime todate);

        /// <summary>
        /// UpdateNodeStatus method is used to update Node Status(On/Off)
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        int UpdateNodeStatus(int nodeId, bool isStart);
    }
}
