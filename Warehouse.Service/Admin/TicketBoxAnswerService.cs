using LinqKit;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class TicketBoxAnswerService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        string name = System.Web.HttpContext.Current.User.Identity.Name;
        private readonly Users user;
        public TicketBoxAnswerService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
            user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
        }
        public async Task<TicketBoxAnswerViewModel> GetTicketAnswerShowModelAsync(int ticketId)
        {
            var ticket = _context.Tickets.Where(x=>x.Id == ticketId).FirstOrDefault();
            var message = await (from p in _context.TicketAnswers
                                 where p.TicketId == ticketId
                                 select new TicketBoxAnswerViewModel()
                                 {

                                     Date = p.Date,
                                     Subject = p.Subject,
                                     Id = p.Id,
                                     isShow = p.isShow,
                                     Message = ticket.Message,
                                     SenderName = p.AnsweringPerson, 
                                     AnswerMessage = p.Message

                                 }).FirstOrDefaultAsync();
            if (user.Role != "admin" && user.Role != "admin2")
            {
                var messageContact = _context.TicketAnswers.FirstOrDefault(x => x.TicketId == ticketId);
                if (messageContact != null)
                {
                    messageContact.isShow = true;
                    _context.SaveChanges();
                }
              
            }
          
            return message;
        }
        private IQueryable<TicketBoxShowViewModel> _getTicketListIQueryable(Expression<Func<Data.Tickets, bool>> expr)
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
                        isAnswer = b.isAnswer,






                    });

        }
        public async Task<TicketBoxShowViewModel> GetTicketBoxListViewAsync(long ticketId)
        {

            var predicate = PredicateBuilder.New<Data.Tickets>(true);/*AND*/
            predicate.And(a => a.Id == ticketId);
            var message = await _getTicketListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return message;
        }

        public async Task<TicketBoxAnswerAddViewModel> GetTicketAnswerViewModelAsync(int ticketId)
        {
            var result = await (from p in _context.Tickets
                                 where p.Id == ticketId
                                 select new TicketBoxAnswerAddViewModel()
                                 {

                                     ComingMessage = p.Message,
                                     TicketId= ticketId,
                                     Subject = p.Subject,
                                     SenderName = p.FullName,
                                     Date = p.Date,
                                     isAnswer = p.isAnswer,
                                     UserId = (long)p.UserId

                                 }).FirstOrDefaultAsync();
            

            return result;
        }
        public async Task<ServiceCallResult> AnswerTicketAsync(TicketBoxAnswerAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var ticketAnswer = new TicketAnswers()
            {

                Subject = model.Subject,
                AnsweringPerson = user.Name + " " +user.Surname,
                Date = DateTime.Now,
                isShow = false,
                Message = model.AnswerMessage,
                TicketId = model.TicketId,
                UserId = model.UserId
               





            };




            _context.TicketAnswers.Add(ticketAnswer);

            var ticket = _context.Tickets.Where(x => x.Id == model.TicketId).FirstOrDefault();
            ticket.isAnswer = true;
            ticket.isShow = true;

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetTicketBoxListViewAsync(model.TicketId).ConfigureAwait(false);
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
