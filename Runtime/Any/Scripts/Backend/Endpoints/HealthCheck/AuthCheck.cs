using System;
using Any.Scripts.Backend.Verificator;

namespace Playbox
{
    public class AuthCheck
    {
        public static async void Check(Action<string> callback)
        {
            var a = await HttpService.GetAsync("/health/auth");

            if (a.IsSuccess)
            {
                callback?.Invoke(a.Body);
            }
        }
    }
}