using System;
using System.Linq;
using Spectre.Console;

namespace PhotoSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Any())
            {
                _ = RunCli(args);
            }
            else
            {
                RunConsole();
            } 
        }

        private static void RunConsole()
        {
            try
            {
                AnsiConsole.Render(new FigletText("PhotoSync").Centered());
                AnsiConsole.Render(new Rule());
                AnsiConsole.Markup("[underline red]Hello[/] World!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred:");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to exit.");
                _ = Console.ReadKey();
            }
        }

        private static int RunCli(string[] args)
        {
            return 0;
        }
    }
}
