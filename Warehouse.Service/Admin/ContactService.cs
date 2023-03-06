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
using Warehouse.ViewModels.Common;

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
                        Date = b.Date,
                        isShow = (bool)b.isShow


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
        public async Task<ContactViewModel> GetContactFormListViewAsync(long contactId)
        {

            var predicate = PredicateBuilder.New<Data.Contact>(true);/*AND*/
            predicate.And(a => a.Id == contactId);
            var contact = await _getContactListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return contact;
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
            var messageContact = _context.Contact.FirstOrDefault(x => x.Id == contactId);
            messageContact.isShow = true;
            _context.SaveChanges();
            return contact;
        }
        public async Task<ContactViewModel> GetIncomingMessageModelAsync(int contactId)
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
        public async Task<ServiceCallResult> DeleteContactFormAsync(long contactId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var contact = await _context.Contact.FirstOrDefaultAsync(a => a.Id == contactId).ConfigureAwait(false);
            if (contact == null)
            {
                callResult.ErrorMessages.Add("Böyle iletişim formu bulunamadı.");
                return callResult;
            }



            _context.Contact.Remove(contact);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetContactFormListViewAsync(contact.Id).ConfigureAwait(false);
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
