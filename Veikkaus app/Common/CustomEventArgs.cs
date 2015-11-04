using System;
using Veikkaus_app.JsonObjects;

namespace Veikkaus_app.Common
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
