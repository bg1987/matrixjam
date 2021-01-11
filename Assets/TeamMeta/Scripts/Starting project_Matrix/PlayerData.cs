using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam
{
    public class PlayerData : MonoBehaviour
    {
        public int current_level;
        int complete_levels = 0;
        int skip_levels_num = 0;
        LinkedList<Connection> been_connections = new LinkedList<Connection>();
        LinkedList<int> skip_level = new LinkedList<int>();
        static PlayerData data;
        public static PlayerData Data
        {
            get
            {
                if (data == null)
                {
                    data = GameObject.FindObjectOfType<PlayerData>();
                }
                return data;
            }
        }
        public int NumGames
        {
            get
            {
                return MatrixTraveler.Instance.matrixGraphData.nodes.Count;
            }
        }
        public bool AddLevel(int finish_level, int ent, int exit)
        {
            //return true if total matrix game is over
            //add the level and the parh to memory
            if (!HaveLevel(finish_level))
            {
                been_connections.AddLast(new Connection(finish_level, ent, finish_level, exit));
                complete_levels++;
                if (complete_levels + skip_levels_num >= MatrixTraveler.Instance.matrixGraphData.nodes.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //ToDo Decide if
            //1) there's a need to remember what connections have been visited
            //2) there's a need for rememebering connections visit history
            //For now going with 2)
            been_connections.AddLast(new Connection(finish_level, ent, finish_level, exit));
            return false;
        }
        public bool HaveLevel(int have_this)
        {
            //return true if the given level was completed before
            foreach (Connection con in been_connections)
            {
                if (con.scene_from == have_this)
                {
                    return true;
                }
            }
            return false;
        }
        public int NumLevels
        {
            get
            {
                return complete_levels;
            }
        }
        public Connection LastCon
        {
            //return the last connection (i.e level) the player has finished. 
            //get LastCon.scene_from for last level finished, LastCon.portal_from for entrence used, and Lastcon.portal_to for exit used.
            get
            {
                LinkedListNode<Connection> lastConnectionNode = been_connections.Last;

                if (lastConnectionNode == null)
                    return new Connection(-1, -1, -1, -1);
                return been_connections.Last.Value;
            }
        }
        public int PastBeen(int lvl)
        {
            //return the number of times this level have been completed before
            int ans = 0;
            foreach(Connection con in been_connections)
            {
                if(con.scene_from == lvl)
                {
                    ans++;
                }
               
            }
            return ans;
        }
    }
}
