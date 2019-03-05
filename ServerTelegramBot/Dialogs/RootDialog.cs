using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace Dialogs
{
    class ProxyException : Exception
    {
        public ProxyException(string message) : base(message) { }
    }


    public class RootDialog
    {
        Telegram.Bot.TelegramBotClient Bot;
        string _botName = null;
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public RootDialog(string nameBot) => _botName = nameBot;
        public async Task MessageReceivedAsync(string key, string proxy = null, string ListChatId = null)
        {
            var webProxy = new WebProxy();




            if (proxy != null)
            {
                webProxy = new WebProxy(proxy);
                Bot = new Telegram.Bot.TelegramBotClient(key, webProxy);//, Client.client);
            }
            else Bot = new Telegram.Bot.TelegramBotClient(key);
            Bot.OnMessage += Bot_OnMessage;
            //Bot.OnMessageEdited += Bot_OnMessageEdited;
            Bot.StartReceiving();
            try
            {
                var webhook = "";
                logger.Info($"Установка webhook - {webhook}");
                await Bot.SetWebhookAsync(webhook);
                //Reqests.Client.InitClient();
                //if (ListChatId != null) await StartWithListChat(Convert.ToInt64(ListChatId));
            }
            catch (Exception)
            {
                logger.Error("Ошибка прокси");
                throw new ProxyException("Ошибка прокси");
            }

            //config = (Config)XmlSerialize.Deserialaze(config);
            /* 

           else
           {
               if (activity.Text == "/roll") //start roll 100
                   await Roll(context, activity);
               else if (activity.Text == "/getsettings")
                   await GetSettings(context, activity);
               else if (activity.Text == "/mstop")
                   await StopReqest(context);
               else if (activity.Text == "/mstart")
                   await StartReqests(context, activity);
               else if (activity.Text == "/help")
                   await Help(context);
               context.Wait(MessageReceivedAsync);
           }
           */
        }

        
        /// <summary>
        /// Изменение сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_OnMessageEdited(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Type == MessageType.Text)
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
        }
    
        /// <summary>
        /// Получение нового сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //throw new NotImplementedException();
            var message = e.Message;
            if (message.Type == MessageType.Text)
            {
                logger.Info($"Входящее сообщеие от @{message.From.Username}: {message.Text}");
                string msg = "";
                if (message.Text.Contains($"@{_botName}"))
                {  msg = message.Text.Replace($"@{_botName}", "");}
                else  { msg = message.Text;}

                if (msg == "/roll")
                {
                    await Roll(message);
                    return;
                }
                
                
                
                switch (msg)
                {
                    case "/chatid":
                        {
                            logger.Info($"Ответ отправлен: Ваш id чата: {message.Chat.Id.ToString()}");
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Ваш id чата: {message.Chat.Id.ToString()}");
                            break;
                        }
                   case "Ироми":
                        {
                            var ans = String.Format("{0} на Ироми", CardEmul.Personal.PersonalForUsName(message.From.Username) ? "Подтверждено" : "Ошибка");
                            logger.Info("Ответ отправлен: {0}",ans);
                            await Bot.SendTextMessageAsync(message.Chat.Id, ans);
                            
                            break;
                        }
                    default:
                        { break; }
                }
                //await Bot.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
        }

        private async Task Roll(Message message)
        {
            // в ответ на команду /roll выводим сообщение
            string msg = $"@{message.From.Username} - выкиныдывает {new Random().Next(1, 100).ToString()} из 100";
            //await Bot.SendTextMessageAsync(message.Chat.Id, msg, replyToMessageId: message.MessageId);
            logger.Info("Ответ отправлен: {0}", msg);
            await Bot.SendTextMessageAsync(message.Chat.Id, msg);
        }
        
        
    }
}