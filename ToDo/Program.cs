using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using ToDoClass;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic;

class Program
{
    public static void Main(string[] args)
    {

        List<ToDoApp> ToDoTask = new List<ToDoApp>();//create object of class ToDoApp

        int completeCount = 0;
        int ID = 0;

        //reading files information and insert them to list called lines       
        string filepath = @"C:\\Users\\User\\Desktop\\C#\\Indvidual project\\ToDo\\text.txt";
        List<string> lines = System.IO.File.ReadAllLines(filepath).ToList();
        var f = new FileInfo(filepath);
        if (f.Length != 0)
        {
            //loop through lines list
            foreach (var line in lines)
            {
                string[] values = line.Split(',');
                int theID = Convert.ToInt32(values[0]);
                string project = values[1];
                DateTime theDate = DateTime.Parse(values[2]);
                string theTask = values[3];

                //add files information to the program list
                ToDoTask.Add(new ToDoApp(theID, project, theDate, theTask));
            }
        }
        bool check = true;
        bool isComplete = false;

    SUDO_MAIN:
        while (true)
        {
            //count the tasks in the program
            int totalTasks = ToDoTask.Count;
            int unCompleted = totalTasks - completeCount;

            ToDoApp.header();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You have " + completeCount + " tasks are done" + " And " + unCompleted + " tasks todo.\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1.Add new Task.\t\t3.Update Task.\n");
            Console.WriteLine("2.View all Tasks.\t4.Delete Task.\n");
            Console.WriteLine("5.Sort by date or project.\t6.Mark as done.\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("7.Save and exit\n");
            Console.ResetColor();
            ToDoApp.footer();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter your choice number: "); Console.ResetColor();


            int choice = 0;
            //user must insert a number to select a case
            try
            {
                choice = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                ToDoApp.UserMsg("ERROR: Please Enter Valid Number!");
            }

            switch (choice)//program will depends on choosed number
            {
                //------------------------------------Add new Task----------------------------------------------------------------------------------------------------------------------------------
                case 1:
                    ToDoApp.header();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter the Date.\t[DD-MM-YYYY]\n");
                    Console.ResetColor();

                    string datum = Console.ReadLine();
                    //check if the inserted date by user is older than todays date
                    int day = ToDoApp.CheckDatum(datum);//using the method that help to check if the date is in the past

                    if (day >= 2) // user will get error message if the date is older than today and get informed from which date the input should be   
                    {
                        DateTime dtn = DateTime.Now;
                        string msg = "Please select date from " + dtn.ToString("d-M-yyyy") + " and upwards!";
                        ToDoApp.UserMsg("ERROR: " + msg);
                        goto SUDO_MAIN;
                    }

                    if (ToDoApp.checkDate(datum)) //if the date is valid the program will continue   
                    {
                        Console.Write("\nEnter Project.\n");
                        string proj = Console.ReadLine();
                        Console.Write("\nEnter Task.\n");
                        string tsk = Console.ReadLine();


                        ID++;//the tasks will get id number which start from number 1


                        ToDoTask.Add(new ToDoApp(ID, proj, DateTime.Parse(datum), tsk));//add user inputs into the list
                        ToDoApp.UserMsg("New Task is created with ID number:" + ID.ToString());

                        //add the inputs into local file using stream writer
                        StreamWriter sw = new StreamWriter(filepath, true);
                        string toFile = ID.ToString() + "," + proj + "," + datum.ToString() + "," + tsk;
                        sw.WriteLine(toFile);
                        sw.Dispose();
                    }
                    else
                    {
                        ToDoApp.UserMsg("ERROR: Invalid date!, ");
                    }
                    break;
                //----------------------------------------View all Tasks-----------------------------------------------------------------------------------------------------------------------
                case 2:
                    ToDoApp.header();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("\tID\tDate\t\tProject\tTask");
                    Console.ResetColor();


                    foreach (ToDoApp x in ToDoTask)//show the list 
                    {
                        check = false;
                        Console.WriteLine("\t" + x.ToDoID + "\t" + x.date.ToString("dd-MM-yyyy") + "\t" + x.project + "\t" + x.Task);
                    }

                    if (check)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No Tasks Found!\n");
                        Console.ResetColor();
                    }
                    ToDoApp.footer();
                    Console.Write("Press any key to continue:");
                    Console.ReadKey();
                    break;
                //-------------------------------------Update Task----------------------------------------------------------------------------------------------------------------------------
                case 3:
                    ToDoApp.header();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter the Task ID.");
                    Console.ResetColor();
                    try
                    {
                        int TaskID = int.Parse(Console.ReadLine());//user choose which task will be updated 
                        for (int i = 0; i < ToDoTask.Count; i++)// loop through the list
                        {
                            if (ToDoTask[i].ToDoID == TaskID)//select the task id  
                            {
                                check = false;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("Enter the Date.\t[DD-MM-YYYY]\n");
                                Console.ResetColor();

                                //new information for the chosen task
                                datum = Console.ReadLine();
                                day = ToDoApp.CheckDatum(datum);

                                if (day >= 2)
                                {
                                    DateTime dtn = DateTime.Now;
                                    string msg = "Please select date from " + dtn.ToString("d-M-yyyy") + " and upwards!";
                                    ToDoApp.UserMsg("ERROR: " + msg);
                                    goto SUDO_MAIN;
                                }

                                if (ToDoApp.checkDate(datum))
                                {
                                    Console.Write("Enter Project.\n");
                                    string newProj = Console.ReadLine();
                                    Console.Write("Enter Task.\n");
                                    string newTask = Console.ReadLine();

                                    ToDoTask[i].date = DateTime.Parse(datum);
                                    ToDoTask[i].Task = newTask;
                                    ToDoTask[i].project = newProj;

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Task Updated!\n");
                                    Console.ResetColor();

                                    //  update the task from the file also
                                    string[] Lines = File.ReadAllLines(filepath);
                                    File.Delete(filepath);
                                    using (StreamWriter sw = File.AppendText(filepath))
                                    {
                                        foreach (string line in Lines)
                                        {
                                            string[] values = line.Split(',');
                                            int theID = Convert.ToInt32(values[0]);
                                            if (theID != TaskID)
                                            {
                                                //Skip the line
                                                continue;
                                            }
                                            else
                                            {   //add the new information to the file
                                                string toFile = TaskID.ToString() + "," + newProj + "," + datum.ToString() + "," + newTask;
                                                sw.WriteLine(toFile);
                                                sw.Dispose();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ToDoApp.UserMsg("ERROR: Invalid Date!");
                                }
                            }
                        }
                        if (check)
                        {
                            Console.WriteLine(" No Record Found!");
                        }
                        ToDoApp.footer();
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                    }
                    catch (Exception)
                    {
                        ToDoApp.UserMsg("ERROR: Enter Numbers Only!!");
                    }
                    break;
                //----------------------------------------Delete Task-------------------------------------------------------------------------------------------------------------------------------
                case 4:
                    ToDoApp.header();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter the Task_ID.\n");
                    Console.ResetColor();
                    try
                    {
                        int TDID = int.Parse(Console.ReadLine());
                        for (int i = 0; i < ToDoTask.Count; i++)
                        {
                            if (ToDoTask[i].ToDoID == TDID)//check if the inserted number equal id number of the list
                            {
                                check = false;
                                ToDoTask.RemoveAll(e => e.ToDoID == TDID);//remove the selected task                              
                            }
                            //  remove the task from the file also
                            string[] Lines = File.ReadAllLines(filepath);
                            File.Delete(filepath);
                            using (StreamWriter sw = File.AppendText(filepath))

                            {
                                foreach (string line in Lines)
                                {
                                    string[] values = line.Split(',');
                                    int theID = Convert.ToInt32(values[0]);
                                    if (theID != TDID + 1)
                                    {
                                        //Skip the line
                                        continue;
                                    }
                                    else
                                    {
                                        sw.WriteLine(line);
                                    }
                                }
                            }
                        }
                        if (check)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No Record Found!\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Record Deleted!\n");
                            Console.ResetColor();

                        }
                        ToDoApp.footer();
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                    }
                    catch (Exception)
                    {
                        ToDoApp.UserMsg("ERROR: Enter Numbers Only! ");
                    }
                    break;
                //------------------------------------------------------Sort by date or project----------------------------------------------------------------------------------------------------------------
                case 5:
                    while (true)
                    {
                        ToDoApp.header();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("1.Sort By Project.");
                        Console.WriteLine("2.Sort By DATE.");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("3.Exit."); Console.ResetColor();
                        ToDoApp.footer();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Enter your choice: "); Console.ResetColor();
                        try
                        {    //read user input
                            choice = int.Parse(Console.ReadLine());
                            switch (choice)
                            {
                                case 1:
                                    ToDoApp.header();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("ID\tDate\t\tProject\tTask");
                                    Console.WriteLine("------------------------------------");
                                    Console.ResetColor();
                                    //order the list by project
                                    ToDoTask = ToDoTask.OrderBy(x => x.project).ToList();
                                    foreach (ToDoApp x in ToDoTask)
                                    {
                                        check = false;
                                        Console.WriteLine(x.ToDoID + "\t" + x.date.ToString("dd-MM-yyyy") + "\t" + x.project + "\t" + x.Task);
                                    }
                                    if (check)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("No tasks founded!\n");
                                        Console.ResetColor();
                                    }
                                    ToDoApp.footer();
                                    Console.Write("Press any key to continue:");
                                    Console.ReadKey();
                                    break;

                                case 2:
                                    ToDoApp.header();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("ID\tDate\t\tProject\tTask");
                                    Console.WriteLine("------------------------------------");
                                    Console.ResetColor();
                                    //order the list by date
                                    ToDoTask = ToDoTask.OrderBy(x => x.date).ToList();
                                    foreach (ToDoApp x in ToDoTask)
                                    {
                                        check = false;
                                        Console.WriteLine(x.ToDoID + "\t" + x.date.ToString("dd-MM-yyyy") + "\t" + x.project + "\t" + x.Task);
                                    }
                                    if (check)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("No tasks founded!\n");
                                        Console.ResetColor();
                                    }
                                    ToDoApp.footer();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.Write("Press any key to continue:");
                                    Console.ResetColor();
                                    Console.ReadKey();
                                    break;

                                case 3:
                                    goto SUDO_MAIN;

                                default:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    ToDoApp.UserMsg("Invalid choice!");
                                    Console.ResetColor();
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            ToDoApp.UserMsg("ERROR: Insert Only Numbers!");
                        }
                    }
                //-----------------------------------------------------Mark as done-----------------------------------------------------------------------------------------------------------------
                case 6:
                    ToDoApp.header();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter the Task_ID.\n");
                    Console.ResetColor();
                    try
                    {
                        int TDID = int.Parse(Console.ReadLine());
                        //loop through the list 
                        for (int i = 0; i < ToDoTask.Count; i++)
                        {   //find out which task is needed 
                            if (ToDoTask[i].ToDoID == TDID)
                            {
                                check = false;
                                completeCount++;//add one to the counter
                                isComplete = true;
                            }
                            //  remove the task from the file 
                            string[] Lines = File.ReadAllLines(filepath);
                            File.Delete(filepath);
                            using (StreamWriter sw = File.AppendText(filepath))

                            {
                                foreach (string line in Lines)
                                {
                                    string[] values = line.Split(',');
                                    int theID = Convert.ToInt32(values[0]);
                                    if (theID != TDID + 1)
                                    {
                                        //Skip the line
                                        continue;
                                    }
                                    else
                                    {
                                        sw.WriteLine(line);//remove the line
                                    }
                                }
                            }
                        }
                        if (check)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" No tasks founded!\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Task marked as done!\n");
                            Console.ResetColor();
                        }
                        ToDoApp.footer();
                        Console.Write("Press any key to continue:");
                        Console.ReadKey();
                    }
                    catch (Exception)
                    {
                        ToDoApp.UserMsg("ERROR: Insert Only Numbers!");
                    }
                    break;
                //-----------------------------------------------Save and exit-----------------------------------------------------------------------------------------------------------------------
                case 7:
                    ToDoApp.header();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Observe: if you exit the program your tasked that are marked as done will be removed\n\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Are you sure you want to exit the program?\n");
                    Console.WriteLine("If you want to continue type 'yes' otherwise press any key to exit\n "); Console.ResetColor();
                    try
                    {
                        string checkIf = Console.ReadLine();
                        if (checkIf != "yes")
                        {
                            Environment.Exit(0);//exit the program 
                        }
                        else
                        {
                            goto SUDO_MAIN;//go back to menu 
                        }

                    }
                    catch (Exception)
                    {
                        ToDoApp.UserMsg("ERROR: Enter Valid Data!");
                    }
                    break;
            }
        }
    }
}



