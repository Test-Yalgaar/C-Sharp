using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GNM.Models
{
    public class Gateway
    {
        public Gateway()
        {
            NodeData = new List<Node>();
        }
        public int GatewayId { get; set; }

        [Required(ErrorMessage = "Gateway Name is required")]
        public string GatewayName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public IList<Node> NodeData { get; set; }
    }
}