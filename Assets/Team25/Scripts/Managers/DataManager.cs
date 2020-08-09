using UnityEngine;

namespace MatrixJam.Team25.Scripts.Managers
{
    public class DataManager : MonoBehaviour
    {
        [HideInInspector] public float fartForce, stink;
        [HideInInspector] public GameObject gamePrefab;
        public int round;
        [HideInInspector] public int pooped;
        [HideInInspector] public bool tutorial;
        [HideInInspector] public bool intro;
        [HideInInspector] public float totalScore;
        
        public void ResetGame()
        {
            var currentGame = GameObject.FindWithTag("GameController");
            Destroy(currentGame);
            var newGame = Instantiate(gamePrefab);
            newGame.SetActive(true);
        }
    }
}
