using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs.Account;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class AccountsController : ApiController
    {
        public ComicNowEntities Context;

        public AccountsController()
        {
            Context = new ComicNowEntities();
        }
        
        //Return list of all active user account
        // GET /api/accounts
        public IHttpActionResult GetAccounts()
        {
            IEnumerable<AccountDto> accounts = from acc in Context.Accounts
                where acc.Role.Id != Constants.AdminRoleId
                select new AccountDto(){Id = acc.Id, Username = acc.Username, RoleId = acc.RoleId};

            if (accounts.Any())
            {
                return Ok(accounts);
            }

            return NotFound();
        }

        //Return a list of all accounts as admin
        //GET /api/admin/accounts
        public IHttpActionResult GetAccountAdmin()
        {
            var accounts = Context.Accounts;
            if (!accounts.Any())
            {
                return NotFound();
            }

            return Ok(accounts.ToList().Select(Mapper.Map<Account, AccountDto>));
        }

        //Return a AccountDto
        //Get /api/accounts/id
        [HttpGet]
        public IHttpActionResult GetAccount(int id)
        {
            var account = Context.Accounts.SingleOrDefault(a => a.Id == id && a.IsActive);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Account, AccountDto>(account));
        }
        
        //Register new account
        //POST /api/accounts
        [HttpPost]
        public IHttpActionResult CreateAccount(RegisterAccountDto account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newAccount = new Account()
            {
                Username = account.Username,
                Password = account.Password,
                Email = account.Email,
                RoleId = 2,
                IsActive = true,
            };

            Context.Accounts.Add(newAccount);
            try
            {
                Context.SaveChanges();
                var newAccountDto = Mapper.Map<Account, AccountDto>(newAccount);
                return Created(new Uri(Request.RequestUri + "/" + newAccount.Id), newAccountDto);
            }
            //Username Already Existed
            catch (Exception)
            {
                return Conflict();
            }
        }
        
        // POST /api/accounts/login
        [HttpPost]
        [Route("api/accounts/login")]
        public IHttpActionResult Login(LoginAccountDto loginAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var account = Context.Accounts.SingleOrDefault(a => a.Username.Equals(loginAccountDto.Username) && a.Password.Equals(loginAccountDto.Password) && a.IsActive);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Account, AccountDto>(account));
        }

        //PUT /api/accounts/changeAccountStatus/id
        //Activate/Deactivate an Account
        [HttpPut]
        [Route("api/accounts/changeAccountStatus/{id}")]
        public IHttpActionResult ChangeAccountStatus(int id)
        {
            var account = Context.Accounts.SingleOrDefault(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            account.IsActive = !account.IsActive;
            Context.SaveChanges();

            return Ok(Mapper.Map<Account, AccountDto>(account));
        }
    }
}
