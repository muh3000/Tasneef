using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Utilities
{
    public class CustomArCulture : CultureInfo
    {
        public CustomArCulture()

        : this("ar", true)
        {

        }

        public CustomArCulture(string cultureName, bool useUserOverride)

        : base(cultureName, useUserOverride)
        {
            base.DateTimeFormat.Calendar = new GregorianCalendar();

        }

    }
}
