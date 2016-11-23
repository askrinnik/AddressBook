using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DotNetOpenAuth.AspNet;

namespace AddressBook
{
  public class VKontakteAuthenticationClient : IAuthenticationClient
  {
    public string AppId;
    public string AppSecret;
    private string _redirectUri;

    public VKontakteAuthenticationClient(string appId, string appSecret)
    {
      AppId = appId;
      AppSecret = appSecret;
    }

    string IAuthenticationClient.ProviderName => "vkontakte";

    void IAuthenticationClient.RequestAuthentication(HttpContextBase context, Uri returnUrl)
    {
      var appId = AppId;
      _redirectUri = context.Server.UrlEncode(returnUrl.ToString());
      var address = $"https://oauth.vk.com/authorize?client_id={appId}&redirect_uri={_redirectUri}&response_type=code";

      HttpContext.Current.Response.Redirect(address, false);
    }

    AuthenticationResult IAuthenticationClient.VerifyAuthentication(HttpContextBase context)
    {
      try
      {
        var code = context.Request["code"];

        var address = $"https://oauth.vk.com/access_token?client_id={AppId}&client_secret={AppSecret}&code={code}&redirect_uri={_redirectUri}";

        var response = Load(address);
        var accessToken = DeserializeJson<AccessToken>(response);

        address = $"https://api.vk.com/method/users.get?uids={accessToken.user_id}&fields=photo_50";

        response = Load(address);
        var usersData = DeserializeJson<UsersData>(response);
        var userData = usersData.response.First();

        return new AuthenticationResult(
            true, (this as IAuthenticationClient).ProviderName, accessToken.user_id,
            userData.first_name + " " + userData.last_name,
            new Dictionary<string, string>());
      }
      catch (Exception ex)
      {
        return new AuthenticationResult(ex);
      }
    }

    class AccessToken
    {
      public string access_token = null;
      public string user_id = null;
    }

    class UserData
    {
      public string uid = null;
      public string first_name = null;
      public string last_name = null;
      public string photo_50 = null;
    }

    class UsersData
    {
      public UserData[] response = null;
    }
    public static string Load(string address)
    {
      var request = WebRequest.Create(address) as HttpWebRequest;
      using (var response = request.GetResponse() as HttpWebResponse)
      using (var reader = new StreamReader(response.GetResponseStream()))
        return reader.ReadToEnd();
    }

    public static T DeserializeJson<T>(string input)
    {
      var serializer = new JavaScriptSerializer();
      return serializer.Deserialize<T>(input);
    }
  }
}
