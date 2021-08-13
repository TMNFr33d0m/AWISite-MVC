using Attanaya_Warrior_Institute.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Attanaya_Warrior_Institute.Startup))]
namespace Attanaya_Warrior_Institute
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }

        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Member"))
            {
                var role = new IdentityRole {Name = "Member"};
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Instructor"))
            {
                var role = new IdentityRole {Name = "Instructor"};
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Staff"))
            {
                var role = new IdentityRole {Name = "Staff"};
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole {Name = "Manager"};
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Owner"))
            {
                var role = new IdentityRole {Name = "Owner"};
                roleManager.Create(role);
            }
        }
    }
}