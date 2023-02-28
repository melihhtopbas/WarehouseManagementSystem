using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Warehouse.ViewModels.Common;

namespace Warehouse.ViewModels.Admin
{
    public class ReferenceListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }
    }
    public class ReferenceAddViewModel : ReferenceCrudBaseViewModel
    {


    }
    public class ReferenceEditViewModel : ReferenceCrudBaseViewModel
    {
        public long Id { get; set; }
    }
    public class ReferenceSearchViewModel
    {
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Language")]
        public long LanguageId { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages),
            ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }
    }

    public class ReferenceCrudBaseViewModel
    {
        public ReferenceCrudBaseViewModel()
        {
            ImageViewModels = new List<ImageViewModel>();

        }

        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }

        public string FileName { get; set; }

        public IEnumerable<ImageViewModel> ImageViewModels { get; set; }
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

    }
}
