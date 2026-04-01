using System;
using Newtonsoft.Json.Linq;
using Playbox.Backend.Service;

namespace Playbox.Backend.Endpoints.HealthCheck
{
    public static class HealthCheck
    {
        public static async void Check(Action<bool> callback)
        {
           var a = await HttpService.GetAsync("/health");

           if (a.IsSuccess)
           {
               var result = JObject.Parse(a.Body);
               
               string status = result["status"]?.ToString();

               if (status == "ok")
               {
                   callback?.Invoke(true);
                   
                   return;
               }
               else
               {
                   callback?.Invoke(false);
                   return;
               }
           }
           else
           {
               callback?.Invoke(false);
               return;
           }
        }
    }
}