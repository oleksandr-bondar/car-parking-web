using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;

namespace CarParking
{
    /// <summary>
    /// Представляет автомобильную парковку.
    /// </summary>
    public sealed class Parking
    {
        #region Singleton

        private static readonly Lazy<Parking> _instance = new Lazy<Parking>(() => new Parking());

        /// <summary>
        /// Представляет единственный экземпляр класса <see cref="Parking"/>.
        /// </summary>
        public static Parking Instance => _instance.Value;

        #endregion

        #region Fields

        private readonly object _lockObj = new object();
        private readonly string TransactionsLogFileName = "Transactions.log";
        private readonly TimeSpan OneMinute = TimeSpan.FromMinutes(1);

        private readonly List<Car> _cars;
        private readonly List<Transaction> _transactions;
        // Использ. для списывания средств за парковку
        private readonly Timer _timerParkingPayment;
        // Использ. для обновления файла "Transactions.log"
        private readonly Timer _timerTransactionsLog;
        private decimal _balance;

        #endregion

        #region Properties/Indexers

        /// <summary>
        /// Возвращает общее количество заработанных средств парковкой.
        /// </summary>
        public decimal Balance
        {
            get
            {
                lock (_lockObj) { return _balance; }
            }
        }
        /// <summary>
        /// Возвращает количество свободных мест на парковке.
        /// </summary>
        public int CountFreeParkingSpace
        {
            get
            {
                lock (_lockObj){ return Settings.ParkingSpace - _cars.Count; }
            }
        }
        /// <summary>
        /// Возвращает количество занятых мест на парковке.
        /// </summary>
        public int CountOccupiedParkingSpace
        {
            get
            {
                lock (_lockObj) { return _cars.Count; }
            }
        }

        public bool HasCars
        {
            get
            {
                lock (_lockObj) { return _cars.Count > 0; }
            }
        }

        /// <summary>
        /// Возвращает общее количество мест на парковке.
        /// </summary>
        public int MaxParkingSpace => Settings.ParkingSpace;

        public Car this[int index]
        {
            get
            {
                lock (_lockObj) { return ((uint)index < (uint)_cars.Count ? _cars[index] : null); }
            }
        }

        public Car this[Guid guid]
        {
            get
            {
                lock (_lockObj) { return _cars.Find(c => c.Guid == guid); }
            }
        }

        #endregion

        #region Constructors

        private Parking()
        {
            _cars = new List<Car>(Settings.ParkingSpace);
            _transactions = new List<Transaction>();

            _timerParkingPayment = new Timer(Settings.Timeout * 1000);
            _timerParkingPayment.AutoReset = true;
            _timerParkingPayment.Elapsed += TimerParkingPayment_Elapsed;
            //_timerParkingPayment.Start();

            _timerTransactionsLog = new Timer(60 * 1000);
            _timerTransactionsLog.AutoReset = true;
            _timerTransactionsLog.Elapsed += TimerTransactionsLog_Elapsed;
            _timerTransactionsLog.Start();
        }

        #endregion

        #region Private Methods

        private void TimerParkingPayment_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_lockObj)
            {
                foreach (Car car in _cars)
                {
                    decimal parkingPrice = Settings.GetParkingPrice(car.Type);

                    if (parkingPrice <= car.Balance)
                    {
                        car.Balance -= parkingPrice;
                        _balance += parkingPrice;
                        _transactions.Add(new Transaction(DateTime.Now, car.Guid, parkingPrice));
                    }
                    else
                    {
                        // Списываем штраф за парковку
                        car.Balance -= parkingPrice * Settings.Fine;
                    }
                }
            }
        }

        private void TimerTransactionsLog_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_lockObj)
            {
                decimal debited = GetDebitedForLastMinute();

                try
                {
                    File.AppendAllText(TransactionsLogFileName,
                        $"{DateTime.Now.ToString()}\t{debited.ToString()}\r\n",
                        Encoding.UTF8);
                }
                catch (DirectoryNotFoundException)
                { }
                catch (IOException)// не удалось записать в файл
                {}
                catch (UnauthorizedAccessException)// отсутствует необходимое разрешение для записи в файл
                { }

                // Удаляем все транзакции, которым больше 2х минут
                ClearOldTransactions();
            }
        }

        private void ClearOldTransactions()
        {
            TimeSpan TwoMinutes = TimeSpan.FromMinutes(2);
            DateTime dtNow = DateTime.Now;
            int count = 0;

            foreach (var item in _transactions)
            {
                if (dtNow - item.DateTime > TwoMinutes)
                    count++;
                else
                    break;
            }

            if (count > 0)
                _transactions.RemoveRange(0, count);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Добавляет указанную машину на парковку и возвращает ее порядковый номер.
        /// </summary>
        /// <param name="car">Машина.</param>
        /// <exception cref="ArgumentNullException">Значение car равно null.</exception>
        /// <exception cref="InvalidOperationException">На парковке отсутствуют свободные места.</exception>
        public int AddCar(Car car)
        {
            lock (_lockObj)
            {
                if (car == null)
                    throw new ArgumentNullException(nameof(car));

                if (_cars.Count == Settings.ParkingSpace)
                    throw new InvalidOperationException("На парковке отсутствуют свободные места.");

                _cars.Add(car);

                if (_cars.Count == 1)
                    _timerParkingPayment.Start();

                return _cars.Count;
            }
        }

        /// <summary>
        /// Удаляет указанную машину с парковки.
        /// В случае успеха возвращает true, иначе false.
        /// </summary>
        /// <param name="car">Машина.</param>
        /// <returns></returns>
        public bool RemoveCar(Car car)
        {
            lock (_lockObj)
            {
                if (car == null)
                    return false;

                int index = _cars.IndexOf(car);

                if (index >= 0)
                {
                    if (car.Balance < 0)
                        return false;

                    _cars.RemoveAt(index);

                    if (_cars.Count == 0)
                        _timerParkingPayment.Stop();

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Пополняет баланс указанной машины.
        /// </summary>
        /// <param name="car">Машина.</param>
        /// <param name="amount">Сумма пополнения.</param>
        /// <exception cref="ArgumentNullException">Значение car равно null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значение amount меньше или равно нулю.</exception>
        /// <returns></returns>
        public void RechargeCarBalance(Car car, decimal amount)
        {
            lock (_lockObj)
            {
                if (car == null)
                    throw new ArgumentNullException(nameof(car));

                if (amount <= 0)
                    throw new ArgumentOutOfRangeException(nameof(amount), "Значение суммы пополнения должно быть больше нуля.");

                car.Balance += amount;
            }
        }

        /// <summary>
        /// Возвращает копию всех машин на парковке.
        /// </summary>
        /// <returns></returns>
        public List<Car> GetCars()
        {
            lock (_lockObj)
            {
                return new List<Car>(_cars);
            }
        }

        /// <summary>
        /// Возвращает историю транзакций за последнюю минуту.
        /// </summary>
        /// <returns></returns>
        public List<Transaction> GetTransactionHistoryForLastMinute()
        {
            lock (_lockObj)
            {
                DateTime dtNow = DateTime.Now;

                return Enumerable.Reverse(_transactions)
                    .TakeWhile(t => dtNow - t.DateTime <= OneMinute)
                    .ToList();
            }
        }

        /// <summary>
        /// Возвращает историю транзакций за последнюю минуту для указанной машины.
        /// </summary>
        /// <returns></returns>
        public List<Transaction> GetTransactionHistoryForLastMinute(Car car)
        {
            lock (_lockObj)
            {
                if (car == null)
                    return new List<Transaction>();

                DateTime dtNow = DateTime.Now;

                return Enumerable.Reverse(_transactions)
                    .TakeWhile(t => dtNow - t.DateTime <= OneMinute)
                    .Where(t => t.CarGuid == car.Guid)
                    .ToList();
            }
        }

        /// <summary>
        /// Возвращает количество заработанных средств за последнюю минуту.
        /// </summary>
        /// <returns></returns>
        public decimal GetDebitedForLastMinute()
        {
            lock (_lockObj)
            {
                DateTime dtNow = DateTime.Now;
                decimal debited = Decimal.Zero;

                foreach (var item in Enumerable.Reverse(_transactions))
                {
                    if (dtNow - item.DateTime > OneMinute)
                        break;

                    debited += item.Debited;
                }

                return debited;
            }
        }

        /// <summary>
        /// Возвращает содержание файла Transactions.log.
        /// </summary>
        /// <returns></returns>
        public List<TransactionLogItem> GetTransactionLog()
        {
            lock (_lockObj)
            {
                var log = new List<TransactionLogItem>();

                try
                {
                    char[] separator = { '\t' };
                    string[] lines = File.ReadAllLines(TransactionsLogFileName, Encoding.UTF8);

                    foreach (string line in lines)
                    {
                        string[] data = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        if (data.Length == 2 
                            && DateTime.TryParse(data[0], out DateTime dt) 
                            && decimal.TryParse(data[1], out decimal debited))
                        {
                            log.Add(new TransactionLogItem(dt, debited));
                        }
                    }

                    return log;
                }
                catch (DirectoryNotFoundException)
                {
                    return log;
                }
                catch (FileNotFoundException)
                {
                    return log;
                }
                catch (IOException)// не удалось прочитать файл
                {
                    return log;
                }
                catch (UnauthorizedAccessException)// отсутствует необходимое разрешение для чтения файла
                {
                    return log;
                }
            }
        }

        #endregion
    }
}
