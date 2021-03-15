using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiCatalogo.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) VALUES('Bebidas', 'http://www.macoratti.net/Imagens/1.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) VALUES('Lanches', 'http://www.macoratti.net/Imagens/2.jpg')");
            mb.Sql("INSERT INTO Categorias(Nome, ImageUrl) VALUES('Sobremesas', 'http://www.macoratti.net/Imagens/3.jpg')");

            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                "VALUE('Coca cola diet', 'Refrigerante de coola 350ml', 5.45, 'http://www.macoratti.net/Imagens/coca.jpg', 50, now()," +
                "(SELECT CategoriaId FROM Categorias WHERE Nome='Bebidas'))");

            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                "VALUE('Lanche de atum', 'Lanche de atum com maionese', 8.50, 'http://www.macoratti.net/Imagens/atum.jpg', 10, now()," +
                "(SELECT CategoriaId FROM Categorias WHERE Nome='Lanches'))");

            mb.Sql("INSERT INTO Produtos(Nome, Descricao, Preco, ImageUrl, Estoque, DataCadastro, CategoriaId) " +
                "VALUE('Pudim 100g', 'Pudim de leite condensado 100g', 6.75, 'http://www.macoratti.net/Imagens/pudim.jpg', 20, now()," +
                "(SELECT CategoriaId FROM Categorias WHERE Nome='Sobremesas'))");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Categorias");
            mb.Sql("DELETE FROM Produtos");
        }
    }
}
