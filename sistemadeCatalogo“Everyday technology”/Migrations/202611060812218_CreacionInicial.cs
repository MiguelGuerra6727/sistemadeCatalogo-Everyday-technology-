namespace sistemadeCatalogo_Everyday_technology_.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreacionInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PedidoItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PedidoId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Codigo = c.String(maxLength: 20),
                        Nombre = c.String(),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pedidos", t => t.PedidoId, cascadeDelete: true)
                .ForeignKey("dbo.Productos", t => t.ProductoId)
                .Index(t => t.PedidoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Pedidos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Cliente = c.String(nullable: false, maxLength: 100),
                        Telefono = c.String(nullable: false, maxLength: 20),
                        DireccionEnvio = c.String(nullable: false, maxLength: 500),
                        Estado = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 20),
                        Nombre = c.String(nullable: false, maxLength: 1000),
                        Descripcion = c.String(nullable: false, maxLength: 1200),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Imagen = c.String(maxLength: 200),
                        Stock = c.Int(nullable: false),
                        SeccionId = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Secciones", t => t.SeccionId)
                .Index(t => t.Codigo, unique: true, name: "Ix_Productos_Codigo")
                .Index(t => t.SeccionId);
            
            CreateTable(
                "dbo.Secciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        Imagen = c.String(maxLength: 200),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(nullable: false, maxLength: 150),
                        PasswordHash = c.String(nullable: false, maxLength: 300),
                        Salt = c.String(nullable: false, maxLength: 150),
                        RolId = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RolId)
                .Index(t => t.Correo, unique: true, name: "Ix_Usuarios_Correo")
                .Index(t => t.RolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "RolId", "dbo.Roles");
            DropForeignKey("dbo.PedidoItems", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.Productos", "SeccionId", "dbo.Secciones");
            DropForeignKey("dbo.PedidoItems", "PedidoId", "dbo.Pedidos");
            DropIndex("dbo.Usuarios", new[] { "RolId" });
            DropIndex("dbo.Usuarios", "Ix_Usuarios_Correo");
            DropIndex("dbo.Productos", new[] { "SeccionId" });
            DropIndex("dbo.Productos", "Ix_Productos_Codigo");
            DropIndex("dbo.PedidoItems", new[] { "ProductoId" });
            DropIndex("dbo.PedidoItems", new[] { "PedidoId" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Roles");
            DropTable("dbo.Secciones");
            DropTable("dbo.Productos");
            DropTable("dbo.Pedidos");
            DropTable("dbo.PedidoItems");
        }
    }
}
