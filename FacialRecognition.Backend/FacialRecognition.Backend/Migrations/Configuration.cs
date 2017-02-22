namespace FacialRecognition.Backend.Migrations
{
    using DataObjects;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FacialRecognition.Backend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FacialRecognition.Backend.Models.MobileServiceContext context)
        {
            //context.IdentifiedUsers.AddOrUpdate(new IdentifiedUser { Id = "4c85546c-f484-4864-b022-1b784b5a012e", Email = "michael.watson@xamarin.com" });
        }
    }
}
