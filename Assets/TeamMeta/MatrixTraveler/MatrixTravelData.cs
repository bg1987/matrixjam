using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixTravelData
    {
        Dictionary<MatrixNodeData, int> gameToVisits = new Dictionary<MatrixNodeData, int>();
        Dictionary<MatrixPortData,int> entrancesToVisits = new Dictionary<MatrixPortData, int>();
        Dictionary<MatrixPortData,int> exitsToVisits = new Dictionary<MatrixPortData, int>();
        HashSet<int> completedGamesByIndex = new HashSet<int>();
        List<MatrixEdgeData> history = new List<MatrixEdgeData>();

        public bool TryGetLastTravel(out MatrixEdgeData matrixEdgeData)
        {
            if (history.Count == 0)
            {
                matrixEdgeData = new MatrixEdgeData(new MatrixPortData(-1, -1), new MatrixPortData(-1, -1));
                return false;
            }
            else
            {
                matrixEdgeData = history[history.Count - 1];
                return true;
            }
        }
        public IReadOnlyList<MatrixEdgeData> GetHistory()
        {
            return history.AsReadOnly();
        }
        void CountGame(MatrixNodeData game)
        {
            if (gameToVisits.ContainsKey(game))
                gameToVisits[game] += 1;
            else
            {
                gameToVisits.Add(game, 1);
            }
        }
        void CountEntrance(MatrixPortData port)
        {
            if (entrancesToVisits.ContainsKey(port))
                entrancesToVisits[port] += 1;
            else
            {
                entrancesToVisits.Add(port, 1);
            }
        }
        void CountExit(MatrixPortData port)
        {
            if (exitsToVisits.ContainsKey(port))
                exitsToVisits[port] += 1;
            else
            {
                exitsToVisits.Add(port, 1);
            }
        }
        public void AddTravel(MatrixPortData start,  MatrixPortData destinationPort, MatrixNodeData destinationGame)
        {
            if (start.id != -1)
            {
                CountExit(start);
            }
            else
            {
                //ToDo See if this will be a necessary thing to check or if GetVisitedGamesCount() is enough
                completedGamesByIndex.Add(start.nodeIndex);
            }
            CountEntrance(destinationPort);
            CountGame(destinationGame);


            history.Add(new MatrixEdgeData(start, destinationPort));
        }
        public void AmendLastTravelDestinationPortId(int id)
        {
            MatrixEdgeData edge = history[history.Count - 1];
            edge.endPort.id = id;
            history[history.Count - 1] = edge;
        }
        public int GetGameVisitCount(MatrixNodeData matrixNodeData)
        {
            if (!gameToVisits.ContainsKey(matrixNodeData))
                return 0;
            return gameToVisits[matrixNodeData];
        }
        public int GetEntranceVisitCount(MatrixPortData matrixPortData)
        {
            if (!entrancesToVisits.ContainsKey(matrixPortData))
                return 0;
            return entrancesToVisits[matrixPortData];
        }
        public int GetExitVisitCount(MatrixPortData matrixPortData)
        {
            if (!exitsToVisits.ContainsKey(matrixPortData))
                return 0;
            return exitsToVisits[matrixPortData];
        }
        public int GetVisitedGamesCount()
        {
            return gameToVisits.Count;
        }
    }
}
