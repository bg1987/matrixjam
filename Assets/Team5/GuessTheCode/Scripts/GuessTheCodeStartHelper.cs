namespace MatrixJam.Team5
{
    public class GuessTheCodeStartHelper : StartHelper
    {
        public GameManager Manager;
        
        public override void StartHelp(int num_ent)
        {
            // this is how the game starts
            Manager.Init(num_ent);
        }
    }
}
