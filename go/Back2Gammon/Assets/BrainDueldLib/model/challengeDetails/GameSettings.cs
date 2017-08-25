
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.model.challengeDetails
{
    public abstract class GameSettings
    {
        public abstract string Save();
        public abstract void Load(string details);

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(GetType())) //-V3115
            {
                GameSettings gs = (GameSettings)obj;
                return gs.Save().Equals(this.Save());
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Save().GetHashCode();
        }
    }
}
