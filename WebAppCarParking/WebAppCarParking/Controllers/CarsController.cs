using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CarParking;

namespace WebAppCarParking.Controllers
{
    [Produces("application/json")]
    [Route("api/cars")]
    public class CarsController : Controller
    {
        [HttpGet]
        public ActionResult GetHelp()
        {
            return Content(Helper.GetHelpCars());
        }

        // GET: api/Cars
        [HttpGet("all")]
        public ActionResult Get()
        {
            if (!Parking.Instance.HasCars)
                return BadRequest(Helper.ErrorParkingEmpty);

            JObject json = new JObject();
            JArray array = new JArray();

            int id = 1;
            foreach (Car car in Parking.Instance.GetCars())
                array.Add(Helper.CarToJObject(car, id++));

            json.Add("Cars", array);

            return Content(json.ToString());
        }


        // GET: api/Cars/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult Get(int id)
        {
            if (!Parking.Instance.HasCars)
                return BadRequest(Helper.ErrorParkingEmpty);

            Car car = Parking.Instance[id - 1];

            if (car == null)
                return BadRequest(Helper.ErrorCarIdNotFound);

            return Content(Helper.CarToJObject(car, id).ToString());
        }

        // POST: api/Cars/add
        [HttpPost("add")]
        public ActionResult Post()
        {
            return AddCar(Car.NewCar());
        }

        // POST: api/Cars/add/Bus;573
        [HttpPost("add/{type};{balance}")]
        public ActionResult Post(CarType type, decimal balance)
        {
            switch (type)
            {
                case CarType.Passenger:
                case CarType.Truck:
                case CarType.Bus:
                case CarType.Motorcycle:
                    if (balance >= Car.MinAmount && balance <= Car.MaxAmount)
                        return AddCar(new Car(type, balance));
                    else
                        return BadRequest(Helper.ErrorCarBalanceOutOfRange);
                default:
                    return BadRequest(Helper.ErrorWrongCarType);
            }
        }

        // POST: api/Cars/addrange/10
        [HttpPost("addrange/{count}")]
        public ActionResult Post(int count)
        {
            if (Parking.Instance.CountFreeParkingSpace == 0)
                return BadRequest(Helper.ErrorParkingFilled);

            int created = 0;
            JObject json = new JObject();
            JArray array = new JArray();

            for (int i = 0; i < count && Parking.Instance.CountFreeParkingSpace > 0; i++, created++)
            {
                try
                {
                    Car car = Car.NewCar();
                    int id = Parking.Instance.AddCar(car);
                    array.Add(Helper.CarToJObject(car, id));
                }
                catch (InvalidOperationException)// parking filled
                {
                    break;
                }
            }

            if (created == 0)
                return BadRequest("Помилка. Не вдалось створити ні однієї машини.");

            json.Add("Cars", array);

            return Content(json.ToString());
        }

        // DELETE: api/cars/delete/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (!Parking.Instance.HasCars)
                return BadRequest(Helper.ErrorParkingEmpty);

            Car car = Parking.Instance[id - 1];

            if (car == null)
                return BadRequest(Helper.ErrorCarIdNotFound);
            else if (Parking.Instance.RemoveCar(car))
                return Content(Helper.CarToJObject(car, id).ToString());
            else if (car.Balance < 0)
                return BadRequest("Помилка. Машина оштрафована. Спочатку поповніть баланс.");
            else
                return BadRequest("Помилка. Машину не вдалось видалити. Причина невідома.");
        }


        private ActionResult AddCar(Car car)
        {
            try
            {
                if (Parking.Instance.CountFreeParkingSpace > 0)
                {
                    int id = Parking.Instance.AddCar(car);

                    return Content(Helper.CarToJObject(car, id).ToString());
                }
                else
                {
                    return BadRequest(Helper.ErrorParkingFilled);
                }
            }
            catch (InvalidOperationException)
            {
                return BadRequest(Helper.ErrorParkingFilled);
            }
        }
    }
}
