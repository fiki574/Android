namespace PopcornTimeRemote
{
    public class Response : ReceivePacket, IPacket
    {
        public void Handle()
        {
            Position = 0;
            string response = ReadS();
            if(response == "con_success")
            {
                MainActivity.handler.Components(true);
                MainActivity.handler.ShowAlert("Connected!", "INFO");
            }
            else if(response == "unk_cmd")
                MainActivity.handler.ShowAlert("Unknown command!", "ERROR");
            else if (response == "exception")
                MainActivity.handler.ShowAlert("An exception occured on PC executable when processing your request!", "ERROR");
        }
    }
}