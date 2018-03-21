using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GNM.Models
{
    public class Node
    {
        public Node()
        {
            GatewayList = new List<SelectListItem>();
        }
        public int NodeId { get; set; }

        [Required(ErrorMessage = "Node Name is required")]
        public string NodeName { get; set; }

        [Display(Name="Gateway")]
        [Required(ErrorMessage = "Gateway is required")]
        public int GatewayId { get; set; }

        public int CreatedBy { get; set; }
             
        public string Createddate { get; set; }

        public bool IsStart { get; set; }

        public string Status { get; set; }

        //public string UserName { get; set; }
        //public User UserData { get; set; }

        public IList<SelectListItem> GatewayList { get; set; }

       // public string GatewayName { get; set; }
       // public Gateway GatewayData { get; set; }
    }
}