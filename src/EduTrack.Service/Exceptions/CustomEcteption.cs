using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Exceptions;

public class CustomEcteption : Exception
{
    public int StatusCode { get; set; }

    public CustomEcteption(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
