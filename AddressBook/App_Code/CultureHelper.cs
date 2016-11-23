using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace AddressBook
{
  public static class CultureHelper
  {
    private static readonly string[] Cultures = { "ru", "en" };

    private static string ParseLanguage(string lang)
    {
      if (string.IsNullOrWhiteSpace(lang)) return Cultures[0];
      return !Cultures.Contains(lang) ? Cultures[0] : lang;
    }
    public static void ChangeCulture()
    {
      var cultureCookie = HttpContext.Current.Request.Cookies["lang"];
      var cultureName = cultureCookie?.Value;

      cultureName = ParseLanguage(cultureName);

      Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
      Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
    }

    public static void ChangeLanguage(string lang)
    {
      lang = ParseLanguage(lang);

      // Сохраняем выбранную культуру в куки
      var cookie = HttpContext.Current.Request.Cookies["lang"];
      if (cookie != null)
        cookie.Value = lang; // если куки уже установлено, то обновляем значение
      else
        cookie = new HttpCookie("lang") {HttpOnly = false, Value = lang, Expires = DateTime.Now.AddYears(1)};

      HttpContext.Current.Response.Cookies.Add(cookie);
    }
  }
}