namespace EntityFrameworkFkNull.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LockedByUserId = c.Int(),
                        ETag = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Owner_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.LockedByUserId)
                .ForeignKey("dbo.User", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.LockedByUserId)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ticket", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Ticket", "LockedByUserId", "dbo.User");
            DropIndex("dbo.Ticket", new[] { "Owner_Id" });
            DropIndex("dbo.Ticket", new[] { "LockedByUserId" });
            DropTable("dbo.Ticket");
            DropTable("dbo.User");
        }
    }
}
