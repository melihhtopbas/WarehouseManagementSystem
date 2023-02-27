using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;

namespace Warehouse.Service.Admin
{
    public class ContactService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public ContactService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<ContactViewModel> _getContactListIQueryable(Expression<Func<Data.Contact, bool>> expr)
        {
            return (from b in _context.Contact.AsExpandable().Where(expr)
                    select new ContactViewModel()
                    {
                        Phone = b.Phone,
                        Email = b.Mail,
                        FullName = b.FullName,
                        Message = b.Message,
                        Subject = b.Subject,
                        Id = b.Id,
                        Date = b.Date

                    });
        }

        public IQueryable<ContactViewModel> GetContactListIQueryable(ContactViewModel contactViewModel)
        {
            var predicate = PredicateBuilder.New<Data.Contact>(true);/*AND*/
            if (!string.IsNullOrWhiteSpace(contactViewModel.FullName))
            {
                predicate.And(a => a.FullName.Contains(contactViewModel.FullName));
            }
            if (!string.IsNullOrWhiteSpace(contactViewModel.Subject))
            {
                predicate.And(a => a.Subject.Contains(contactViewModel.Subject));
            }

            return _getContactListIQueryable(predicate);
        }
        public async Task<ContactViewModel> GetContactDetailModelAsync(int contactId)
        {
            var contact = await (from p in _context.Contact
                                 where p.Id == contactId
                                 select new ContactViewModel()
                                 {
                                     FullName = p.FullName,
                                     Date = p.Date,
                                     Phone = p.Phone,
                                     Subject = p.Subject,
                                     Email = p.Mail,
                                     Message = p.Message

                                 }).FirstOrDefaultAsync();
            return contact;
        }
    }
}
