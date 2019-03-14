using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCProjectVIdent.Models
{

    public struct MyRole
    {

        public const string BasicAdmin = "BasicAdmin";
        public const string Admin = "Admin";
        public const string Employee = "Employee";
        public const string Member = "Member";
	    public const string BasicAdmin_Admin = "BasicAdmin,Admin";
	}
    public class MyDataType
    {
       
        public enum BookStatus
        {
            isReading = 0,
            isborrowking = 1,
        }
        public enum blockStaues
        {
            notBlock = 0,
            blockInWeek,
            blockAfterWeek

        }
    }
}