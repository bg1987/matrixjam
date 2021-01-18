using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [System.Serializable]
    public class MatrixTravelHistory
    {
        Dictionary<int, int> gamesIndexToVisits = new Dictionary<int, int>();
        Dictionary<MatrixPortData,int> entrancesToVisits = new Dictionary<MatrixPortData, int>();
        Dictionary<MatrixPortData,int> exitsToVisits = new Dictionary<MatrixPortData, int>();
        HashSet<int> completedGamesByIndex = new HashSet<int>();
        List<MatrixEdgeData> history = new List<MatrixEdgeData>();

        MatrixTravelHistorySaver matrixTravelHistorySaver = new MatrixTravelHistorySaver();

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
        void CountGame(int gameIndex)
        {
            if (gamesIndexToVisits.ContainsKey(gameIndex))
                gamesIndexToVisits[gameIndex] += 1;
            else
            {
                gamesIndexToVisits.Add(gameIndex, 1);
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
        public void AddTravel(MatrixPortData startPort,  MatrixPortData destinationPort)
        {
            if (startPort.id != -1)
            {
                CountExit(startPort);
            }
            if (startPort.nodeIndex != -1)
                completedGamesByIndex.Add(startPort.nodeIndex);

            CountEntrance(destinationPort);
            CountGame(destinationPort.nodeIndex);

            history.Add(new MatrixEdgeData(startPort, destinationPort));

            //matrixTravelHistorySaver.Save(history);
        }
        void AddTravel(MatrixEdgeData edge)
        {
            AddTravel(edge.startPort, edge.endPort);
        }
        public void AmendLastTravelDestinationPortId(int id)
        {
            MatrixEdgeData edge = history[history.Count - 1];
            edge.endPort.id = id;
            history[history.Count - 1] = edge;
            //matrixTravelHistorySaver.Save(history);
        }
        public int GetGameVisitCount(MatrixNodeData matrixNodeData)
        {
            if (!gamesIndexToVisits.ContainsKey(matrixNodeData.index))
                return 0;
            return gamesIndexToVisits[matrixNodeData.index];
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
            return gamesIndexToVisits.Count;
        }
        public int GetCompletedGamesCount()
        {
            return completedGamesByIndex.Count;
        }
        public int[] GetCompletedGamesCopy()
        {
            int[] completedGames = new int[completedGamesByIndex.Count];
            completedGamesByIndex.CopyTo(completedGames);

            return completedGames;
        }
        public void LoadFromDisk()
        {
            List<MatrixEdgeData> loadedHistory = matrixTravelHistorySaver.Load();
            if(loadedHistory == null)
            {
                Debug.Log("No saved history found");
                return;
            }
            Clear();
            foreach (var entry in loadedHistory)
            {
                history.Add(entry);
                AddTravel(entry);
            }
        }
        public bool IsPossibleToLoadFromDisk()
        {
            return matrixTravelHistorySaver.SaveDataExists();
        }
        void Clear()
        {
            gamesIndexToVisits.Clear();
            entrancesToVisits.Clear();
            exitsToVisits.Clear();
            completedGamesByIndex.Clear();
            history.Clear();
        }
    }
}