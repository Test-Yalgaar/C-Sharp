using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GNM.Models;
using GNM.Service;
using YalgaarNet;
using Newtonsoft.Json;

namespace GNM.Controllers
{
    public class NodeController : Controller
    {
        //
        // GET: /Node/
        #region Fields
        INodeService _nodeService = new NodeService();
        IGatewayService _gatewayService = new GatewayService();
        #endregion
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                IList<Gateway> gateway = new List<Gateway>();
                try
                {
                    gateway = _nodeService.GetNodeList();
                    return View(gateway);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult AddEdit(int id = 0)
        {
            if (Session["UserId"] != null)
            {
                Node model = new Node();
                if (id > 0)
                {
                    model = _nodeService.GetNodeByNodeId(id);
                }
                model.GatewayList = _gatewayService.GetGatewayList();
                return View("AddEditNode", model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddEdit(Node model)
        {
            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    var isNodeExist = _nodeService.IsNodeExist(model.NodeId, model.NodeName);
                    if (!isNodeExist)
                    {
                        try
                        {
                            int userId = Convert.ToInt32(Session["UserId"].ToString());
                            model.CreatedBy = userId;
                            int i = 0;
                            if (model.NodeId > 0)
                            {
                                i = _nodeService.UpdateNode(model);
                                TempData["Node"] = "Updated";
                            }
                            else
                            {
                                i = _nodeService.AddNode(model);
                                TempData["Node"] = "Inserted";
                                var nodelist = _nodeService.GetNodeDataByGatewayId(model.GatewayId);
                                foreach (var n in nodelist)
                                {
                                    if (n.NodeName == model.NodeName)
                                    {
                                        model.NodeId = n.NodeId;
                                        break;
                                    }
                                }
                            }

                            if (i > 0)
                            {
                                SendMessage(TempData["Node"].ToString(), model.GatewayId, model.NodeId, model.NodeName);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Node"] = null;
                                return View("AddEditNode", model);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("", "Node already Exist");
                        model.GatewayList = _gatewayService.GetGatewayList();
                        return View("AddEditNode", model);
                    }
                }
                else
                {
                    return RedirectToAction("AddNode");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //public ActionResult Edit(int id)
        //{

        //    if (Session["UserId"] != null)
        //    {
        //        try
        //        {
        //            Node model = new Node();
        //            model = _nodeService.GetNodeByNodeId(id);
        //            model.GatewayList = _gatewayService.GetGatewayList();
        //            return View("AddEditNode", model);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}

        public ActionResult Delete(int id)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    var node = _nodeService.GetNodeByNodeId(id);
                    var i = _nodeService.DeleteNode(id);
                    if (i > 0)
                    {
                        TempData["Node"] = "Deleted";
                        SendMessage(TempData["Node"].ToString(), node.GatewayId, node.NodeId, node.NodeName);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        #region Yalgaar Publish
        private void SendMessage(string action, int gatewayId, int nodeId, string ndoename)
        {
            var data = new
            {
                type = "Node",
                action = action,
                nodeId = nodeId,
                nodeName = ndoename,
                gatewayId = gatewayId
            };
            var logJson = JsonConvert.SerializeObject(data);
            YalgaarPubSub.SendMessage("node", logJson);
        }
        #endregion
    }
}