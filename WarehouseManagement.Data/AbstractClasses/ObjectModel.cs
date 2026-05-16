using System.Numerics;

namespace WarehouseManagement.Data.AbstractClasses
{
    public abstract class ObjectModel
    {
        public static BigInteger Counter = BigInteger.Zero;
        public BigInteger Id { get; set; }

        public ObjectModel()
        {
            Id = ++Counter;
        }
    }
}
