using ImageShare.Data;

namespace ImageShare.Web.Models
{
    public class UploadViewModel
    {
        public Image Image { get; set; }
        public bool Unlocked { get; set; }
        public string InvalidPassword { get; set; }
    }
}
