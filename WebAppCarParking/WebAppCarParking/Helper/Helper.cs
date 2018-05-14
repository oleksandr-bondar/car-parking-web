using CarParking;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCarParking
{
    /// <summary>
    /// Содержит вспомогательные методы.
    /// </summary>
    internal static class Helper
    {
        public static JObject CarToJObject(Car car, int id)
        {
            JObject jsonCar = new JObject
            {
                { "ParkingPlace", new JValue(id) },
                { nameof(Car.Guid), new JValue(car.Guid) },
                { nameof(Car.Balance), new JValue(car.Balance) },
                { nameof(Car.Type), new JValue(car.Type.ToString()) }
            };

            return jsonCar;
        }

        public static JObject TransactionToJObject(Transaction trn)
        {
            JObject json = new JObject
            {
                { nameof(Transaction.DateTime), new JValue(trn.DateTime) },
                { nameof(Transaction.CarGuid), new JValue(trn.CarGuid) },
                { nameof(Transaction.Debited), new JValue(trn.Debited) }
            };

            return json;
        }

        public static string GetHelp()
        {
            return GetHelpCars() + GetHelpParking() + GetHelpTransactions();
        }

        public static string GetHelpCars()
        {
            return @"REST API: Cars

-Список всіх машин (GET) 
    http://localhost:53577/api/cars/all

-Деталі по одній машині (GET) 
    http://localhost:53577/api/cars/id
        [id - порядковий номер машини на парковці]

-Видалити машину (DELETE) 
    http://localhost:53577/api/cars/delete/id
        [id - порядковий номер машини на парковці]

-Додати машину (POST) 
    http://localhost:53577/api/cars/add
    http://localhost:53577/api/cars/add/type;balance
        [type - приймає наступні значення: 1, 2, 3, 4, Passenger, Truck, Bus, Motorcycle]
        [balance - приймає значення в діапазоні від 50 до 1000]

-Додати певне число машин (POST) 
    http://localhost:53577/api/cars/addrange/count
        [count - кількість машин для додавання на парковку]

";
        }

        public static string GetHelpParking()
        {
            return @"REST API: Parking

-Кількість вільних місць (GET) 
    http://localhost:53577/api/parking/free

-Кількість зайнятих місць (GET) 
    http://localhost:53577/api/parking/occupied

-Загальний дохід (GET) 
    http://localhost:53577/api/parking/balance

-Загальна статистика (GET) 
    http://localhost:53577/api/parking/all

";
        }

        public static string GetHelpTransactions()
        {
            return @"REST API: Transactions

-Вивести Transactions.log (GET) 
    http://localhost:53577/api/transactions/log

-Вивести транзакції за останню хвилину (GET)
    http://localhost:53577/api/transactions/history

-Вивести транзакції за останню хвилину по одній конкретній машині (GET) 
    http://localhost:53577/api/transactions/history/id
        [id - порядковий номер машини на парковці]

-Поповнити баланс машини (PUT) 
    http://localhost:53577/api/transactions/recharge/id;amount
        [id - порядковий номер машини на парковці]
        [amount - приймає значення в діапазоні від 50 до 1000]

";
        }

        public static readonly string ErrorCarBalanceOutOfRange = $"Помилка. Баланс машини повинен знаходитись в межах від {Car.MinAmount.ToString()} до {Car.MaxAmount.ToString()}.";
        public static readonly string ErrorCarAmountOutOfRange = $"Помилка. Сума поповнення повинна знаходитись в межах від {Car.MinAmount.ToString()} до {Car.MaxAmount.ToString()}.";
        public static readonly string ErrorWrongCarType = "Помилка. Недопустиме значення типу машини. Використайте одне із наступних: 1, 2, 3, 4, Passenger, Truck, Bus, Motorcycle.";
        public static readonly string ErrorParkingFilled = "Помилка. Парковка заповнена.";
        public static readonly string ErrorParkingEmpty = "Помилка. Парковка пуста.";
        public static readonly string ErrorCarIdNotFound = "Помилка. По вказаному номеру місця на парковці не вдалось знайти машину.";
        //public static readonly string Error = "";
    }
}
