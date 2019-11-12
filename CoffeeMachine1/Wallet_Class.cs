using System.Data.Entity;


namespace Wpf_CoffeeMachine
{

    public class Wallet_Class
    {
        public int Id { get; set; }
        public string Rating { get; set; }
        public int Rating_Value { get; set; }
        public int Quantity { get; set; }
        
    }


    public class WalletContext : DbContext
    {
        public WalletContext() : base("DefaultConnection")
        {

        }
        public DbSet<Wallet_Class> Wallet { get; set; }
    }
}
