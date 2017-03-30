using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoGallery.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public bool Show { get; set; } = false;
    }
}