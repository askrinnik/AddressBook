using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Security;
using AddressBook.DataAccess;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace AddressBook
{
  public static class AuthConfig
  {
    public static void InitalizeWebSecurity()
    {
      // We don't use an initializer. All adding steps will be below
      Database.SetInitializer<UsersContext>(null);

      try
      {
        // Create the SimpleMembership database without Entity Framework migration schema
        using (var context = new UsersContext())
        {
          var databaseExists = context.Database.Exists(); // !!!! use variable, otherwise returns false
          if (!databaseExists)
            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
        }

        // create tables for users and roles
        WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", true);

        var roles = (SimpleRoleProvider)Roles.Provider;
        var membership = (SimpleMembershipProvider)Membership.Provider;

        if (!roles.RoleExists("Admin"))
          roles.CreateRole("Admin");

        if (!roles.RoleExists("User"))
          roles.CreateRole("User");

        if (membership.GetUser("Admin", false) == null)
          membership.CreateUserAndAccount("Admin", "SuperAdminPassword");

        if (!roles.GetRolesForUser("Admin").Contains("Admin"))
          roles.AddUsersToRoles(new[] { "Admin" }, new[] { "Admin" });
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(
          "The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588",
          ex);
      }
    }

    public static void RegisterAuth()
    {
      // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
      // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

      //OAuthWebSecurity.RegisterMicrosoftClient(
      //    clientId: "",
      //    clientSecret: "");

//      OAuthWebSecurity.RegisterTwitterClient(
//          consumerKey: "VWyuTCPcEL4MlIVGE9Rsw",
//          consumerSecret: "S2DBd7BFxa3WBOT1fCykl9gJgOmzAFEg3yKxJ8TBxU");

      OAuthWebSecurity.RegisterFacebookClient(
          appId: "575772215804525",
          appSecret: "7e29a8ba8b06a2f4993c98fb603eb077");

      OAuthWebSecurity.RegisterGoogleClient();

      OAuthWebSecurity.RegisterClient(
       client: new VKontakteAuthenticationClient("3905893", "SHhIiURN8uoRdwhT05Hl"),
       displayName: "ВКонтакте", // надпись на кнопке
       extraData: null);
    }
  }
}
