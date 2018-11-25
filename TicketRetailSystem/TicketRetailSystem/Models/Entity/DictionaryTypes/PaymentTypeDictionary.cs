using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity.DictionaryTypes
{
    public class PaymentTypeDictionary
    {
        [Required, Key]
        public PaymentType PaymentTypeId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}