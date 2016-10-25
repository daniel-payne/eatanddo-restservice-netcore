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
using Newtonsoft.Json;

namespace WebApplication5.Controllers
{

  [Route("[controller]")]
  public class DiaryController : Controller
  {
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public DiaryController(IHttpContextAccessor HttpContextAccessor, IDataService DataService)
    {
      dataService = DataService;

      context         = HttpContextAccessor.HttpContext;
      remoteIpAddress = context.Connection?.RemoteIpAddress?.ToString();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private IDataService dataService;
    private HttpContext  context;
    private String       remoteIpAddress;
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /diary
    [HttpGet]
    public String GetPing()
    {
      return "[{ data: 'diary', currentTime: '" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss") + "'} ]";
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /diary/days?session=6C1607EB-1CAA-47B7-A658-4D7434BAB02A&count=25
    [HttpGet("days")]
    public String GetDays([FromQuery] String session, [FromQuery] String DayCount = null, [FromQuery] String SkipDays = null)
    {
      Int32? count  = null;
      Int32? skip   = null;
      Guid sessionGUID = Guid.Parse(session);

      if ((DayCount != null) && (DayCount != String.Empty))
      {
        count = Int32.Parse(DayCount);
      }

      if ((SkipDays != null) && (SkipDays != String.Empty))
      {
        skip = Int32.Parse(SkipDays);
      }

      dataService.initilizeSQLCommand("diary.GetDays");

      dataService.setupSQLCommand("SessionGuid", SqlDbType.UniqueIdentifier, null, sessionGUID);

      if (count != null)
      {
        dataService.setupSQLCommand("DayCount", SqlDbType.Int, null, count);
      }

      if (skip != null)
      {
        dataService.setupSQLCommand("SkipDays", SqlDbType.Int, null, skip);
      }

      return dataService.processSQLCommand();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /diary/details?session=6C1607EB-1CAA-47B7-A658-4D7434BAB02A&dayDate=2016-06-29&edit=0
    [HttpGet("details")]
    public String GetDetails([FromQuery] String session, [FromQuery] String dayDates)
    {

      Guid   sessionGUID = Guid.Parse(session);

      dataService.initilizeSQLCommand("diary.GetDetails");

      dataService.setupSQLCommand("SessionGuid", SqlDbType.UniqueIdentifier, null, sessionGUID);
      dataService.setupSQLCommand("DayDates",    SqlDbType.Date,             null, dayDates   );

      return dataService.processSQLCommand();

    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // TEST /diary/foodEntries?session={session}&dayDate={dayDate}&mealtime={mealtime}
    [HttpPost("foodEntries")]
    public String PostFoodEntry([FromQuery] String session, [FromQuery] String dayDate, [FromQuery] String mealName, [FromBody] Dictionary<String, String> data)
    {

      Guid sessionGUID = Guid.Parse(session);

      dataService.initilizeSQLCommand("diary.PostFoodEntry");

      dataService.setupSQLCommand("SessionGuid", SqlDbType.UniqueIdentifier, null, sessionGUID);

      dataService.setupSQLCommand("DayDate",  SqlDbType.Date,     null, dayDate);
      dataService.setupSQLCommand("MealName", SqlDbType.VarChar,  255,  mealName);

      dataService.setupSQLCommand("FoodDescription",    SqlDbType.VarChar, 255, data["foodDescription"]  );
      dataService.setupSQLCommand("AmountDescription",  SqlDbType.VarChar, 255, data["amountDescription"]);

      if (data.ContainsKey("energy")         ) { dataService.setupSQLCommand("energyKiloJoulesPerEntry",    SqlDbType.Float, null, data["energy"]);         }
      if (data.ContainsKey("protein")        ) { dataService.setupSQLCommand("proteinGramsPerEntry",        SqlDbType.Float, null, data["protein"]);        }
      if (data.ContainsKey("carbohydrate")   ) { dataService.setupSQLCommand("carbohydrateGramsPerEntry",   SqlDbType.Float, null, data["carbohydrate"]);   }
      if (data.ContainsKey("sugar")          ) { dataService.setupSQLCommand("sugarGramsPerEntry",          SqlDbType.Float, null, data["sugar"]);          }
      if (data.ContainsKey("starch")         ) { dataService.setupSQLCommand("starchGramsPerEntry",         SqlDbType.Float, null, data["starch"]);         }
      if (data.ContainsKey("fat")            ) { dataService.setupSQLCommand("fatGramsPerEntry",            SqlDbType.Float, null, data["fat"]);            }
      if (data.ContainsKey("saturatedFat")   ) { dataService.setupSQLCommand("saturatedFatGramsPerEntry",   SqlDbType.Float, null, data["saturatedFat"]);   }
      if (data.ContainsKey("unsaturatedFat") ) { dataService.setupSQLCommand("unsaturatedFatGramsPerEntry", SqlDbType.Float, null, data["unsaturatedFat"]); }
      if (data.ContainsKey("cholesterol")    ) { dataService.setupSQLCommand("cholesterolGramsPerEntry",    SqlDbType.Float, null, data["cholesterol"]);    }
      if (data.ContainsKey("transFat")       ) { dataService.setupSQLCommand("transFatGramsPerEntry",       SqlDbType.Float, null, data["transFat"]);       }
      if (data.ContainsKey("dietaryFibre")   ) { dataService.setupSQLCommand("dietaryFibreGramsPerEntry",   SqlDbType.Float, null, data["dietaryFibre"]);   }
      if (data.ContainsKey("solubleFibre")   ) { dataService.setupSQLCommand("solubleFibreGramsPerEntry",   SqlDbType.Float, null, data["solubleFibre"]);   }
      if (data.ContainsKey("insolubleFibre") ) { dataService.setupSQLCommand("insolubleFibreGramsPerEntry", SqlDbType.Float, null, data["insolubleFibre"]); }
      if (data.ContainsKey("sodium")         ) { dataService.setupSQLCommand("sodiumGramsPerEntry",         SqlDbType.Float, null, data["sodium"]);         }
      if (data.ContainsKey("alcohol")        ) { dataService.setupSQLCommand("alcoholGramsPerEntry",        SqlDbType.Float, null, data["alcohol"]);        }

      return dataService.processSQLCommand();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  }
}
