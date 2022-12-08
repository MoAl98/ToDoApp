using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoClass
{
    public class ToDoApp 
    {
        public int ToDoID { get; set; }
        public string project { get; set; }
        public DateTime date { get; set; }
        public string Task { get; set; }

        public ToDoApp(int ToDoID, string project, DateTime date, string Task )
        {
            this.ToDoID = ToDoID;
            this.project = project;
            this.date = date;
            this.Task = Task;
            
        }
        // method header and footer that can be reusable in the code
        public static void header()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("====================================");
            Console.WriteLine("\t TO-DO List");
            Console.WriteLine("====================================");
            Console.ResetColor();
        }
        public static void footer()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("====================================");
            Console.ResetColor();
        }

        public static void UserMsg(String msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + msg + "\n");
            Console.Write("Press any key to continue:");
            Console.ResetColor();
            Console.ReadKey();
        }
        //method that helps to check if the date is valid
        public static int CheckDatum(string datum)
        {
            DateTime cur_time = DateTime.Now;
            cur_time.ToString("d-M-yyyy");
            //check if the inserted date by user is older than todays date
            TimeSpan duration = DateTime.Parse(cur_time.ToString()) - (DateTime.Parse(datum.ToString()));
            int day = (int)Math.Round(duration.TotalDays);
            return day;
        }

        //method that check if the data is in date form
        public static bool checkDate(String datum)
        {
            DateTime parsedDateTime;
            if (DateTime.TryParse(datum, out parsedDateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}