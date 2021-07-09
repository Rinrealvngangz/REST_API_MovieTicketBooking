using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.IConfiguration;
using Core.IRepository;
using Core.Repository;
using Microsoft.AspNetCore.Identity;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Data.Entities;
using NETCore.MailKit.Core;

namespace Core.UnitOfWork
{
   public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _appDbContext;
        public  IUserRepository User { get; private set; }

        public UnitOfWork(UserManager<User> userManager,
                          IEmailService emailService,
                          AppDbContext appDbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
            _appDbContext = appDbContext;
            User = new UserRepository(_appDbContext, _userManager, _emailService);
        }
       

        public async Task CompleteAsync()
        {
          await  _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
