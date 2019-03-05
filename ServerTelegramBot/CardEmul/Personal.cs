namespace CardEmul
{
    static class Personal
    {
        //static Dictionary<string, string> persList = new Dictionary<string, string>{
        //    { "Grin_K","885531" }
        //};
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

            //if (persList.Keys.Contains(usName))
            //{
            //    foreach (var i in persList[usName])
            //        KeyboardSend.KeyDown(i);
            //    KeyboardSend.KeyDown(System.Windows.Forms.Keys.Enter);
            //    return true;
            //}
            //else return false;

        }
        }
}
