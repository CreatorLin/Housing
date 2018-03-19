using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Housing
{
    public interface IGeocoder
    {
        string[] GetFromLocationName(string locationName, int maxResults);
    }
}
