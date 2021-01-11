using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixTravelData
    {
        public Dictionary<MatrixNodeData, int> gameToVisits = new Dictionary<MatrixNodeData, int>();
        public Dictionary<MatrixPortData,int> entrancesToVisits = new Dictionary<MatrixPortData, int>();
        public Dictionary<MatrixPortData,int> exitsToVisits = new Dictionary<MatrixPortData, int>();

        public MatrixNodeData currentGame;
        public MatrixPortData enteredAt;

        public void CountGame(MatrixNodeData game)
        {
            if (gameToVisits.ContainsKey(game))
                gameToVisits[game] += 1;
            else
            {
                gameToVisits.Add(game, 1);
            }
        }
        public void CountEntrance(MatrixPortData port)
        {
            if (entrancesToVisits.ContainsKey(port))
                entrancesToVisits[port] += 1;
            else
            {
                entrancesToVisits.Add(port, 1);
            }
        }
        public void CountExit(MatrixPortData port)
        {
            if (exitsToVisits.ContainsKey(port))
                exitsToVisits[port] += 1;
            else
            {
                exitsToVisits.Add(port, 1);
            }
        }
    }
}
