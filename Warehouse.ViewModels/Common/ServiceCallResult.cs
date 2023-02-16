using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Common
{
    public class ServiceCallResult
    {
        public ServiceCallResult()
        {
            ErrorMessages = new List<string>();
            WarningMessages = new List<string>();
            SuccessMessages = new List<string>();
        }
        public bool Success { get; set; }
        public object Item { get; set; }

        public IList<string> ErrorMessages { get; set; }
        public IList<string> SuccessMessages { get; set; }
        public IList<string> WarningMessages { get; set; }
    }
    public class LanguageListModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

    }
    public class ImageViewModel
    {
        public string FileName { get; set; }
        public string BlogUniqueTempId { get; set; }
        public string ServiceUniqueTempId { get; set; }


        
        public string Title { get; set; }

        
        public string Alt { get; set; }
    }
    public class ImageListViewModel
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }

    }

    public class ImageInsertViewModel
    {
        public string BlogUniqueTempId { get; set; }
        public string ServiceUniqueTempId { get; set; }
        public string HtmlPrefix { get; set; }
        
        public string Title { get; set; }

       
        public string Alt { get; set; }
    }
}