﻿using CamundaInsurance.Data;
using CamundaInsurance.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services
{
    public class IdentityService : ServiceBase
    {
        private readonly SignInManager<User> signInManager;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ApplicationDbContext context;

        public IdentityService(SignInManager<User> signInManager, AuthenticationStateProvider authenticationStateProvider, ApplicationDbContext context)
        {
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<SContentResponce<User>> GetCurrentUserAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated == false)
            {
                return Error<User>("User is not authenticated");
            }
            var user = await context.Users.FirstOrDefaultAsync(v=>v.UserName == authState.User.Identity.Name);
            if(user == null)
            {
                return Error<User>("User is not found");             
            }
            return Ok(user);
        }

        public async Task<SResponce> UpdateUserInfoAsync(User user)
        {
            if(user == null)
            {
                return Error("User not found");
            }
            var result = Tools.ValidateModel(user);
            if(result != null)
            {
                return Error(result.Select(v => v.ErrorMessage).ToArray());
            }

            user.NormalizedUserName = user.UserName.ToUpper();
            context.Attach(user);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
