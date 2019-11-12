using System.Data.Entity;

namespace Wpf_CoffeeMachine
{

    public class Menu_Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }


    public class MenuContext : DbContext
    {
        public MenuContext() : base("DefaultConnection")
        {

        }
        public DbSet<Menu_Class> Menu { get; set; }
    }
}
