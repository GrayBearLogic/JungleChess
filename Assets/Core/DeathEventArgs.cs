namespace JungleCore
{
    public delegate void DeathEventHandler(Game sender, DeathEventArgs e);
    
    public class DeathEventArgs
    {
        public DeathEventArgs(Rank rank, Side owner)
        {
            Rank  = rank;
            Owner = owner;
        }

        public Rank Rank { get; }
        public Side Owner { get; }
    }
}