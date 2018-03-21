using GNM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GNM.Service
{
    interface IGatewayService
    {
        /// <summary>
        /// AddGateway method used to all Gateway
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddGateway(Gateway model);

        /// <summary>
        /// UpdateGateway method used to Update Gateway
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateGateway(Gateway model);

        /// <summary>
        /// Delete Gateway method used to delete gateway
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteGateway(int id);

        /// <summary>
        /// GetGatewayByGatewayId mthod used to get Gateway data by GatewayId
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        Gateway GetGatewayByGatewayId(int gatewayId);

        /// <summary>
        /// IsGatewayNameExist method used to check Gateway name is Exist or not
        /// </summary>
        /// <param name="gatewayid"></param>
        /// <param name="gatewayname"></param>
        /// <returns></returns>
        bool IsGatewayNameExist(int gatewayid, string gatewayname);

        /// <summary>
        /// GetAllGatewayData method used Gel All Gateway data from table
        /// </summary>
        /// <returns></returns>
        IList<Gateway> GetAllGatewayData();

        /// <summary>
        /// GetGatewayList methos used for dropdown menu
        /// </summary>
        /// <returns></returns>
        IList<SelectListItem> GetGatewayList();

        /// <summary>
        /// GatewayByGatewayName method used to Get Gateway Information by name
        /// </summary>
        /// <param name="gatewayname"></param>
        /// <returns></returns>
        Gateway GatewayByGatewayName(string gatewayname);
    }
}
