using System;

namespace Core
{
    public interface IUserCurrenciesStateReadonly
    {
        int Crystals { get; }
        int Gas { get; }

        event Action Changed;
    }
}
