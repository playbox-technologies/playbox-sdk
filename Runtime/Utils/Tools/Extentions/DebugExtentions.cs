using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Playbox;
using UnityEngine;

namespace Utils.Tools.Extentions
{
    public static class DebugExtentions
    {
        private static Stack<string> prefixes = new();
        
        private static string currentPrefix = "";
        
        public static void BeginPrefixZone(string prefix)
        {
            if(!string.IsNullOrEmpty(currentPrefix)) prefixes.Push(currentPrefix);
         
            currentPrefix = prefix;
            
        }
        
        public static void EndPrefixZone()
        {
            if (prefixes.Count == 0)
            {
                currentPrefix = "";
                return;
            }
            
            if (prefixes.TryPop(out var prefix))
            {
                currentPrefix = prefix;
            }
            else
            {
                currentPrefix = "";
            }
        }

        public static void ClearPrefixes()
        {
            currentPrefix = "";
            prefixes.Clear();
        }
        
        [HideInCallstack]
        public static void PlayboxSplashLogUGUI(this object obj)
        {
            PlayboxSplashUGUILogger.SplashEvent?.Invoke(obj.ToString());

            obj.PbLog();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        [System.Diagnostics.DebuggerStepThrough]
        [HideInCallstack]
        private static void PlayboxLogger(
            object text,
            string predicate = "Playbox",
            string description = "",
            LogType logType = LogType.Log,
            [CallerFilePath] string file = "", 
            [CallerLineNumber] int line = 0)
        {
            string fileName = System.IO.Path.GetFileName(file);
            string prfx = string.IsNullOrEmpty(currentPrefix) ? "" : $"[{currentPrefix}] ";
            string desct = string.IsNullOrEmpty(description) ? "" : $" [{description}] ";
            string pred = string.IsNullOrEmpty(predicate) ? "" : $" [{predicate}] ";
            
            switch (logType)
            {
                case LogType.Error:
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    
                    Debug.Log($"[Playbox] {prfx}{pred}{desct}: {text}\n{fileName}:{line}");
                    
                    break;
                case LogType.Log:
                   
                    Debug.Log($"[Playbox] {prfx}{pred}{desct}: {text}\n{fileName}:{line}");
                    
                    break;
                case LogType.Exception:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        [HideInCallstack]
        public static void PbLog(this object text, string description = "",[CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
             PlayboxLogger(text,"Log",description, LogType.Log);
        }
        
        [HideInCallstack]
        public static void PbInfo(this object text, string description = "",[CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
             PlayboxLogger(text,"Info",description, LogType.Log);
        }
        
        [HideInCallstack]
        public static void PbWarning(this object text, string description = "",[CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            PlayboxLogger(text,"Warning",description, LogType.Warning);
        }
    }
}