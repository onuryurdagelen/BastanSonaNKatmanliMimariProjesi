using Castle.DynamicProxy;
using Core.Extensions;
using Core.IoC;
using Core.Utilities.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Secured
{
    public class SecuredAspect:MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredAspect()
        {
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public SecuredAspect(string roles)
        {
            _roles = roles.Split(",");
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }
        protected override void OnBefore(IInvocation invocation)
        {

            if (_roles != null)
            {
                var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

                foreach (var role in _roles)
                {
                    if (roleClaims.Contains(role))
                    {
                        return; //Programa devam et.Burayı geç demektir.
                    }
                }
                throw new Exception("İşlem için yetkiniz bulunmamaktadır.");
            }
            else
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
                if (claims.Count > 0) //Token var ise kontrolü yapılır
                {
                    return; //Token var ise burayı geç demektir.
                }
                throw new Exception("İşlem için yetkiniz bulunmamaktadır.");
            }

            
        }
    }
}
