namespace TicketRetailSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoprawkiWModelu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "Transaction_Id", "dbo.Transactions");
            DropIndex("dbo.Tickets", new[] { "Transaction_Id" });
            AddColumn("dbo.Transactions", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Transactions", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Tickets", "Transaction_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "Transaction_Id");
            AddForeignKey("dbo.Tickets", "Transaction_Id", "dbo.Transactions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Transaction_Id", "dbo.Transactions");
            DropIndex("dbo.Tickets", new[] { "Transaction_Id" });
            AlterColumn("dbo.Tickets", "Transaction_Id", c => c.Int());
            DropColumn("dbo.Transactions", "TotalPrice");
            DropColumn("dbo.Transactions", "Date");
            CreateIndex("dbo.Tickets", "Transaction_Id");
            AddForeignKey("dbo.Tickets", "Transaction_Id", "dbo.Transactions", "Id");
        }
    }
}
