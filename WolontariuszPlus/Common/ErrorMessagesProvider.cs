using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Common
{
    public static class ErrorMessagesProvider
    {
        public static class EventErrors
        {
            public const string EventNotExists = "Wydarzenie o takim Id nie istnieje";
            public const string OnlyVolunteerCanTakePartInEvent = "Do wydarzenia może zapisać się wyłącznie wolontariusz";
            public const string EventDatePassed = "Nie można zapisać się na wydarzenie, które już się odbyło.";
            public const string NotEnoughPoints = "Wolontariusz nie ma wystarczającej liczby punktów, aby zapisać się na to wydarzenie";
        }

        public static class VolunteerErrors
        {
            public const string VolunteerNotExists = "Wolontariusz o podanym Id nie istnieje";
        }

        public static class VolunteerOnEventErrors
        {
            public const string VolunteerOnEventNotExists = "VolunteerOnEvent o podanym Id nie istnieje";
            public const string VolunteerIsNotOnThisEvent = "Ten wolontariusz nie jest uczestnikiem tego wydarzenia";
            public const string InvalidAmmountOfMoney = "Ilość pieniędzy nie może być mniejsza od 0";
        }
    }
}
