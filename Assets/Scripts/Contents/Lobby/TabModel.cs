using Sh.Contents.Common;

namespace Sh.Contents.Lobby
{
    public class TabModel
    {
        public LobbyUiManager Manager { get; set; }

        internal TabType TabType { get; set; }

        public string Text { get; set; }
    }
}