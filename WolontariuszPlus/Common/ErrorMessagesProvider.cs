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
            public static string EventNotExists => "Wydarzenie o takim Id nie istnieje";
            public static string OnlyVolunteerCanTakePartInEvent => "Do wydarzenia może zapisać się wyłącznie wolontariusz";
            public static string EventDatePassed => "Nie można zapisać się na wydarzenie, które już się odbyło.";
            public static string NotEnoughPoints => "Wolontariusz nie ma wystarczającej liczby punktów, aby zapisać się na to wydarzenie";
        }
    }
}
