namespace TicketRetailSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardTypeDictionaries",
                c => new
                    {
                        CardTypeId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CardTypeId);
            
            CreateTable(
                "dbo.DiscountTypeDictionaries",
                c => new
                    {
                        DiscountTypeId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        PercentDiscount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiscountTypeId);
            
            CreateTable(
                "dbo.PaymentTypeDictionaries",
                c => new
                    {
                        PaymentTypeId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentTypeId);
            
            CreateTable(
                "dbo.TicketBindingDictionaries",
                c => new
                    {
                        TicketBindingId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TicketBindingId);
            
            CreateTable(
                "dbo.TicketPeriodDictionaries",
                c => new
                    {
                        TicketPeriodId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.TicketPeriodId);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssuedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFromDate = c.DateTime(),
                        ValidToDate = c.DateTime(),
                        Card_Id = c.Int(),
                        TicketType_Id = c.Int(nullable: false),
                        Transaction_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TransportCards", t => t.Card_Id)
                .ForeignKey("dbo.TicketTypes", t => t.TicketType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id)
                .Index(t => t.Card_Id)
                .Index(t => t.TicketType_Id)
                .Index(t => t.Transaction_Id);
            
            CreateTable(
                "dbo.TransportCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardType = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        PersonalId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketPeriod = c.Int(nullable: false),
                        Zone = c.Int(nullable: false),
                        DiscountType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ZoneDictionaries",
                c => new
                    {
                        ZoneId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.Tickets", "TicketType_Id", "dbo.TicketTypes");
            DropForeignKey("dbo.TransportCards", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Tickets", "Card_Id", "dbo.TransportCards");
            DropIndex("dbo.TransportCards", new[] { "User_Id" });
            DropIndex("dbo.Tickets", new[] { "Transaction_Id" });
            DropIndex("dbo.Tickets", new[] { "TicketType_Id" });
            DropIndex("dbo.Tickets", new[] { "Card_Id" });
            DropTable("dbo.ZoneDictionaries");
            DropTable("dbo.Transactions");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.Users");
            DropTable("dbo.TransportCards");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketPeriodDictionaries");
            DropTable("dbo.TicketBindingDictionaries");
            DropTable("dbo.PaymentTypeDictionaries");
            DropTable("dbo.DiscountTypeDictionaries");
            DropTable("dbo.CardTypeDictionaries");
        }
    }
}
