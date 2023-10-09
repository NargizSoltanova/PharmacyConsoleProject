using System.Data.SqlClient;

namespace AptekJed.Models;

public class Stock
{
    public static int GetStock(int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select p.PharmacyName,p.PharmacyId,s.StockId,s.MedicineCount , p.StockCount from Stocks as s \r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@pId", pId);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Console.WriteLine($"Pharmacy: {dataReader[0]} PharmacyId: {dataReader[1]} StockId: {dataReader[2]} Medicine Count: {dataReader[3]} Stock Count: {dataReader[4]} ");
                    }
                    return 1;
                }
                else
                {
                    Console.WriteLine("Stock not found!");
                    return 0;
                }
            }
        }
    }

    public static int SearchByMedicineId(int id, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select count(*) from MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicineId = @id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@pId", pId);
                var value = command.ExecuteScalar();
                if ((int)value == 0)
                {
                    Console.WriteLine("Medicine not found!");
                    return 0;
                }
                else return 1;
            }
        }
    }

    public static int SearchByName(string name, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicineName=@name";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@pId", pId);
                var data = command.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Console.WriteLine($"Medicine Name: {data[0]} Medicine Price: {data[1]} Medicine ProdDate: {data[2]} Medicine ExpDate: {data[3]} ");
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

    public static void SearchByPrice(double price, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from  MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicinePrice=@price";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@pId", pId);
                var data = command.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Console.WriteLine($"Medicine Name: {data[0]} Medicine Price: {data[1]} Medicine ProdDate: {data[2]} Medicine ExpDate: {data[3]} ");
                    }
                }
                else
                {
                    Console.WriteLine("Not Found!");
                }
            }
        }
    }

    public static void SearchByPurpose(int purpose, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select md.MedicineName,md.MedicinePrice from MedicineDetails as md\r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicinePurpose = @purpose";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@purpose", purpose);
                command.Parameters.AddWithValue("@pId", pId);
                SqlDataReader data = command.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Console.WriteLine($"Medicine Name: {data[0]} Medicine Price: {data[1]} ");
                    }
                }
                else Console.WriteLine("Not Found!");
            }
        }
    }

    public static int SearchByNameAndStock(string name, int stockId, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId\r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere md.MedicineName=@name and s.StockId = @stockId and p.PharmacyId = @pId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@pId", pId);
                command.Parameters.AddWithValue("@stockId", stockId);
                var data = command.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Console.WriteLine($"Medicine Name: {data[0]} Medicine Price: {data[1]} Medicine ProdDate: {data[2]} Medicine ExpDate: {data[3]} ");
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
