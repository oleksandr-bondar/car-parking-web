using System;

namespace CarParking
{
    /// <summary>
    /// Представляет единичную транзакцию.
    /// </summary>
    public sealed class Transaction
    {
        private decimal _debited;

        /// <summary>
        /// Возвращает дату и время, когда была произведена транзакция.
        /// </summary>
        public DateTime DateTime { get; private set; }
        /// <summary>
        /// Возвращает уникальный идентификатор машины.
        /// </summary>
        public Guid CarGuid { get; private set; }
        /// <summary>
        /// Возвращает количество списанных средств.
        /// </summary>
        public decimal Debited
        {
            get => _debited;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Значение списанных средств не может быть меньше нуля.");

                _debited = value;
            }
        }

        public Transaction(DateTime dateTime, Guid carGuid, decimal debited)
        {
            DateTime = dateTime;
            CarGuid = carGuid;
            Debited = debited;
        }

        public override string ToString()
        {
            return $"Время: {DateTime.ToString()} Guid машины: {CarGuid.ToString()} Списано средств: {_debited.ToString()}";
        }
    }
}
