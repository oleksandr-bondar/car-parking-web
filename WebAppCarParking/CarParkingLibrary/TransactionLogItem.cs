using System;

namespace CarParking
{
    public struct TransactionLogItem
    {
        public DateTime DateTime { get; private set; }
        public decimal Debited { get; private set; }

        public TransactionLogItem(DateTime dt, decimal debited)
        {
            DateTime = dt;
            Debited = debited;
        }

        public override string ToString()
        {
            return $"[{DateTime.ToString()}]: {Debited.ToString()}";
        }
    }
}
