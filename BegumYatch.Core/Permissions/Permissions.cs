using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Permissions
{
    public static class Permissions
    {
        public static class Demand
        {
            public const string Read = "Permissions.Demand.Read";
            public const string Create = "Permissions.Demand.Create";
            public const string Update = "Permissions.Demand.Update";
            public const string Delete = "Permissions.Demand.Delete";
        }
        public static class Order
        {
            public const string Read = "Permissions.Order.Read";
            public const string Create = "Permissions.Order.Create";
            public const string Update = "Permissions.Order.Update";
            public const string Delete = "Permissions.Order.Delete";
        }
    }
}
