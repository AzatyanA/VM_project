using System.Data.Entity;


namespace Wpf_CoffeeMachine
{

    public class VM_Wallet_Class
    {
        public int Id { get; set; }
        public string Rating { get; set; }
        public int Rating_Value { get; set; }
        public int Quantity { get; set; }

    }


    public class VM_WalletContext : DbContext
    {
        public VM_WalletContext() : base("DefaultConnection")
        {

        }
        public DbSet<VM_Wallet_Class> VM_Wallet { get; set; }
    }
}
