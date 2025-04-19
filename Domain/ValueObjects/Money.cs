using System;

namespace Domain.ValueObjects
{
    public class Money : IEquatable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));
            Amount = amount;
            Currency = currency;
        }

       public override bool Equals(object? obj)
        {
            return Equals(obj as Money);
        }
        public bool Equals(Money? other)
        {
            if (other is null) return false;
            return Amount == other.Amount && Currency == other.Currency;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}
