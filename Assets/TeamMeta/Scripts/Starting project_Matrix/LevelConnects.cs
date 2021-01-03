using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam
{
    [System.Serializable]
    public struct LevelConnects
    {
        public Connection[] level_connects;
        public Connection FindConnect(int number, bool to)
        {
            //Bugged. Will be looked at next commit
            foreach (Connection con in level_connects)
            {
                if (to)
                {
                    if (con.portal_to == number)
                    {
                        return con;
                    }
                }
                else
                    if (con.portal_from == number)
                {
                    return con;
                }

            }

            return new Connection();
        }
    }
}

