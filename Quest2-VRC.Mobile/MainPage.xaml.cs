using static Quest2_VRC.Mobile.PacketSender;

namespace Quest2_VRC.Mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
            string HMDBat = "HMDBat";
            VRChatMessage Msg_emu1 = new VRChatMessage(HMDBat, $"Clicked {count} times");
            SendPacket(Msg_emu1);
            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}