using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class insert_gabinetes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Sérgio Meneguelli',1);
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Adilson Espindula',1); 
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Alcântaro Filho',1);   
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Alexandre Xambinho',1);
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Allan Ferreira',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Bispo Alves',1);       
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Callegari',1);         
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Camila Valadão',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Capitão Assumção',1);  
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Coronel Weliton',1);   
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Danilo Bahiense',1);   
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Dary Pagung',1);       
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Denninho Silva',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Dr. Bruno Resende',1); 
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Fabrício Gandini',1);  
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Hudson Leal',1);       
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Iriny Lopes',1);       
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Janete Sá',1);         
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. João Coser',1);        
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. José Esmeraldo',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Lucas Polese',1);      
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Lucas Scaramussa',1);  
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Marcelo Santos',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Mazinho dos Anjos',1); 
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Pablo Muribeca',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Raquel Lessa',1);      
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Theodorico Ferraço',1);
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Tyago Hoffmann',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Vandinho Leite',1);    
INSERT INTO [dbo].[Gabinetes] ([Name],[Ativo]) VALUES ('Gab. Dep. Zé Preto',1);          
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
