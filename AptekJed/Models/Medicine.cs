using System.Data.SqlClient;

namespace AptekJed.Models;

public class Medicine
{
    public int StockId { get; set; }
    public int Medicinee { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Type { get; set; }
    public int Purpose { get; set; }
    public DateTime ProdDate { get; set; }
    public DateTime ExpDate { get; set; }
    public static void GetMedicine()
    {
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("Enter 1 for update medicine's name:");
        Console.WriteLine("Enter 2 for update medicine's price:");
        Console.WriteLine("Enter 3 for update medicine's type:");
        Console.WriteLine("Enter 4 for update medicine's purpose:");
        Console.WriteLine("Enter 5 for update medicine's prodDate and expDate:");
        Console.WriteLine("--------------------------------------");
    }

    public static int GetTypes()
    {
        int count = 0;
        foreach (var typee in Enum.GetNames(typeof(Types)))
        {
            Console.WriteLine($"{count} - {typee}");
            count++;
        }
        return count;
    }

    public static void AddMedicine(Medicine medicine, int pId)
    {
        if (medicine.Price < 0 || medicine.Medicinee < 0)
        {
            Console.WriteLine("Values cannot be less than zero");
            return;
        }
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();

            string query = "Insert Into MedicineDetails\r\nValues\r\n(@name,@price,@type,@purpose,@prodDate,@expDate) SELECT SCOPE_IDENTITY()";
            string query2 = "insert into Medicines\r\nvalues \r\n(@medicineId,@stockId)";
            string pharmacyCount = "select count(*) from Pharmacies as p where p.PharmacyId = @pharmacyId";
            string stockCountQuery = "select COUNT(*) from Stocks Where Stocks.StockId = @stockId";
            string pharmacyStock = "select Count(*) from Stocks as s where s.StockId = @stockId and s.PharmacyId =@pharmacyId";
            string medicineCount = "select s.MedicineCount from Stocks as s\r\nwhere s.StockId = @stockId";
            string pharmacyStockCount = "select Pharmacies.StockCount from Pharmacies where Pharmacies.PharmacyId = @pharmacyId";
            string updateCountquery = "update Stocks \r\nset MedicineCount = @medicineCount\r\nwhere PharmacyId = @pharmacyId and StockId = @stockId";
            using (SqlCommand pharmacyCommand = new SqlCommand(pharmacyCount, connection))
            {
                pharmacyCommand.Parameters.AddWithValue("@pharmacyId", pId);
                var count = pharmacyCommand.ExecuteScalar();
                if ((int)count == 0)
                {
                    Console.WriteLine("PharmacyId not founded!");
                    return;
                }
                pharmacyCommand.CommandText = stockCountQuery;

                pharmacyCommand.Parameters.AddWithValue("@stockId", medicine.StockId);
                var count2 = pharmacyCommand.ExecuteScalar();
                if ((int)count2 == 0)
                {
                    Console.WriteLine("StockId not founded!");
                    return;
                }

                pharmacyCommand.CommandText = pharmacyStock;

                pharmacyCommand.Parameters["@stockId"].Value = medicine.StockId;
                pharmacyCommand.Parameters["@pharmacyId"].Value = pId;
                var count3 = pharmacyCommand.ExecuteScalar();
                if ((int)count3 == 0)
                {
                    Console.WriteLine("Not Found!");
                    return;
                }

                pharmacyCommand.CommandText = medicineCount;

                pharmacyCommand.Parameters["@stockId"].Value = medicine.StockId;
                var MedicineCount = pharmacyCommand.ExecuteScalar();

                pharmacyCommand.CommandText = pharmacyStockCount;

                pharmacyCommand.Parameters["@pharmacyId"].Value = pId;
                var pharStock = pharmacyCommand.ExecuteScalar();
                if ((int)MedicineCount + medicine.Medicinee > (int)pharStock)
                {
                    Console.WriteLine("Medicine count must be less than stock count! Choose another stock!");
                    return;
                }
                else
                {
                    int count4 = (int)MedicineCount + medicine.Medicinee;
                    pharmacyCommand.CommandText = updateCountquery;
                    pharmacyCommand.Parameters.AddWithValue("@medicineCount", count4);
                    pharmacyCommand.Parameters["@pharmacyId"].Value = pId;
                    pharmacyCommand.Parameters["@stockId"].Value = medicine.StockId;
                    int updateCount = pharmacyCommand.ExecuteNonQuery();
                    if (updateCount > 0) Console.WriteLine("Added!");
                    else Console.WriteLine("Fatal error!");
                }
                pharmacyCommand.CommandText = query;

                pharmacyCommand.Parameters.AddWithValue("@name", medicine.Name);
                pharmacyCommand.Parameters.AddWithValue("@price", medicine.Price);
                pharmacyCommand.Parameters.AddWithValue("@type", medicine.Type);
                pharmacyCommand.Parameters.AddWithValue("@purpose", medicine.Purpose);
                pharmacyCommand.Parameters.AddWithValue("@prodDate", medicine.ProdDate);
                pharmacyCommand.Parameters.AddWithValue("@expDate", medicine.ExpDate);

                var medicineId = pharmacyCommand.ExecuteScalar();
                pharmacyCommand.CommandText = query2;
                pharmacyCommand.Parameters.AddWithValue("@medicineId", medicineId);
                pharmacyCommand.Parameters["@stockId"].Value = medicine.StockId;
                var value2 = pharmacyCommand.ExecuteNonQuery();
                if (value2 > 0) Console.WriteLine("Medicine Added!");
                else Console.WriteLine("Invalid Operation!");
            }
        }
    }

    public static (double, int) SellMedicine(string name, int count, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            double price = 0;
            string query = "select Count(*) from MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicineName=@name";
            string priceQuery = "select md.MedicinePrice from MedicineDetails as md \r\njoin Medicines as m on m.MedicineId = md.MedicineId \r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicineName=@name";
            string countQuery = "select s.MedicineCount from Medicines as m\r\njoin MedicineDetails as  md on m.MedicineId = md.MedicineId\r\njoin Stocks as s on s.StockId = m.StockId\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and md.MedicineName=@name";
            string updateQuery = "Exec UptadeCountt @count , @name, @pId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@pId", pId);
                var value = command.ExecuteScalar();
                if ((int)value == 0)
                {
                    Console.WriteLine("Medicine not found!");
                    return (price, 0);
                }
                else
                {
                    command.CommandText = priceQuery;

                    command.Parameters["@name"].Value = name;
                    command.Parameters["@pId"].Value = pId;
                    var priceCo = command.ExecuteScalar();
                    price = (double)priceCo;

                    command.CommandText = countQuery;

                    command.Parameters["@name"].Value = name;
                    command.Parameters["@pId"].Value = pId;

                    var countCo = command.ExecuteScalar();
                    int countSell = (int)countCo;
                    if (count < 0)
                    {
                        Console.WriteLine("Count cannot be less than zero! ");
                        return (price, 0);
                    }
                    if (countSell < count)
                    {
                        Console.WriteLine($"Count must be less than stock count! Stock Count: {countSell} ");
                        return (price, 0);
                    }
                    else
                    {
                        command.CommandText = updateQuery;
                        command.Parameters.AddWithValue("@count", count);
                        command.Parameters["@name"].Value = name;
                        command.Parameters["@pId"].Value = pId;
                        int value2 = command.ExecuteNonQuery();
                        if (value2 == 0) Console.WriteLine("Fatal Error!");
                        return (price, 1);
                    }
                }
            }
        }
    }

    public static void UpdateName(int id, string name)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "update MedicineDetails set MedicineName = @name where MedicineId = @id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                int value = command.ExecuteNonQuery();
                if (value > 0) Console.WriteLine("Updated!");
                else Console.WriteLine("Fatal Error!");
            }
        }
    }

    public static void UpdatePrice(int id, double price)
    {
        if (price < 0)
        {
            Console.WriteLine("Price cannot be less than zero");
            return;
        }
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "update MedicineDetails set MedicinePrice = @price where MedicineId = @id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@price", price);
                int value = command.ExecuteNonQuery();
                if (value > 0) Console.WriteLine("Updated!");
                else Console.WriteLine("Fatal Error!");
            }
        }
    }

    public static void UpdateType(int id, int type, int count)
    {
        if (type >= count || type < 0)
        {
            Console.WriteLine("Fatal Error!"); return;
        }
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "update MedicineDetails set MedicineType = @type where MedicineId = @id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@type", type);
                int value = command.ExecuteNonQuery();
                if (value > 0) Console.WriteLine("Updated!");
                else Console.WriteLine("Fatal Error!");
            }
        }
    }

    public static void UpdatePurpose(int id, int purpose, int count)
    {
        if (purpose >= count || purpose < 0)
        {
            Console.WriteLine("Fatal Error!"); return;
        }
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "update MedicineDetails set MedicinePurpose = @purpose where MedicineId = @id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@purpose", purpose);
                int value = command.ExecuteNonQuery();
                if (value > 0) Console.WriteLine("Updated!");
                else Console.WriteLine("Fatal Error!");
            }
        }
    }

    public static void UpdateDate(int id, DateTime prodDate, DateTime expDate)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "update MedicineDetails set MedicineProdDate = @prodDate, MedicineExpDate = @expDate where MedicineId = @id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@prodDate", prodDate);
                command.Parameters.AddWithValue("@expDate", expDate);
                int value = command.ExecuteNonQuery();
                if (value > 0) Console.WriteLine("Updated!");
                else Console.WriteLine("Fatal Error!");
            }
        }
    }

    public static void UpdateMedicineCount(int id, int count, int pId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string queryCount = "select count(*) from Stocks as s\r\njoin Pharmacies as p on p.PharmacyId = s.PharmacyId\r\nwhere p.PharmacyId = @pId and s.StockId=@id";
            string queryUpdate = "\r\nUpdate Stocks Set MedicineCount = @count where StockId = @id and PharmacyId = @pId";
            using (SqlCommand command = new SqlCommand(queryCount, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@pId", pId);
                var value = command.ExecuteScalar();
                if ((int)value == 0) Console.WriteLine("Not Found!");
                else
                {
                    using (SqlCommand command1 = new SqlCommand(queryUpdate, connection))
                    {
                        command1.Parameters.AddWithValue("@id", id);
                        command1.Parameters.AddWithValue("@pId", pId);
                        command1.Parameters.AddWithValue("@count", count);
                        int value2 = command1.ExecuteNonQuery();
                        if (value2 > 0) Console.WriteLine("Updated!");
                        else Console.WriteLine("Fatal Error!");

                    }
                }
            }
        }
    }

    public static void UpdateMedCount(int count, string name, int pharmacyId, int stockId)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string stockQuery = "select p.StockCount from Pharmacies as p join Stocks as s on s.PharmacyId=p.PharmacyId\r\nwhere p.PharmacyId = @pharmacyId  and s.StockId = @stockId";
            string updateQuery = "Exec UptadeCount @count , @name";
            string countQuery = "select s.MedicineCount from Stocks as s join Medicines as m on m.StockId = s.StockId join MedicineDetails as md\r\non md.MedicineId = m.MedicineId where md.MedicineName = @name";
            int existsCount = 0;
            int stockCount = 0;
            int sum = 0;
            using (SqlCommand sqlcom = new SqlCommand(stockQuery, connection))
            {
                sqlcom.Parameters.AddWithValue("@pharmacyId", pharmacyId);
                sqlcom.Parameters.AddWithValue("@stockId", stockId);
                var stock = sqlcom.ExecuteScalar();
                stockCount = (int)stock;

                sqlcom.CommandText = countQuery;
                sqlcom.Parameters.AddWithValue("@name", name);
                var value = sqlcom.ExecuteScalar();
                existsCount = (int)value;

                sum = existsCount + count;

                if (sum > stockCount)
                {
                    Console.WriteLine("Medicine count must be less than stock count! Choose another stock!");
                    return;
                }
                else
                {
                    sqlcom.CommandText = updateQuery;
                    sqlcom.Parameters.AddWithValue("@count", sum);
                    sqlcom.Parameters["@name"].Value = name;
                    int valuee = sqlcom.ExecuteNonQuery();
                    if (valuee > 0) Console.WriteLine("Updated");
                    else Console.WriteLine("Fatal Error!");
                }
            }
        }
    }

    public static int GetPurposes()
    {
        int count = 0;
        foreach (var item in Enum.GetNames(typeof(Purposes)))
        {
            Console.WriteLine($"{count} - {item}");
            count++;
        }
        return count;
    }

    public enum Types
    {
        None = 0,
        Liquid = 1,
        Tablet = 2,
        Capsule = 3,
        Injection = 4
    }

    public enum Purposes
    {
        None = 0,
        Headache = 1,
        Temperature = 2
    }
}
