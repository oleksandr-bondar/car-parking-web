using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarParking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WebAppCarParking.Controllers
{
    [Produces("application/json")]
    [Route("api/parking")]
    public class ParkingController : Controller
    {
        // GET: api/Parking
        [HttpGet]
        public ActionResult GetHelp()
        {
            return Content(Helper.GetHelpParking());
        }

        // GET: api/Parking/free
        [HttpGet("free")]
        public ActionResult GetCountFree()
        {
            JObject json = new JObject(
                new JProperty("FreePlace", new JValue(Parking.Instance.CountFreeParkingSpace)));

            return Content(json.ToString());
        }

        // GET: api/Parking/occupied
        [HttpGet("occupied")]
        public ActionResult GetCountOccupied()
        {
            JObject json = new JObject(
                new JProperty("OccupiedPlace", new JValue(Parking.Instance.CountOccupiedParkingSpace)));

            return Content(json.ToString());
        }

        // GET: api/Parking/balance
        [HttpGet("balance")]
        public ActionResult GetBalance()
        {
            JObject json = new JObject(
                new JProperty("Balance", new JValue(Parking.Instance.Balance)));

            return Content(json.ToString());
        }

        // GET: api/Parking/general
        [HttpGet("all")]
        public ActionResult Get()
        {
            JObject json = new JObject
            {
                { "FreePlace", new JValue(Parking.Instance.CountFreeParkingSpace) },
                { "OccupiedPlace", new JValue(Parking.Instance.CountOccupiedParkingSpace) },
                { "Balance", new JValue(Parking.Instance.Balance) }
            };

            return Content(json.ToString());
        }
    }
}
