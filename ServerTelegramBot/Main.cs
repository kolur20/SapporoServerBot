using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerTelegramBot
{
    static class MainClass
    {
        static NotifyIcon notify = new NotifyIcon();
        static ContextMenuStrip contextMenu = new ContextMenuStrip();
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

            
            Application.Run();
        }

        private static void MainClass_ClickReload(object sender, EventArgs e)
        {
          
        }

        private static void MainClass_ClickClose(object sender, EventArgs e)
        {
            //notify.Dispose();
            Application.Exit();

        }
    }
}
