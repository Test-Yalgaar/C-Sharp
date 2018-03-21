using GNM.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GNM.Service;
namespace GNM.Controllers
{
    public class NodeStateController : Controller
    {
        //
        // GET: /NodeState/
        #region ServiceDelaration
        IGatewayService _gatewayService = new GatewayService();
        INodeService _nodeService = new NodeService();
        #endregion
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {

                Node model = new Node();
                try
                {
                    model.GatewayList = _gatewayService.GetGatewayList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public JsonResult GetNodeData(string gatewayId)
        {
            IList<Node> model = new List<Node>();
            try
            {
                model = _nodeService.GetNodeDataByGatewayId(Convert.ToInt32(gatewayId));
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception rx)
            {
                throw rx;
            }
        }

        public ActionResult ChangeState(int nodeId,bool IsStart)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    int userId = Convert.ToInt32(Session["UserId"].ToString());
                    int i = 0;
                    //var node = _nodeService.GetNodeByNodeId(nodeId);
                    //if (!node.IsStart)
                    //{
                    //    i = _nodeService.UpdateNodeStatus(nodeId,true);
                    //}
                    //else
                    //{
                    i = _nodeService.UpdateNodeStatus(nodeId, IsStart?false:true);
                    //}
                    
                    if (i > 0)
                    {
                        int j = _nodeService.AddNodeLogs(nodeId, userId, IsStart?"Off":"On");
                        if (j > 0)
                        {
                            return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Some Problem in Changing State", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Some Problem in Changing State", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception rx)
                {
                    throw rx;
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        #region NodeHistory
        public ActionResult NodeHistory()
        {
            if (Session["UserId"] != null)
            {
                IList<User> model = new List<User>();
                DateTime fdate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime tdate = fdate.AddDays(1);
                model = _nodeService.GetNodeHistoryList(fdate, tdate);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        
        public JsonResult GetNodeHistoryData(string fromdate, string todate)
        {
            //IList<Node> model = new List<Node>();
            IList<User> model = new List<User>();
            try
            {
               DateTime fdate = DateTime.ParseExact(fromdate, "d/M/yyyy", CultureInfo.InvariantCulture);
               DateTime tdate = DateTime.ParseExact(todate, "d/M/yyyy", CultureInfo.InvariantCulture);
                tdate = tdate.AddDays(1);
               // model = _nodeService.GetNodeLogByFromToDate(fdate, tdate);
                model = _nodeService.GetNodeHistoryList(fdate, tdate);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}