using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketRetailSystem.Models.Entity
{
    public class User
    {
        [DisplayName("User id")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname {get; set;}

        [Required]
        public string PersonalId { get; set; }

        public virtual List<TransportCard> Cards { get; set; }
    }
}