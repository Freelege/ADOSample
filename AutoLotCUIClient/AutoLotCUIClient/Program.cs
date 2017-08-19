using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoLotConnectedLayer;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AutoLotCUIClient
{
    class Program
    {
        private static void ShowInstructions()
        {
            Console.WriteLine("I: Inserts a new car.");
            Console.WriteLine("U: Updates an existing car.");
            Console.WriteLine("D: Deletes an existing car.");
            Console.WriteLine("L: Lists current inventory.");
            Console.WriteLine("S: Shows these instructions.");
            Console.WriteLine("P: Looks up pet name.");
            Console.WriteLine("Q: Quits program.");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("***** The AutoLot Console UI *****\n");
            // Get connection string from App.config.
            string cnStr =
            ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;
            bool userDone = false;
            string userCommand = "";
            // Create our InventoryDAL object.
            InventoryDAL invDAL = new InventoryDAL();
            invDAL.OpenConnection(cnStr);
            // Keep asking for input until user presses the Q key.
            try
            {
                ShowInstructions();
                do
                {
                    Console.Write("\nPlease enter your command: ");
                    userCommand = Console.ReadLine();
                    Console.WriteLine();
                    switch (userCommand.ToUpper())
                    {
                        case "I":
                            InsertNewCar(invDAL);
                            break;
                        case "U":
                            UpdateCarPetName(invDAL);
                            break;
                        case "D":
                            DeleteCar(invDAL);
                            break;
                        case "L":
                            ListInventory(invDAL);
                            break;
                        case "S":
                            ShowInstructions();
                            break;
                        case "P":
                            LookUpPetName(invDAL);
                            break;
                        case "Q":
                            userDone = true;
                            break;
                        default:
                            Console.WriteLine("Bad data! Try again");
                            break;
                    }
                } while (!userDone);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                invDAL.CloseConnection();
            }
        }
        private static void InsertNewCar(InventoryDAL invDAL)
        {
            // First get the user data.
            int newCarID;
            string newCarColor, newCarMake, newCarPetName;
            Console.Write("Enter Car ID: ");
            newCarID = int.Parse(Console.ReadLine());
            Console.Write("Enter Car Color: ");
            newCarColor = Console.ReadLine();
            Console.Write("Enter Car Make: ");
            newCarMake = Console.ReadLine();
            Console.Write("Enter Pet Name: ");
            newCarPetName = Console.ReadLine();
            // Now pass to data access library.
            invDAL.InsertAuto(newCarID, newCarColor, newCarMake, newCarPetName);
        }
        private static void UpdateCarPetName(InventoryDAL invDAL)
        {
            // First get the user data.
            int carID;
            string newCarPetName;
            Console.Write("Enter Car ID: ");
            carID = int.Parse(Console.ReadLine());
            Console.Write("Enter New Pet Name: ");
            newCarPetName = Console.ReadLine();
            // Now pass to data access library.
            invDAL.UpdateCarPetName(carID, newCarPetName);
        }
        private static void LookUpPetName(InventoryDAL invDAL)
        {
            // Get ID of car to look up.
            Console.Write("Enter ID of Car to look up: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Petname of {0} is {1}.",
            id, invDAL.LookUpPetName(id).TrimEnd());
        }
        private static void DeleteCar(InventoryDAL invDAL)
        {
            // Get ID of car to delete.
            Console.Write("Enter ID of Car to delete: ");
            int id = int.Parse(Console.ReadLine());
            // Just in case you have a referential integrity
            // violation!
            try
            {
                invDAL.DeleteAuto(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void ListInventory(InventoryDAL invDAL)
        {
            // Get the list of inventory.
            DataTable dt = invDAL.GetAllInventoryAsDataTable();
            // Pass DataTable to helper function to display.
            DisplayTable(dt);
        }
        private static void DisplayTable(DataTable dt)
        {
            // Print out the column names.
            for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
            {
                Console.Write(dt.Columns[curCol].ColumnName + "\t");
            }
            Console.WriteLine("\n----------------------------------");
            // Print the DataTable.
            for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
            {
                for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                {
                    Console.Write(dt.Rows[curRow][curCol].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }

    }
}
