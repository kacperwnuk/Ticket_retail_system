using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity.DictionaryTypes
{
    public class DiscountTypeDictionary
    {
        [Required, Key]
        public DiscountType DiscountTypeId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int PercentDiscount { get; set; }
    }
}