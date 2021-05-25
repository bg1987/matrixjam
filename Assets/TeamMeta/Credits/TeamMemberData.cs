using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [System.Serializable]
    public struct TeamMemberData
    {
        public string name;
        public string[] roles;

        public TeamMemberData(string name, string[] roles)
        {
            this.name = name;
            this.roles = roles;
        }
    }
}
