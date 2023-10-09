using System.Data.SqlClient;

namespace AptekJed.Models;

public class Menu
{
    public static void GetMenu()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("1-Info");
        Console.WriteLine("2-Search(name,price,purpose)");
        Console.WriteLine("3-Add Medicine");
        Console.WriteLine("4-Sell Medicine");
        Console.WriteLine("5-Update");
        Console.WriteLine("6-Change Pharmacy");
        Console.WriteLine("0-Quit");
        Console.WriteLine("=============================================");
    }

    public static int Info(int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select md.MedicineId, md.MedicineName,md.MedicinePrice from MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@pId", pId);
                SqlDataReader data = command.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Console.WriteLine($"Medicine Id: {data[0]} Medicine Name: {data[1]} Medicine Price: {data[2]}");
                    }
                    return 1;
                }
                else
                {
                    Console.WriteLine("Not Found!");
                    return 0;
                }
            }
        }
    }

    public static bool Check(int value, int count)
    {
        if (value >= count || value < 0)
        {
            return false;
        }
        else return true;
    }
}
