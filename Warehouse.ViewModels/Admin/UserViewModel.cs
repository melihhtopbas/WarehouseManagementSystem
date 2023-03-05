using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Warehouse.ViewModels.Common;

namespace Warehouse.ViewModels.Admin
{
    public class UserViewModel : UserCrudBaseViewModel
    {
      

    }
    public class UserListViewModel
    {
        public long Id { get; set; }


        public string UserName { get; set; }


        public string Password { get; set; }
        public string Role { get; set; }



        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public long CustomerId { get; set; }         

    }
    public class UserAddViewModel : UserCrudBaseViewModel
    {
        [DisplayFormat(DataFormatString = "MM/dd/yyyy HH:mm", ApplyFormatInEditMode = true)]

        public DateTime Date { get; set; }
    }
    public class UserEditViewModel : UserCrudBaseViewModel
    {
        [DisplayFormat(DataFormatString = "MM/dd/yyyy HH:mm", ApplyFormatInEditMode = true)] 

        public DateTime Date { get; set; }

        public string Day { get; set; }
        public TimeSpan TimeDay { get; set; }
    }

    public class UserSearchViewModel
    {
         

        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages),
            ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }

    }

    public class UserCrudBaseViewModel
    {


        public long Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
      
        [StringLength(50)]
       
      
        public string UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        public string Role { get; set; }


        [Display(Name = "Adı")]
       
        [StringLength(50)]
        public string Name { get; set; }
     
        [StringLength(50)]
        [Display(Name = "Soy Adı")]
        public string Surname { get; set; }
        [Display(Name = "Mail")]
       
        [StringLength(100)]

        public string Mail { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefon numarası numerik olmalıdır!")]
        [Display(Name = "Telefon")]
       
        [StringLength(11)]

        public string Phone { get; set; }
        public OrderCountryIdSelectViewModel Country { get; set; }

        public OrderCityIdSelectViewModel City { get; set; }

    }
    public class UserChangePasswordViewModel
    {
        [Display(Name = "Eski Şifre")]
        [StringLength(50)]
        [Required(ErrorMessage ="Eski şifrenizi giriniz")]
        public string OldPassword { get; set; }
        [Display(Name = "Yeni Şifre")]
        [StringLength(50)]
        [Required(ErrorMessage = "Yeni şifrenizi giriniz")]
        public string NewPassword { get; set; }
        [Display(Name = "Yeni Şifre(tekrar)")]
        [StringLength(50)]
        [Required(ErrorMessage = "Yeni şifrenizi tekrar giriniz")]
        public string ReNewPassword { get; set; }

    }
    public class UserForgotPasswordViewModel
    {
        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Mail adresinizi giriniz")]
        [StringLength(100)]

        public string Mail { get; set; }

        public string Message { get; set; }
        public string Subject { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }



    }

}
