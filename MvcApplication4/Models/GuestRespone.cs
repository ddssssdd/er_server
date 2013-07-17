using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MvcApplication4.Models
{
    public class GuestRespone
    {
        [Required(ErrorMessage="Please enter your name")]
        public String Name { get; set; }
        [Required(ErrorMessage="Please input your email address")]
        [RegularExpression(".+\\@.\\..+",ErrorMessage="Please enter a valid email address")]
        public String Email { get; set; }
        [Required(ErrorMessage="Please enter your phone number")]
        public String Phone {get;set;}
        [Required(ErrorMessage="Please specify whether you'll attend")]
        public bool? WillAttend { get; set; }
    }
}