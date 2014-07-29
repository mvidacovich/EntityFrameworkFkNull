namespace EntityFrameworkFkNull.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkFkNull.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            var users = context.Set<User>();
            var user = new User
            {
                Name = "Foo"
            };
            users.AddOrUpdate(x => x.Name, user);
            user = users.SingleOrDefault(x => x.Name == "Foo") ?? user;

            var tickets = context.Set<Ticket>();
            tickets.AddOrUpdate(x=>x.Name, new Ticket
            {
                Name = "Bar",
                Owner = user,
            });
        }
    }
}
