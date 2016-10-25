using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

using WebApplication5.Services;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Text;

namespace WebApplication5.Controllers
{

  [Route("[controller]")]
  public class SecurityController : Controller
  {
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public SecurityController(IHttpContextAccessor HttpContextAccessor, IDataService DataService)
    {
      dataService     = DataService;

      context         = HttpContextAccessor.HttpContext;
      remoteIpAddress = context.Connection?.RemoteIpAddress?.ToString();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private IDataService    dataService;
    private HttpContext     context;
    private String          remoteIpAddress;
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private String DecodeBASE64(String Input)
    {

      Byte[] encodedDataAsBytes = Convert.FromBase64String(Input);

      String returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);

      return returnValue;

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private String EncodeBASE64(String Input)
    {

      Byte[] encodedDataAsBytes = Encoding.UTF8.GetBytes(Input);

      String returnValue = Convert.ToBase64String(encodedDataAsBytes);

      return returnValue;

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /security
    [HttpGet]
    public String GetPing()
    {
      return "[{ data: 'security', currentTime: '" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss") + "'} ]";
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /security/anonymousUsers/authorizations
    [HttpGet("anonymousUsers/authorizations")]
    public String GetAuthorizationForAnonymousUser()
    {

      dataService.initilizeSQLCommand("Security.GetAuthorizationForAnonymousUser");

      dataService.setupSQLCommand("IPAddress", SqlDbType.VarChar, 255, remoteIpAddress);

      return dataService.processSQLCommand();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /security/existingUsers/authorizations
    [HttpGet("existingUsers/authorizations")]
    public String GetAuthorizationForExistingUser()
    {

      String[] data;
      String authorization = this.context.Request.Headers["AUTHORIZATION"];

      if (!String.IsNullOrWhiteSpace(authorization))
      {

        data = this.DecodeBASE64(authorization.Replace("Basic ", "")).Split(':');

        dataService.initilizeSQLCommand("Security.GetAuthorizationForExistingUser");

        dataService.setupSQLCommand("EMail",     SqlDbType.VarChar, 255, data[0]);
        dataService.setupSQLCommand("Password",  SqlDbType.VarChar, 255, data[1]);
        dataService.setupSQLCommand("IPAddress", SqlDbType.VarChar, 255, remoteIpAddress);

        return dataService.processSQLCommand();

      }

      return null;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /security/newUsers/authorizations
    [HttpGet("newUsers/authorizations")]
    public String GetAuthorizationForNewUser()
    {

      String[] data;
      String authorization = this.context.Request.Headers["AUTHORIZATION"];

      if (!String.IsNullOrWhiteSpace(authorization))
      {

        data = this.DecodeBASE64(authorization.Replace("Basic ", "")).Split(':');

        dataService.initilizeSQLCommand("Security.GetAuthorizationForNewUser");

        dataService.setupSQLCommand("EMail",     SqlDbType.VarChar, 255, data[0]);
        dataService.setupSQLCommand("Password",  SqlDbType.VarChar, 255, data[1]);
        dataService.setupSQLCommand("IPAddress", SqlDbType.VarChar, 255, remoteIpAddress);

        return dataService.processSQLCommand();

      }

      return null;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /security/existingUsers/authorizations?session=
    [HttpGet("registeringUsers/authorizations")]
    public String GetAuthorizationForRegisteringUser([FromQuery] String Session)
    {

      String[] data;
      String authorization = this.context.Request.Headers["AUTHORIZATION"];

      if (!String.IsNullOrWhiteSpace(authorization))
      {

        data = this.DecodeBASE64(authorization.Replace("Basic ", "")).Split(':');

        dataService.initilizeSQLCommand("Security.GetAuthorizationForExistingUser");

        dataService.setupSQLCommand("EMail",     SqlDbType.VarChar, 255, data[0]);
        dataService.setupSQLCommand("Password",  SqlDbType.VarChar, 255, data[1]);
        dataService.setupSQLCommand("IPAddress", SqlDbType.VarChar, 255, remoteIpAddress);

        return dataService.processSQLCommand();

      }

      return null;

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /security/disavows?session=
    [HttpGet("disavows")]
    public String GetDisavows([FromQuery] String Session)
    {

      String[] data;
      String authorization = this.context.Request.Headers["AUTHORIZATION"];
      Guid  sessionGUID    = Guid.Parse(Session);


      if (!String.IsNullOrWhiteSpace(authorization))
      {

        data = this.DecodeBASE64(authorization).Replace("Basic ", "").Split(':');

        dataService.initilizeSQLCommand("Security.GetDisavows");

        dataService.setupSQLCommand("SessionGuid", SqlDbType.VarChar, 255, sessionGUID);
        dataService.setupSQLCommand("EMail",       SqlDbType.VarChar, 255, data[0]);
        dataService.setupSQLCommand("IPAddress",   SqlDbType.VarChar, 255, remoteIpAddress);

        return dataService.processSQLCommand();

      }

      return null;

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  }
}
