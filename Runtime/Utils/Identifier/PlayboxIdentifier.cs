using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Utils.Identifier
{
    public static class PlayboxIdentifier
    {
        public static string GetUID => GetActualGUID();

        private static readonly string GuidKey = "Playbox_GUID";
        
        public static string NewGUID()
        {
            JObject obj = new JObject();
            
            obj["generated_uuid"] = Guid.NewGuid().ToString();
            obj["device_id"] = SystemInfo.deviceUniqueIdentifier;
            obj["device_model"] = SystemInfo.deviceModel;
            obj["device_name"] = SystemInfo.deviceName;
            obj["register_date"] = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            
            return obj.ToString();
        }

        public static string GetActualGUID()
        {
            if (PlayerPrefs.HasKey(GuidKey))
            {
                return PlayerPrefs.GetString(GuidKey);
            }
            
            var guid = NewGUID();
            
            PlayerPrefs.SetString(GuidKey, guid);
            
            PlayerPrefs.Save();
            
            return guid;
        }
    }
}