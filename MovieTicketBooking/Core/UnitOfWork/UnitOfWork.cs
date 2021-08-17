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
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _appDbContext;
        private readonly IOptionsMonitor<JwtConfig> _optionsMonitor;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public IUserRepository User { get; private set; }
        public IAuthenRepository Authen { get; private set; }

        public  IRoleRepository Role { get; private set; }

        public IUserRoleRepository UserRole { get; private set; }

        public IAuditoriumRepository Auditorium { get; private set; }

        public IRowRepository Row { get; private set; }

        public ISeatRepository Seat { get; private set; }

        public ISeatTypeRepository SeatType { get; private set; }

        public IReservationRepository Reservation { get; private set;  }

        public IMovieRepository MovieRepository { get; private set; }

        public IScheduleMovieRepository ScheduleMovie { get; private set; }

        public UnitOfWork(UserManager<User> userManager,
                          RoleManager<Role> roleManager,
                          IEmailService emailService,
                          AppDbContext appDbContext,
                          IOptionsMonitor<JwtConfig> optionsMonitor,
                          TokenValidationParameters tokenValidationParameters
                           )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _appDbContext = appDbContext;
            _optionsMonitor = optionsMonitor;
            _tokenValidationParameters = tokenValidationParameters;
            User = new UserRepository(_appDbContext, _userManager, _roleManager, _emailService);
            Authen = new AuthenRepository(_appDbContext, _optionsMonitor, _userManager, _tokenValidationParameters);
            Role = new RoleRepository(_roleManager,_appDbContext,_userManager);
            UserRole = new UserRoleRepository(_userManager, _roleManager);
            Auditorium = new AuditoriumRepository(_appDbContext);
            Row = new RowRepository(_appDbContext);
            Seat = new SeatRepository(_appDbContext);
            SeatType = new SeatTypeRepository(_appDbContext);
            Reservation = new ReservationRepository(_appDbContext, _tokenValidationParameters);
           MovieRepository =new MovieRepository(_appDbContext);
            ScheduleMovie = new ScheduleMovieRepository(_appDbContext, MovieRepository);
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
