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
    [Route("api/transactions")]
    public class TransactionsController : Controller
    {
        // GET: api/Transactions
        [HttpGet]
        public ActionResult GetHelp()
        {
            return Content(Helper.GetHelpTransactions());
        }

        // GET: api/Transactions/log
        [HttpGet("log")]
        public ActionResult GetLog()
        {
            var log = Parking.Instance.GetTransactionLog();

            if (log.Count == 0)
                return BadRequest("Не вдалось прочитати файл або він є пустим.");

            JObject json = new JObject();
            JArray array = new JArray();

            foreach (var item in log)
            {
                array.Add(new JObject(
                    new JProperty(nameof(item.DateTime), new JValue(item.DateTime)),
                    new JProperty(nameof(item.Debited), new JValue(item.Debited))));
            }

            json.Add("Transactions", array);

            return Content(json.ToString());
        }

        // GET: api/Transactions/history
        [HttpGet("history")]
        public ActionResult GetHistory()
        {
            JObject json = new JObject();
            JArray array = new JArray();

            foreach(var item in Parking.Instance
                .GetTransactionHistoryForLastMinute()
                .Select(t => Helper.TransactionToJObject(t)))
            {
                array.Add(item);
            }

            json.Add("Transactions", array);

            return Content(json.ToString());
        }


        // GET: api/Transactions/history/5
        [HttpGet("history/{id}")]
        public ActionResult GetHistory(int id)
        {
            Car car = Parking.Instance[id - 1];

            if (car == null)
                return BadRequest(Helper.ErrorCarIdNotFound);

            JObject json = new JObject();
            JArray array = new JArray();

            foreach (var item in Parking.Instance
                .GetTransactionHistoryForLastMinute(car)
                .Select(t => Helper.TransactionToJObject(t)))
            {
                array.Add(item);
            }

            json.Add("Car", Helper.CarToJObject(car, id));
            json.Add("Transactions", array);

            return Content(json.ToString());
        }

        // PUT: api/Transactions/5;50
        [HttpPut("recharge/{id};{amount}")]
        public ActionResult Put(int id, decimal amount)
        {
            if (!Parking.Instance.HasCars)
                return BadRequest(Helper.ErrorParkingEmpty);

            Car car = Parking.Instance[id - 1];

            if (car == null)
                return BadRequest(Helper.ErrorCarIdNotFound);

            if (amount >= Car.MinAmount && amount <= Car.MaxAmount)
            {
                Parking.Instance.RechargeCarBalance(car, amount);
                return Content(Helper.CarToJObject(car, id).ToString());
            }
            else
            {
                return BadRequest(Helper.ErrorCarAmountOutOfRange);
            }
        }
    }
}
