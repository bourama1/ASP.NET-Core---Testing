using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class BuggyModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int BuggyID { get; set; }
        public string Name { get; set; }

        public ICollection<RouteModel> Routes { get; set; }
    }
}
