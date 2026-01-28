using System;

namespace ConstructionInventory.Domain.Enums
{
    public enum MovementType
    {
        Entry = 1, // depo giriş
        Exit =2, // şantiye çıkış
        Return = 3, // şantiyeden iade
        Waste = 4 // kayıp

    }
}