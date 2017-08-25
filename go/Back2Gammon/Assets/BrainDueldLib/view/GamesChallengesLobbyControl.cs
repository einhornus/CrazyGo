
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.threads;

namespace BrainDuelsLib.view
{
    public interface GamesChallengesLobbyControl
    {
        void SetSelectUserCallback(Action<int> action);
        void SetOnEnterGameAsPlayerCallback(Action<Game> action);
        void SetOnEnterGameAsObserverCallback(Action<Game> action);
        void SetOnLeaveGameCallback(Action<Game> action);
        void Update(List<Game> games);
    }
}
