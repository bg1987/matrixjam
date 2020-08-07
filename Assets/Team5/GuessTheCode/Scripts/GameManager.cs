using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team5
{
    public class GameManager : MonoBehaviour
    {
        public Text display;
        
        public CodeLetter[] letters;
        public void Init(int num_ent)
        {
            // this is how the game starts
            Debug.Log("start: " + num_ent);

            var abc = new string[]
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "T", "S",
                "V", "W", "X", "Y", "Z"
            };
            foreach (var letter in letters)
            {
                letter.Init(this, abc[Random.Range(0, abc.Length)]);
            }
        }

        private string _guess; 
        
        public void Clicked(CodeLetter letter)
        {
            _guess += letter.label.text;
            display.text = _guess;
            if (_guess.Length > 5)
                display.text = "";
        }
    }
}
