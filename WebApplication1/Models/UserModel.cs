using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int LineID { get; set; }
        public LineModel Line { get; set; }
    }
}
