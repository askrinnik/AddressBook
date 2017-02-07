using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace AddressBook.Ang
{
  internal static class CustomExceptionLogger
  {
    public static HttpResponseMessage LogAndCreateResponse(Exception exception, HttpRequestMessage request)
    {
      var errorGuid = Guid.NewGuid();
      //log into some logger
      //LogManager.GetCurrentClassLogger().Error(errorGuid + ": " + exception);

      var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
      {
        //Content = new StringContent($"Internal Server Error <{errorGuid}>. Please, call to the administrator."),
        Content = new StringContent($"Internal Server Error\n{exception}"),
        RequestMessage = request
      };
      return response;
    }
  }

  /// <summary>
  /// Exception Filter
  /// Using: see section Registering Exception Filters
  /// https://www.asp.net/web-api/overview/error-handling/exception-handling
  /// There are a number of cases that exception filters can’t handle. For example:
  ///  - Exceptions thrown from controller constructors.
  ///  - Exceptions thrown from message handlers.
  ///  - Exceptions thrown during routing.
  ///  - Exceptions thrown during response content serialization.
  /// </summary>
  public class LogExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(HttpActionExecutedContext context)
    {
      context.Response = CustomExceptionLogger.LogAndCreateResponse(context.Exception, context.Request);
    }

  }

  /// <summary>
  /// The best approach
  /// https://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
  /// Using: config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());
  /// </summary>
  internal class CustomExceptionHandler : ExceptionHandler
  {
    public override void Handle(ExceptionHandlerContext context)
    {
      context.Result = new LogExceptionResult(context);
    }

    private class LogExceptionResult : IHttpActionResult
    {
      private readonly ExceptionHandlerContext _context;

      public LogExceptionResult(ExceptionHandlerContext context)
      {
        _context = context;
      }

      public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
      {
        return Task.FromResult(CustomExceptionLogger.LogAndCreateResponse(_context.Exception, _context.ExceptionContext.Request));
      }
    }
  }

  /// <summary>
  /// Just logger. Has no ability to rewrite response
  /// https://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
  /// Using: config.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());
  /// </summary>
  public class TraceExceptionLogger : ExceptionLogger
  {
    public override void Log(ExceptionLoggerContext context)
    {
      var response = CustomExceptionLogger.LogAndCreateResponse(context.Exception, context.ExceptionContext.Request);
    }
  }
}