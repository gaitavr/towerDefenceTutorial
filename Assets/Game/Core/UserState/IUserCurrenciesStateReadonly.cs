using System;

namespace Core
{
    public interface IUserCurrenciesStateReadonly
    {
        int Crystals { get; }
        int Gas { get; }
        int Energy { get; }

        event Action Changed;
    }
}
