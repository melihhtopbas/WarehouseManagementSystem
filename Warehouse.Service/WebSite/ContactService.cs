using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ActivationContext;
using Warehouse.ViewModels.Common;
using Warehouse.Data;
using Warehouse.ViewModels.WebSite;

namespace Warehouse.Service.WebSite
{
    public class ContactService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public ContactService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }

        public ServiceCallResult AddContactForm(ContactViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            var contact = new Contact()
            {
                Phone = model.Phone,
                FullName = model.FullName,
                Mail = model.Email,
                Message = model.Message,
                Subject = model.Subject,
                Date = DateTime.Now,
                isShow = false
            };
            _context.Contact.Add(contact);
            callResult.SuccessMessages.Add("Formunuz başarıyla kaydedildi");
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    dbtransaction.Commit();
                    callResult.Success = true;

                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
    }
}
