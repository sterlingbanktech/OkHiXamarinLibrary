using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp
{
    public interface IOkLocationService
    {
        string GetAddress(string phoneNumber,string firstName,string LastName);
    }
}
