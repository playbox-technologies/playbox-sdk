using System;
using Any.Scripts.Backend.Verificator;
using Newtonsoft.Json.Linq;

namespace Playbox
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