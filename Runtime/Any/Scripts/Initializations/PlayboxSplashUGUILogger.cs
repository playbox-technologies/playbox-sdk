using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Timer;

namespace Playbox
{
    [Serializable]
    public class ConsoleUGUIMessage
    {
        public string text = "";
        public float time = 5f;
        public bool hasDelete;

        public void Update()
        {
            time -= Time.deltaTime;
            
            hasDelete = time < 0f;
        }
    }

    [Serializable]
    public class MessagePool
    {
        [SerializeField]
        private List<ConsoleUGUIMessage> messages = new();

        public void PushMessage(ConsoleUGUIMessage message)
        {
            messages.Add(message);
        }

        public string GetString()
        {
            string output = "";

            foreach (ConsoleUGUIMessage message in messages)
            {
                output += message.text + "\n";
            }

            return output;
        }

        public void Update()
        {
            foreach (var item in messages)
            {
                item.Update();
            }
            
            List<ConsoleUGUIMessage> messagesToRemove = new();
            
            foreach (var item in messages)
            {
                if (item.hasDelete)
                {
                    messagesToRemove.Add(item);
                }
            }

            foreach (var item in messagesToRemove)
            {
                messages.Remove(item);
            }
        }
    }

    public class PlayboxSplashUGUILogger : PlayboxBehaviour
    {
        public static UnityAction<string> SplashEvent;

        [SerializeField]
        private string text = "";
        
        [SerializeField]
        MessagePool messagePool = new();
        
        [SerializeField]
        private GUIStyle style = new ();
        private Texture2D texture;

        private float splashTime = 25;
        
        private PlayboxTimer timer;
        private bool isEnabled = false;


        public override void Initialization()
        {
            Init();
        }

        private void Init()
        {
            SplashEvent += OnText;
            
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            style.fontStyle = FontStyle.Normal;
            style.fontSize = 36;
            style.normal.background = texture;
            
            timer = new PlayboxTimer();
            timer.initialTime = splashTime;

            timer.OnTimeRemaining += f =>
            {


            };
            
            timer.OnTimerStart += () =>
            {
                isEnabled = true;

            };
            
            timer.OnTimeOut += () =>
            {
                isEnabled = false;
            };
        }

        private void OnText(string text)
        {
            //this.text = text;
            messagePool.PushMessage(new ConsoleUGUIMessage { text = text });
            timer.Start();
        }

        private void OnGUI()
        {
            if(!isEnabled)
                return;
            
            messagePool.Update();

            var message = messagePool.GetString();
            
            var rect = style.CalcSize(new GUIContent(message));
            
            GUI.Label(new Rect(new Vector2(200,200), rect), message, style);
        }

        private void Update()
        {
            timer.Update(Time.deltaTime);
        }
    }
}