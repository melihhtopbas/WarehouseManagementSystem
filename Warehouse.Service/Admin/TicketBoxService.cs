using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class TicketBoxService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        string name = System.Web.HttpContext.Current.User.Identity.Name;
        private readonly Users user;

        public TicketBoxService(WarehouseManagementSystemEntities1 context)
        {
            _context = context; 
            user = _context.Users.Where(x=>x.UserName == name).FirstOrDefault();    
        }
        private IQueryable<TicketBoxViewModel> _getTicketListIQueryable(Expression<Func<Data.Tickets, bool>> expr)
        {
            return (from b in _context.Tickets.AsExpandable().Where(expr)
                                .Where(x => x.UserId == user.Id)
                    select new TicketBoxViewModel()
                    {

                        Message = b.Message,
                        Subject = b.Subject,
                        Id = b.Id,
                        Date = b.Date,
                        isShow = b.isShow,
                        SenderName = b.FullName,
                        isAnswer = b.isAnswer,






                    });

        }

        public IQueryable<TicketBoxViewModel> GetTicketListIQueryable(TicketBoxViewModel contactViewModel)
        {
            var predicate = PredicateBuilder.New<Data.Tickets>(true);/*AND*/
            if (!string.IsNullOrWhiteSpace(contactViewModel.SearchName))
            {
                predicate.And(a => a.Subject.Contains(contactViewModel.SearchName) || a.FullName.Contains(contactViewModel.SearchName));
            }
         
            return _getTicketListIQueryable(predicate);
        }
        public async Task<TicketBoxViewModel> GetTicketBoxListViewAsync(long ticketId)
        {

            var predicate = PredicateBuilder.New<Data.Tickets>(true);/*AND*/
            predicate.And(a => a.Id == ticketId);
            var message = await _getTicketListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return message;
        }
        private IQueryable<TicketBoxShowViewModel> _getTicketListShowIQueryable(Expression<Func<Data.Tickets, bool>> expr)
        {
            return (from b in _context.Tickets.AsExpandable().Where(expr)
                                
                    select new TicketBoxShowViewModel()
                    {

                        Message = b.Message,
                        Subject = b.Subject,
                        Id = b.Id,
                        Date = b.Date,
                        isShow = b.isShow,
                        SenderName = b.FullName,
                        isAnswer= b.isAnswer,
                        





                    });

        }

        public IQueryable<TicketBoxShowViewModel> GetTicketListShowIQueryable(TicketBoxShowViewModel contactViewModel)
        {
            var predicate = PredicateBuilder.New<Data.Tickets>(true);/*AND*/
            if (!string.IsNullOrWhiteSpace(contactViewModel.SearchName))
            {
                predicate.And(a => a.Subject.Contains(contactViewModel.SearchName) || a.FullName.Contains(contactViewModel.SearchName));
            }

            return _getTicketListShowIQueryable(predicate);
        }
        public async Task<TicketBoxShowViewModel> GetTicketBoxListShowViewAsync(long ticketId)
        {

            var predicate = PredicateBuilder.New<Data.Tickets>(true);/*AND*/
            predicate.And(a => a.Id == ticketId);
            var message = await _getTicketListShowIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return message;
        }
        public async Task<TicketBoxViewModel> GetTicketBoxDetailModelAsync(int ticketId)
        {
            var message = await (from p in _context.Tickets
                                 where p.Id == ticketId
                                 select new TicketBoxViewModel()
                                 {
                                     
                                     Date = p.Date,
                                     Subject = p.Subject,
                                     Id = p.Id,
                                     isShow = p.isShow, 
                                     Message = p.Message,
                                      SenderName = p.FullName

                                 }).FirstOrDefaultAsync();
            if (user.Role == "admin" || user.Role == "admin2")
            {
                var messageContact = _context.Tickets.FirstOrDefault(x => x.Id == ticketId);
                messageContact.isShow = true;
                _context.SaveChanges();
            }
            
        
            
            return message;
        }
        public async Task<ServiceCallResult> AddTicketAsync(TicketBoxAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            

            var ticket = new Tickets()
            {

                Subject = model.Subject,
                Message = model.Message,    
                Date= DateTime.Now,
                UserId = user.Id,
                FullName = user.Name + " " +user.Surname,
                isAnswer = false,
                isShow = false,
                




            };




            _context.Tickets.Add(ticket);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetTicketBoxListViewAsync(ticket.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<ServiceCallResult> DeleteTicketBoxAsync(long ticketId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var message = await _context.Tickets.FirstOrDefaultAsync(a => a.Id == ticketId).ConfigureAwait(false);
            if (message == null)
            {
                callResult.ErrorMessages.Add("Böyle bir mesaj bulunamadı.");
                return callResult;
            }

            var ticketAnswers = _context.TicketAnswers.Where(x => x.TicketId == ticketId).ToList();
            foreach (var item in ticketAnswers)
            {
                _context.TicketAnswers.Remove(item);
            }


            _context.Tickets.Remove(message);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetTicketBoxListViewAsync(message.Id).ConfigureAwait(false);
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
