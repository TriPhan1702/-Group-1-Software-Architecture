using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs.Account;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class AccountsController : ApiController
    {
        public AccountServices AccountServices;

        public AccountsController()
        {
            AccountServices = new AccountServices();
        }
        
        //Return list of all active user account
        // GET /api/accounts
        public IHttpActionResult GetAccounts()
        {
            var account = AccountServices.GetAllActiveAccount();
            if (!account.Any())
            {
                return NotFound();
            }

            return Ok(account.Select(Mapper.Map<Account, AccountDto>));
        }

        //Return a list of all accounts as admin
        //GET /api/admin/accounts
        [HttpGet]
        [Route("api/admin/accounts")]
        public IHttpActionResult GetAccountAdmin()
        {
            var accounts = AccountServices.GetAllAccount();
            if (!accounts.Any())
            {
                return NotFound();
            }

            return Ok(accounts.Select(Mapper.Map<Account, AccountDtoForAdmin>));
        }

        //Return a AccountDto
        //Get /api/accounts/id
        [HttpGet]
        public IHttpActionResult GetAccount(int id)
        {
            var account = AccountServices.GetAccountById(id);

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

            var newAccount = AccountServices.CreateAccount(account);

            if (newAccount == null)
            {
                return Conflict();
            }
            
            return Created(new Uri(Request.RequestUri + "/" + AccountServices.GetAccountId(newAccount)), Mapper.Map<Account, AccountDto>(newAccount));
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

            var account = AccountServices.Login(loginAccountDto);

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
            var account = AccountServices.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }

            account = AccountServices.ChangeAccountStatus(account);

            if (account == null)
            {
                return Conflict();
            }

            return Ok(Mapper.Map<Account, AccountDto>(account));
        }
    }
}
