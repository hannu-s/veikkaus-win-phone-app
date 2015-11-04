using System;

namespace Veikkaus_app
{
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(MatchData matchData)
        {
            MatchData = matchData;
        }

        public MatchData MatchData { get; private set; }
    }
}
