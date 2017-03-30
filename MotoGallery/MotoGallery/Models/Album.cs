using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoGallery.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<int> ImagesId { get; set; }
        public ICollection<ImageModel> Images { get; set; }
    }
}