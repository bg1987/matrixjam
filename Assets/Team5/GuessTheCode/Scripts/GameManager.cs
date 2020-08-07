using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.Team5
{
    public class GameManager : MonoBehaviour
    {
        public Text display;
        
        public CodeLetter[] letters;
        public Button[] doors;

        private Data _data;

        public void Init(Data data)
        {
            _data = data;
            
            var panel = _data.letters.ToCharArray();
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i].Init(this, panel[i].ToString());
            }
            
            Reset();
        }

        private string _guess; 
        
        public void Clicked(CodeLetter letter)
        {
            letter.button.interactable = false;
            _guess += letter.label.text;
            if (_guess.Length == 5)
            {
                for (int i = 0; i < _data.codes.Length; i++)
                {
                    if (_guess == _data.codes[i])
                    {
                        doors[i].interactable = true;
                    }
                }
            }
            if (_guess.Length > 5)
            {
                Reset();
            }
            display.text = _guess;
        }

        public void Reset()
        {
            _guess = "";
            foreach (var letter in letters)
            {
                letter.button.interactable = true;
            }
            
            foreach (var door in doors)
            {
                door.interactable = false;
            }
        }
    }

    [System.Serializable]
    public struct Data
    {
        public string[] codes;
        public string letters;
    }
}
