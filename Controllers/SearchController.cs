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
using Microsoft.Extensions.Configuration;
using System.Text;

namespace WebApplication5.Controllers
{

  [Route("[controller]")]
  public class SearchController : Controller
  {
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public SearchController(IHttpContextAccessor HttpContextAccessor, IDataService DataService)
    {
      dataService  = DataService;

      context         = HttpContextAccessor.HttpContext;
      remoteIpAddress = context.Connection?.RemoteIpAddress?.ToString(); 
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private IDataService    dataService;
    private HttpContext     context;
    private String          remoteIpAddress;
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /search
    [HttpGet]
    public String GetPing()
    {
      return "[{ data: 'search', currentTime: '" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss")  + "} ]";
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /search/foodNames?match=pork%20pie&maxResults=100
    [HttpGet("foodNames")]
    public String GetMatchingFoodNames([FromQuery] String match, [FromQuery] String sources, [FromQuery] String maxResults)
    {

      dataService.initilizeSQLCommand("search.GetFoodNames");
      //
      dataService.setupSQLCommand("Match", SqlDbType.VarChar, 255, System.Net.WebUtility.HtmlDecode(match));
      //
      if (sources != null)
      {
        dataService.setupSQLCommand("Sources", SqlDbType.VarChar, 255, System.Net.WebUtility.HtmlDecode(sources));
      }
      //
      if (maxResults != null)
      {
        dataService.setupSQLCommand("MaxResults", SqlDbType.Int, null, Int32.Parse(maxResults));
      }
      //
      return dataService.processSQLCommand();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /search/foods?ids=7582,8288
    [HttpGet("foods")]
    public String GetFoods([FromQuery] String ids)
    {

      dataService.initilizeSQLCommand("search.GetFoods");

      dataService.setupSQLCommand("IDs", SqlDbType.VarChar, -1, ids);

      return dataService.processSQLCommand();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /search/units
    [HttpGet("units")]
    public String GetUnits([FromQuery] Boolean ShowFullDetails = true)
    {

      dataService.initilizeSQLCommand("search.GetUnits");

      dataService.setupSQLCommand("ShowFullDetails", SqlDbType.Bit, null, ShowFullDetails);

      return dataService.processSQLCommand();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /search/foodCalculations?foodID=7582&amount=125&unitName=g
    [HttpGet("foodCalculations")]
    public String GetFoodEntryCalculation([FromQuery] String FoodID, [FromQuery] String Amount, [FromQuery] String UnitName)
    {

      Double amount = Double.Parse(Amount);

      dataService.initilizeSQLCommand("search.GetFoodEntryCalculation");

      dataService.setupSQLCommand("FoodID", SqlDbType.Int, null, FoodID);
      dataService.setupSQLCommand("Amount", SqlDbType.Float, null, amount);
      dataService.setupSQLCommand("UnitName", SqlDbType.VarChar, 255, UnitName);

      return dataService.processSQLCommand();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  }
}
