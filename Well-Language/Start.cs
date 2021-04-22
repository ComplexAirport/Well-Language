using System;

namespace WellLang
{
    class Start
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ErrorStream tempErrorStream = new ErrorStream();
                Console.Write("Well> ");
                string code = Console.ReadLine();
                Utils.Execute("<stdin>", code, tempErrorStream);
            }
        }
    }
}