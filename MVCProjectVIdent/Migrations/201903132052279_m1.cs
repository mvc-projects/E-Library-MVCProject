namespace MVCProjectVIdent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "CategoryName", "dbo.Categories");
            DropIndex("dbo.Books", new[] { "CategoryName" });
            RenameColumn(table: "dbo.Books", name: "CategoryName", newName: "CategoryId");
            DropPrimaryKey("dbo.Categories");
            AddColumn("dbo.Categories", "id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Authors", "birthDate", c => c.DateTime());
            AlterColumn("dbo.Books", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Categories", "name", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Categories", "id");
            CreateIndex("dbo.Books", "CategoryId");
            AddForeignKey("dbo.Books", "CategoryId", "dbo.Categories", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Books", new[] { "CategoryId" });
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Books", "CategoryId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Authors", "birthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Categories", "id");
            AddPrimaryKey("dbo.Categories", "name");
            RenameColumn(table: "dbo.Books", name: "CategoryId", newName: "CategoryName");
            CreateIndex("dbo.Books", "CategoryName");
            AddForeignKey("dbo.Books", "CategoryName", "dbo.Categories", "name");
        }
    }
}
