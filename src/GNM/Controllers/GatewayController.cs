using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GNM.Models;
using GNM.Service;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace GNM.Controllers
{
    public class GatewayController : Controller
    {
        //
        // GET: /Gateway/
        #region ServiceDelaration
        IGatewayService _gatewayService = new GatewayService();
        INodeService _nodeService = new NodeService();
        #endregion

        public ActionResult Index()
        {
            //GetLocalIPAddress();
            if (Session["UserId"] != null)
            {
                IList<Gateway> model = new List<Gateway>();
                model = _gatewayService.GetAllGatewayData();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //ip address
        //public static string GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("Local IP Address Not Found!");
        //}

        [HttpPost]
        public ActionResult AddEditGateway(Gateway model)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isGatewayExist = _gatewayService.IsGatewayNameExist(model.GatewayId, model.GatewayName);
                        if (!isGatewayExist)
                        {
                            int userId = Convert.ToInt32(Session["UserId"].ToString());
                            var i = 0;
                            model.CreatedBy = userId;
                            model.CreatedDate = DateTime.Now;
                            if (model.GatewayId > 0)
                            {
                                i = _gatewayService.UpdateGateway(model);
                                TempData["Gateway"] = "Updated";
                            }
                            else
                            {
                                TempData["Gateway"] = "Inserted";
                                i = _gatewayService.AddGateway(model);
                                model = _gatewayService.GatewayByGatewayName(model.GatewayName);
                            }

                            if (i > 0)
                            {
                                SendMessage(TempData["Gateway"].ToString(), model.GatewayId, model.GatewayName);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Gateway"] = null;
                                return View("AddEditGateway", model);
                            }
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("", "Gateway already Exist");
                            return View("AddEditGateway", model);
                        }
                    }
                    else
                    {
                        return View(model);
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

        public ActionResult AddEditGateway(int id = 0)
        {
            if (Session["UserId"] != null)
            {
                Gateway model = new Gateway();
                if (id > 0)
                {
                    model = _gatewayService.GetGatewayByGatewayId(id);
                }
                return View(model);
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
        //        Gateway model = new Gateway();
        //        model = _gatewayService.GetGatewayByGatewayId(id);
        //        return View("AddEditGateway", model);
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
                    IList<Node> node = new List<Node>();
                    node = _nodeService.GetNodeDataByGatewayId(id);
                    if (node.Count > 0)
                    {
                        TempData["Gateway"] = "First Delete All Existing Node Of This Gateway";
                        return RedirectToAction("Index");
                    }
                    var i = _gatewayService.DeleteGateway(id);
                    if (i > 0)
                    {
                        TempData["Gateway"] = "Deleted";
                        SendMessage(TempData["Gateway"].ToString(), id, "");
                    }
                    else
                    {
                        TempData["Gateway"] = "Problem in Deleting Gateway";
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

        public JsonResult GetGatewayList()
        {
            try
            {
                IList<SelectListItem> item = new List<SelectListItem>();
                item = _gatewayService.GetGatewayList();
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Yalgaar Publish
        private void SendMessage(string action, int gatewayId, string gatewayname)
        {
            var data = new
            {
                type = "Gateway",
                action = action,
                gatewayId = gatewayId,
                gatewayname = gatewayname
            };
            var logJson = JsonConvert.SerializeObject(data);
            YalgaarPubSub.SendMessage("gateway", logJson);
        }
        #endregion


        //#region PratialView PopUp
        //public ActionResult Details()
        //{
        //    Gateway frnds = new Gateway();
        //  //  frnds = db.FriendsInfo.Find(Id);
        //    return PartialView("_Details", frnds);
        //}   
        //#endregion

    }
}