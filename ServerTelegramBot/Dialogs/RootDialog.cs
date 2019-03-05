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
        long ChatId = 0;
       
        
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
                await Bot.SetWebhookAsync("");
                //Reqests.Client.InitClient();
                //if (ListChatId != null) await StartWithListChat(Convert.ToInt64(ListChatId));
            }
            catch (Exception)
            {
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

        

        private async void Bot_OnMessageEdited(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message.Type == MessageType.Text)
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
        }
    

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //throw new NotImplementedException();
            var message = e.Message;
            if (message.Type == MessageType.Text)
            {
                string msg = "";
                if (message.Text.Contains($"@HelperSapporo_bot"))
                {  msg = message.Text.Replace($"@HelperSapporo_bot", "");}
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
                            await Bot.SendTextMessageAsync(message.Chat.Id, $"Ваш id чата: {message.Chat.Id.ToString()}");
                            break;
                        }
                 
                    //case "/help":
                    //    {
                    //        await Help(message);
                    //        break;
                    //    }
                    case "Ироми":
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, String.Format("{0} на Ироми", 
                                CardEmul.Personal.PersonalForUsName(message.From.Username) ? "Подтверждено" : "Ошибка"));
                            
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
            await Bot.SendTextMessageAsync(message.Chat.Id, msg);
        }
        //private async Task GetSettings(Message message)
        //{
        //    string msg = $"Интервал времени запросов:   \n Точка старта:  " +
        //        $"\n Доступ к okdesk:   \n Доступ к API okdesk  ";
        //    await Bot.SendTextMessageAsync(message.Chat.Id, msg);
        //}
        //private async Task Help(Message message)
        //{
        //    string msg = $"/roll - случайное число до 100 \n" +
        //                    $"/settimeout < int > -время обновления окдеска\n" +
        //                    $"/setnumber < int > -номер с которого мониторить\n" +
        //                    $"/settoken < string > -установка токена для окдеска\n" +
        //                    $"/setokdesk < string > -хостинг на окдеск\n" +
        //                    $"/mstart - старт мониторинга\n" +
        //                    $"/mstop - остановка мониторинга\n" +
        //                    $"/getsettings - просмотр настроек\n" +
        //                    $"/unlocking - разблокировать бота для работы\n" +
        //                    $"/blocking - заблокировать бота";
        //    await Bot.SendTextMessageAsync(message.Chat.Id, msg);
        //}
        
        //private void StartReqests()
        //{
        //    timer = new System.Timers.Timer();
        //    timer.AutoReset = true;
        //    //timer.Interval = System.Convert.ToInt32(config.Timeout) * 1000;
        //    //timer.Elapsed += Timer_Elapsed;
        //    timer.Start();
        //}
        //private void StopReqest()
        //{
        //    timer.Stop();
        //    timer.Dispose();
        //}
        //private async void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    //await Bot.SendTextMessageAsync(ChatId, $"Все хорошо");
        //    var ansver = Reqests.Client.RunAsync(_Number).GetAwaiter().GetResult();
        //    try
        //    {
        //        if (ansver == null)
        //        {
        //            await Bot.SendTextMessageAsync(ChatId, $"Ошибка распознавания ответа на заявку: {_Number}");
        //            _Number++;
        //        }
        //        else if (ansver.errors != null)
        //        {
        //            //Console.WriteLine(ansver.errors);
        //        }
        //        else
        //        {
        //            await Bot.SendTextMessageAsync(ChatId, $"Новая заявка! \n Номер: {ansver.id} {ansver.title} \n Клиент: {ansver.author.name} \n " +
        //                $"Описание: {ansver.description.Replace($"<br>","")}");
        //            _Number++;
        //            Timer_Elapsed(sender, e);
        //        }
        //        config = (Config)XmlSerialize.Deserialaze(config);
        //        config.FirstNumber = _Number.ToString();
        //        XmlSerialize.Serialize(config);
        //    }
        //    catch (System.Net.Http.HttpRequestException)
        //    {
        //        return;
        //        //throw new System.Net.Http.HttpRequestException("Ошибка при запросе");
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //        //throw new Exception("Ошибка при запросе", ex.InnerException);
        //    }
        //}
        

/*
private async Task SetTimeDialogAfter(IDialogContext context, IAwaitable<int> result)
{
   int timeout = await result;
   await context.PostAsync($"Таймаут запросов установлен: {timeout}");
   Reqests.Config.Timer = timeout;
   context.Wait(MessageReceivedAsync);
   await Task.CompletedTask;
}
private async Task SetTokenDialogAfter(IDialogContext context, IAwaitable<string> result)
{
   string token = await result;
   await context.PostAsync($"Токен установлен: {token}");
   Reqests.Config.Api = token;
   context.Wait(MessageReceivedAsync);
   await Task.CompletedTask;
}
private async Task SetOkdeskDialogAfter(IDialogContext context, IAwaitable<string> result)
{
   string Okdesk = await result;
   await context.PostAsync($"Okdesk установлен: {Okdesk}");
   Reqests.Config.Okdesk = Okdesk;
   context.Wait(MessageReceivedAsync);
   await Task.CompletedTask;
}
private async Task SetNumberDialogAfter(IDialogContext context, IAwaitable<int> result)
{
   int number = await result;
   await context.PostAsync($"Начальный номер заявок установлен: {number}");
   Reqests.Config.Number = number;
   context.Wait(MessageReceivedAsync);
   await Task.CompletedTask;
}
private async Task ProcessErrors(IDialogContext context)
{
   await context.PostAsync($"Введены некорректные данные");
   context.Fail(new Exception("Введены некорректные данные"));
}
private async Task BotIsBlockingErrors(IDialogContext context)
{
   await context.PostAsync($"Для работы разблокируйте бота");
   context.Fail(new Exception("Бот заблокирован"));
}
*/
    }
}