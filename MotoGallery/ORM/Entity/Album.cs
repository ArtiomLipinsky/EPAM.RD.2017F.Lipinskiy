using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entity
{
    [Table("Albums")]
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<int> ImagesId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("Id")]
        public ICollection<Image> Images { get; set; }

        [Required]
        public User User { get; set; }
    }
}
