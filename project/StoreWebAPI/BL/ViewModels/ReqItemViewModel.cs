using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;

namespace BL.ViewModels
{
    public class ReqItemViewModel
    {
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Kind { get; set; }
        public string Subkind { get; set; }
        public string Status { get; set; }
        public string Size { get; set; }
        public string Sex { get; set; }
    }
}
