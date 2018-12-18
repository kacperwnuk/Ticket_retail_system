namespace TicketRetailSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoprawkiWModelu1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketTypes", "TicketBinding", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketTypes", "TicketBinding");
        }
    }
}
