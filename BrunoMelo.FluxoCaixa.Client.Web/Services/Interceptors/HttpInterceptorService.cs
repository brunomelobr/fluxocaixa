﻿using BrunoMelo.FluxoCaixa.Client.Web.Services.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace BrunoMelo.FluxoCaixa.Client.Web.Services.Interceptors
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly AuthenticationService _authenticationManager;
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackBar;

        public HttpInterceptorService(HttpClientInterceptor interceptor, AuthenticationService authenticationManager, NavigationManager navigationManager, ISnackbar snackBar)
        {
            _interceptor = interceptor;
            _authenticationManager = authenticationManager;
            _navigationManager = navigationManager;
            _snackBar = snackBar;
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("accounts"))
            {
                try
                {
                    var token = await _authenticationManager.TryRefreshToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        _snackBar.Add("Refreshed Token.", Severity.Success);
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _snackBar.Add("You are Logged Out.", Severity.Error);
                    await _authenticationManager.Logout();
                    _navigationManager.NavigateTo("/");
                }
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}