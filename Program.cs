using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace To_Do;

//main to do app
class Program
{

    enum UserChoice { 
    AddTask = 1, 
    DeleteTask, 
    Exit
    }

    public static List<string> GetData()
{
    var entries = new List<string>();
    using (var db = new SqliteConnection("Data Source=ToDo.db"))
    {
        db.Open();
        var selectCommand = new SqliteCommand
            ("SELECT * from items", db);

        SqliteDataReader query = selectCommand.ExecuteReader();

        while (query.Read())
        {
            entries.Add(query.GetString(0));
        }
    }

    return entries;
}

public static void AddData(string inputText)
{
    using (var db = new SqliteConnection("Data Source=ToDo.db"))
    {
        db.Open();

        var insertCommand = new SqliteCommand();
        insertCommand.Connection = db;

        // Use parameterized query to prevent SQL injection attacks
        insertCommand.CommandText = "INSERT INTO items (list_item) VALUES (@Entry);";
        insertCommand.Parameters.AddWithValue("@Entry", inputText);

        insertCommand.ExecuteReader();
    }

}

public static void DeleteData(string inputText)
{
    using (var db = new SqliteConnection("Data Source=ToDo.db"))
    {
        db.Open();

        var insertCommand = new SqliteCommand();
        insertCommand.Connection = db;

        // Use parameterized query to prevent SQL injection attacks
        insertCommand.CommandText = "DELETE from items where list_item = @Entry;";
        insertCommand.Parameters.AddWithValue("@Entry", inputText);

        insertCommand.ExecuteReader();
    }

}
    static void Main(string[] args)
    {
       List<string> toDoList = new List<string>();
       toDoList = GetData();

       while (true) {

        if (toDoList.Count > 0) { 
            Console.WriteLine("To-do List:");

            for (int i = 0; i < toDoList.Count; i++) { 
            Console.WriteLine("- " + toDoList[i]); 
            } 

            Console.WriteLine("");
        } else { 
            Console.WriteLine("You currently have no tasks in your To-do list."); 
            Console.WriteLine("");
        }
        Console.WriteLine("1. Add task");
        Console.WriteLine("2. Delete task");
        Console.WriteLine("3. Exit");
        int choice = int.Parse(Console.ReadLine());

        if (choice == (int)UserChoice.AddTask) {
            Console.Write("Enter task: "); 
            string task = Console.ReadLine(); 
            toDoList.Add(task); 
            AddData(task);
            Console.Clear(); 
            Console.WriteLine("Task added successfully!");
        } else if (choice == (int)UserChoice.Exit) { 
            break;
        } else if (choice == (int)UserChoice.DeleteTask) {
            if (toDoList.Count > 0) { 
                Console.WriteLine("Enter the number of the task you want to delete:");

            for (int i = 0; i < toDoList.Count; i++) { 
                Console.WriteLine("(" + (i + 1) + ") " + toDoList[i]); 
            }
            }
            int taskNum = int.Parse(Console.ReadLine());
            taskNum--;

            if (taskNum >= 0 && taskNum < toDoList.Count) { 
                var deleted_item = toDoList[taskNum];
                toDoList.RemoveAt(taskNum); 
                DeleteData(deleted_item);
                Console.Clear(); 
                Console.WriteLine("Task deleted successfully!"); 
                Console.WriteLine("");
            } else { 
                Console.Clear(); 
                Console.WriteLine("Invalid task number."); 
                Console.WriteLine("");
            }
        }

        }
    }
}
