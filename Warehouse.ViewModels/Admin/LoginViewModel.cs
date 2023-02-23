using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class LoginViewModel
    { 
        [Required(ErrorMessage = "Kullanıcı Adı Boş Bırakılamaz!")]
        [Display( Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        
        [DataType(DataType.Password)]
      
        [Required(ErrorMessage = "Şifre Boş Bırakılamaz!")]
        [Display(Name = "Şifre")]

        public string Password { get; set; }



    }
    public class RegisterViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]

        [Required(ErrorMessage = "Boş Bırakılamaz!")]
        [Display(Name = "Şifre")]

        public string Password { get; set; }
        public string Role { get; set; }

        [Required(ErrorMessage = "Boş Bırakılamaz!")]
        [Display(Name = "İsim")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Boş Bırakılamaz!")]
        [Display(Name = "Soy isim")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Boş Bırakılamaz!")]
        [Display(Name = "Mail")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Boş Bırakılamaz!")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        

     
    }
}
