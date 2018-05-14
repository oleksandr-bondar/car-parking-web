using System;

namespace CarParking
{
    /// <summary>
    /// Представляет машину.
    /// </summary>
    public sealed class Car
    {
        /// <summary>
        /// Возвращает уникальный идентификатор.
        /// </summary>
        public Guid Guid { get; private set; }
        /// <summary>
        /// Возвращает баланс.
        /// </summary>
        public decimal Balance { get; internal set; }
        /// <summary>
        /// Возвращает тип машины.
        /// </summary>
        public CarType Type { get; private set; }

        public Car(CarType carType, decimal initBalance)
        {
            Guid = Guid.NewGuid();
            Type = carType;
            Balance = initBalance;
        }

        public override string ToString()
        {
            return $"Guid: {Guid.ToString()} Баланс: {Balance.ToString()} Тип: {Type.ToString()}";
        }

        public readonly static decimal MinAmount = 50;
        public readonly static decimal MaxAmount = 1000;

        private readonly static Random rnd = new Random();

        /// <summary>
        /// Выполняет создание машины со случайными свойствами.
        /// </summary>
        /// <returns></returns>
        public static Car NewCar() => new Car((CarType)rnd.Next(1, 5), rnd.Next(50, 1051));
    }
}
