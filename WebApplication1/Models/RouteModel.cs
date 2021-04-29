using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class RouteModel
    {
        [Key]
        public int RouteID { get; set; }
        public int LineID { get; set; }
        public int BuggyID { get; set; }

        public LineModel Line { get; set; }
        public BuggyModel Buggy { get; set; }
    }
}
