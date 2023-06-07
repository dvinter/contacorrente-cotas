using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Infra.CrossCutting.Utils.Extensions
{
    public static class EnumExtensions
    {
        public struct TEnumDetails
        {
            public int Value { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }


        }

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        public static string GetEnumDisplayName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayNameAttribute[] attributes = fi.GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().DisplayName;
            }

            return value.ToString();
        }
    }
}
