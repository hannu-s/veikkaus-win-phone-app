using System;

namespace Veikkaus_app
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(Match match)
        {
            Match = match;
        }

        public Match Match { get; private set; }
    }
}
