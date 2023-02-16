using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.ViewModels.WebSite
{
    public class AboutViewModel
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string MainFile { get; set; }
        public string MainFile2 { get; set; }

    }

    public class SettingViewModel
    {
        public long LanguageId { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }
        public string Gplus { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Meta { get; set; }
        public string Maps { get; set; }
        public string Analytics { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }
    }
    [Serializable]
    public class NotificationModel
    {
        public string Title { get; set; }
        public string Message { get; set; }


    }
}
