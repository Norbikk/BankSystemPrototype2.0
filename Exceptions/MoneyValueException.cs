using System;


namespace BankSystemPrototype
{
   internal class MoneyValueException:Exception
    {
        public MoneyValueException(int count)
        {
            Count = count;
        }
        public int Count { get; set; }
    }
}
