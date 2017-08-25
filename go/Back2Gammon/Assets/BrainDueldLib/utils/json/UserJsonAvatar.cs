
using System.Collections.Generic; using System;

using System.Text;
using BrainDuelsLib.model.entities;
using System;

namespace BrainDuelsLib.utils.json
{
    public class UserJsonAvatar : JsonAvatar<User>
    {
        public String login;
        public int rank;

        public override void CopyData(User user)
        {
            HandleNull(login, () => { user.login = login; });
            HandleNull(rank, () => { user.rank = rank;});
        }
    }
}
