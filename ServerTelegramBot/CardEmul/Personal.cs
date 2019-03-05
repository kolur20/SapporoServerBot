namespace CardEmul
{
    static class Personal
    {
        
        static public bool PersonalForUsName(string usName)
        {
            if (usName == ServerTelegramBot.Properties.Settings.Default.UserName)
            {
                foreach (var i in ServerTelegramBot.Properties.Settings.Default.TrackCard)
                    KeyboardSend.KeyDown(i);
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.Enter);
                return true;
            }
            else return false;
            

        }
        }
}
