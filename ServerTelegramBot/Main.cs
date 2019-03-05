using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;


namespace ServerTelegramBot
{
    static class MainClass
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static NotifyIcon notify = new NotifyIcon();
        static ContextMenuStrip contextMenu = new ContextMenuStrip();
        static BackgroundWorker bw = null;
        static void Main()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Перезапуск", Properties.Resource.reload, MainClass_ClickReload).Name = "Reload";
            contextMenu.Items.Add("Закрыть", Properties.Resource.close, MainClass_ClickClose).Name = "Close";
            
            


            NotifyIcon notify = new NotifyIcon();
            notify.Icon = Properties.Resource.media;
            notify.Visible = true;
            notify.Text = "Server BotTelegram";
            notify.ContextMenuStrip = contextMenu;

            //Dialogs.ProxyParser parser = new Dialogs.ProxyParser("http://foxtools.ru/Proxy?country=US&al=True&am=True&ah=True&ahs=True&http=True&https=True");

            bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;//(new Dialogs.RootDialog()).Bw_DoWork;
            //bw.RunWorkerAsync();
            bw.RunWorkerAsync(new object[] { Properties.Settings.Default.ApiTelegramBot, Properties.Settings.Default.WebProxy });
            //bw.RunWorkerAsync(new object[] { Properties.Settings.Default.ApiTelegramBot, "http://67.205.132.241:1080/" });
            Application.Run();
        }
        static private async void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker; // получаем ссылку на класс вызвавший событие

            var arg = e.Argument as object[]; // получаем ключ из аргументов
            try
            {
                if (arg == null)
                {
                    await (new Dialogs.RootDialog(Properties.Settings.Default.NameTelegramBot)).MessageReceivedAsync(Properties.Settings.Default.ApiTelegramBot);
                }
                else
                {
                    await (new Dialogs.RootDialog(Properties.Settings.Default.NameTelegramBot)).MessageReceivedAsync(arg[0].ToString(), arg[1].ToString());
                }
            }
            catch (Dialogs.ProxyException ex)
            {
                //if (Convert.ToInt32(arg[1]) != config.Proxys.Count - 1) bw.RunWorkerAsync(new object[] { arg[0], Convert.ToInt32(arg[1]) + 1, arg[2] });
                //else MessageBox.Show("Ни одно прокси не оказалось рабочим!\n Возможно проблемы с интернетом!", "Ошибка");
                logger.Fatal(ex.Message +"  "+ex.InnerException);
                //MessageBox.Show(ex.Source + ex.StackTrace, ex.Message);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                logger.Fatal(ex.Message + "  " + ex.InnerException);
                //MessageBox.Show(ex.Source + ex.StackTrace, ex.Message);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message + "  " + ex.InnerException);
                //MessageBox.Show(ex.Source + ex.StackTrace, ex.Message);
            }
        }
        private static void MainClass_ClickReload(object sender, EventArgs e)
        {
          
        }

        private static void MainClass_ClickClose(object sender, EventArgs e)
        {
            notify.Dispose();
            Application.Exit();
            logger.Info("Закрытие приложения");
        }
    }
}
