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
using Microsoft.Extensions.Options;
using Core.Config;
using Microsoft.IdentityModel.Tokens;
namespace Core.UnitOfWork
{
   public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _appDbContext;
        private readonly IOptionsMonitor<JwtConfig> _optionsMonitor;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public  IUserRepository User { get; private set; }
        public IAuthenRepository Authen { get; private set; }
        public UnitOfWork(UserManager<User> userManager,
                          IEmailService emailService,
                          AppDbContext appDbContext,
                          IOptionsMonitor<JwtConfig> optionsMonitor,
                          TokenValidationParameters tokenValidationParameters
                           )
        {
            _userManager = userManager;
            _emailService = emailService;
            _appDbContext = appDbContext;
            _optionsMonitor = optionsMonitor;
            _tokenValidationParameters = tokenValidationParameters;
            User = new UserRepository(_appDbContext, _userManager, _emailService);
            Authen = new AuthenRepository(_appDbContext, _optionsMonitor, _userManager, _tokenValidationParameters);
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
