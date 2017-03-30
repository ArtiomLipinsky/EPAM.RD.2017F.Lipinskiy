using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entity
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }


        public int UserId { get; set; }


        public DateTime? CreationDate { get; set; }


        public int AlbumId { get; set; }

        [Required]
        [ForeignKey("Id")]
        public Album Album { get; set; }



    }
}
