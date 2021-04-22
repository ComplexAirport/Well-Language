using System;

namespace WellLang
{
    class Start
    {
        static void Main(string[] args)
        {
            ErrorStream globalErrorStream = new ErrorStream();

            string code = @"10";
            Utils.Execute("a.txt", code, globalErrorStream);
        }
    }


}