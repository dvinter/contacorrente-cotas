//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AlesCotas.Infra.Data.Context
//{
//    //Scaffold só funciona com esse factory
//    //public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//    //{
//    //    public ApplicationDbContext CreateDbContext(string[] args)
//    //    {
//    //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//    //        optionsBuilder.UseSqlServer("Server=localhost;Database=ales_cotas;Trusted_Connection=True;");

//    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//    {
//        public ApplicationDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//            optionsBuilder.UseSqlServer("Server=localhost;Database=ales_cotas2;Trusted_Connection=True;");

//            return new ApplicationDbContext(optionsBuilder.Options);
//        }
//    }
//            //return new ApplicationDbContext(optionsBuilder.Options);
//}
    

