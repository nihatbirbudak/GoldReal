using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GR.Models.Enum
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).First();
            var attributes = memberInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Any())
            {
                return ((DisplayAttribute)attributes.First()).Name;
            }
            return enumValue.ToString();
        }
    }
}
