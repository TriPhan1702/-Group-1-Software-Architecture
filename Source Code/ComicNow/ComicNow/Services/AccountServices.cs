using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ComicNow.DTOs.Account;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class AccountServices
    {
        public ComicNowEntities Context;

        public AccountServices()
        {
            Context = new ComicNowEntities();
        }

        public List<Account> GetAllActiveAccount()
        {
            return (from account in Context.Accounts where account.IsActive select account).ToList();
        }

        public List<Account> GetAllAccount()
        {
            return Context.Accounts.ToList();
        }

        public Account GetActiveAccountById(int id)
        {
            return Context.Accounts.SingleOrDefault(account => account.IsActive && account.Id == id);
        }

        public Account GetAccountById(int id)
        {
            return Context.Accounts.SingleOrDefault(account => account.Id == id);
        }

        public int GetAccountId(Account account)
        {
            return account.Id;
        }

        public Account CreateAccount(RegisterAccountDto account)
        {
            var newAccount = new Account()
            {
                Username = account.Username,
                Password = account.Password,
                Email = account.Email,
                RoleId = 2,
                IsActive = true,
            };

            try
            {
                Context.Accounts.Add(newAccount);
                Context.SaveChanges();
                return newAccount;
            }
            //Username Already Existed
            catch (Exception)
            {
                return null;
            }
        }

        public Account Login(LoginAccountDto loginAccountDto)
        {

            return Context.Accounts.SingleOrDefault(a => a.Username.Equals(loginAccountDto.Username) && a.Password.Equals(loginAccountDto.Password) && a.IsActive);
        }

        public Account ChangeAccountStatus(Account account)
        {
            try
            {
                account.IsActive = !account.IsActive;
                Context.SaveChanges();
                return account;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Comic> ViewFavoriteList(Account account)
        {
            var favorites = account.Comics;

            return favorites.ToList();
        }

        public bool DeleteFavorite(Account account, Comic comic)
        {
            try
            {
                account.Comics.Remove(comic);
                Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}