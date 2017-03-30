using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entity
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
   
        public string Name { get; set; }

        [Required]
        [ForeignKey("Id")]
        public ICollection<User> Users { get; set; }
    }
}
