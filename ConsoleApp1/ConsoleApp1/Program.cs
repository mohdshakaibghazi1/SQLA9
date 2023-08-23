using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    internal class Program
    {
        static SqlConnection con;
        static SqlCommand cmd;
        static string conStr = "Data Source=HP\\SQLEXPRESS;Initial Catalog=OrderB;Integrated Security=True";

        static void Main(string[] args)
        {
            try
            {
                con = new SqlConnection(conStr);
                con.Open();

                Console.WriteLine("Connection established.");

                while (true)
                {
                    Console.WriteLine("1. Insert Customer");
                    Console.WriteLine("2. Place Order");
                    Console.WriteLine("3. Exit");
                    Console.Write("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            InsertCustomer();
                            break;
                        case 2:
                            PlaceOrder();
                            break;
                        case 3:
                            return;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
                Console.WriteLine("Connection closed.");
            }
        }

        static void InsertCustomer()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            try
            {
                using (cmd = new SqlCommand("INSERT INTO Customers (CustomerId, FirstName, LastName) VALUES (@customerId, @firstName, @lastName)", con))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Customer inserted.");
                    }
                    else
                    {
                        Console.WriteLine("Customer insertion failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        static void PlaceOrder()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Total Amount: ");
            decimal totalAmount = Convert.ToDecimal(Console.ReadLine());

            try
            {
                using (cmd = new SqlCommand("PlaceOrder", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount);

                    int output = (int)cmd.ExecuteScalar();

                    if (output >= 1)
                    {
                        Console.WriteLine("Order Placed Successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Order Placing Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while placing the order: " + ex.Message);
            }
        }
    }
}
