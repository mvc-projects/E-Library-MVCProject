namespace MVCProjectVIdent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        email = c.String(),
                        fName = c.String(nullable: false),
                        lName = c.String(nullable: false),
                        image = c.String(),
                        birthDate = c.DateTime(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        copiesCount = c.Int(nullable: false),
                        availableCopies = c.Int(nullable: false),
                        title = c.String(nullable: false),
                        AutherId = c.Int(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        CategoryName = c.String(maxLength: 128),
                        cover = c.String(),
                        name = c.String(nullable: false),
                        source = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                        joinDate = c.DateTime(nullable: false),
                        publishDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Authors", t => t.AutherId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryName)
                .ForeignKey("dbo.Publishers", t => t.PublisherId, cascadeDelete: true)
                .Index(t => t.AutherId)
                .Index(t => t.PublisherId)
                .Index(t => t.CategoryName);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        name = c.String(nullable: false, maxLength: 128),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.name);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UserBooks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Memberid = c.String(maxLength: 128),
                        Bookid = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        startDate = c.DateTime(nullable: false),
                        returnDate = c.DateTime(),
                        deliveredDate = c.DateTime(nullable: false),
                        isDelivered = c.Boolean(nullable: false),
                        Employeeid = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.Employeeid)
                .ForeignKey("dbo.Books", t => t.Bookid, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.Memberid)
                .Index(t => t.Memberid)
                .Index(t => t.Bookid)
                .Index(t => t.Employeeid);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        fName = c.String(),
                        lName = c.String(),
                        image = c.Binary(),
                        birthDate = c.DateTime(),
                        JoinDate = c.DateTime(),
                        isDeleted = c.Boolean(nullable: false),
                        firstLogin = c.Boolean(nullable: false),
                        address = c.String(),
                        salary = c.Decimal(precision: 18, scale: 2),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        isSeen = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Title = c.String(),
                        Message = c.String(),
                        UserId = c.String(maxLength: 128),
                        ToUser = c.String(maxLength: 128),
                        ToGroup = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUser)
                .Index(t => t.UserId)
                .Index(t => t.ToUser);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        isBlock = c.Boolean(nullable: false),
                        endDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserBooks", "Memberid", "dbo.Members");
            DropForeignKey("dbo.Members", "id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserBooks", "Bookid", "dbo.Books");
            DropForeignKey("dbo.UserBooks", "Employeeid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "ToUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Books", "PublisherId", "dbo.Publishers");
            DropForeignKey("dbo.Books", "CategoryName", "dbo.Categories");
            DropForeignKey("dbo.Books", "AutherId", "dbo.Authors");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Members", new[] { "id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Notifications", new[] { "ToUser" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserBooks", new[] { "Employeeid" });
            DropIndex("dbo.UserBooks", new[] { "Bookid" });
            DropIndex("dbo.UserBooks", new[] { "Memberid" });
            DropIndex("dbo.Books", new[] { "CategoryName" });
            DropIndex("dbo.Books", new[] { "PublisherId" });
            DropIndex("dbo.Books", new[] { "AutherId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Members");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserBooks");
            DropTable("dbo.Publishers");
            DropTable("dbo.Categories");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
