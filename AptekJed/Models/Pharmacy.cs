using System.Data.SqlClient;

namespace AptekJed.Models;

public class Pharmacy
{
    public static void GetPharmacies()
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select p.PharmacyId, p.PharmacyName,p.PharmacyAddress,p.PharmacyPhone from Pharmacies as p ";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                SqlDataReader data = command.ExecuteReader();
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Console.WriteLine($"Pharmacy Id: {data[0]} Pharmacy Name: {data[1]} Pharmacy Address: {data[2]}  Pharmacy Phone: {data[3]}");
                    }
                }
                else Console.WriteLine("Not Found!");
            }
        }
    }

    public static (int, int) FindPharmacy(string name)
    {
        using (SqlConnection connection = new SqlConnection(Configure.CONNECTION_STRING))
        {
            connection.Open();
            string query = "select Count(*) from Pharmacies as p where p.PharmacyName = @name";
            string findIdQuery = "select p.PharmacyId from Pharmacies as p where p.PharmacyName = @name";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                var count = command.ExecuteScalar();
                if ((int)count > 0)
                {
                    Console.WriteLine($"Welcome to {name.ToUpper()} Pharmacy!");
                    command.CommandText = findIdQuery;
                    command.Parameters["@name"].Value = name;
                    var val = command.ExecuteScalar();
                    int pharmacyId = (int)val;
                    return (1, pharmacyId);
                }
                else return (0, 0);
            }
        }
    }
}
