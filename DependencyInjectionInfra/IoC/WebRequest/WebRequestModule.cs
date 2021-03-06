﻿using CustomInfra.Injector.Simple.Enums;
using CustomInfra.Injector.Simple.IoC;
using SimpleInjector;
using System;
using System.Web;

namespace CustomInfra.Injector.Simple.AspNet
{
    internal class WebRequestModule : IHttpModule
    {
        /// <summary>
        /// Overrides Begin and End Request for WebRequestLifeStyle objects
        /// </summary>
        /// <param name="httpApp"></param>
        public void Init(HttpApplication httpApp)
        {
            httpApp.BeginRequest += MvcApplication_BeginRequest;
            httpApp.EndRequest += MvcApplication_EndRequest;
        }

        /// <summary>
        /// Store Scope in current context of request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items.Add(IoCInfraLifeCycle.WebRequest, IoCInfra.BeginWebRequestScope());
        }

        /// <summary>
        /// Get scope from current content of request to dispose objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            var scope = GetScopeFromHttpContext();
            if(scope != null)
                scope.Dispose();

            HttpContext.Current.Items.Remove(IoCInfraLifeCycle.WebRequest);
        }

        public static Scope GetScopeFromHttpContext()
        {
            if (HttpContext.Current.Items.Contains(IoCInfraLifeCycle.WebRequest))
            {
                return HttpContext.Current.Items[IoCInfraLifeCycle.WebRequest] as Scope;
            }

            return null;
        }

        public void Dispose()
        {

        }
    }
}