using AptekJed.Models;

namespace AptekJed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int pharmacyId;
            int answer = 0;
            string name = null;
            string pharma;
            int p = 0;
            Pharmacy.GetPharmacies();
            do
            {
                Console.Write("Enter Pharmacy Name: ");
                pharma = Console.ReadLine();
                var ph = Pharmacy.FindPharmacy(pharma);
                p = ph.Item1;
                pharmacyId = ph.Item2;
            } while (p == 0);
            do
            {
                Menu.GetMenu();
                string answerS;
                do
                {
                    Console.Write("Choose operation: ");
                    answerS = Console.ReadLine();
                } while (!int.TryParse(answerS, out answer));
                switch (answer)
                {
                    case 1:
                        Console.Clear();
                        Menu.Info(pharmacyId);
                        break;
                    case 2:
                        double value;
                        string valueS;
                        Console.Clear();
                        do
                        {
                            Console.WriteLine("Enter 1 for search by name ");
                            Console.WriteLine("Enter 2 for search by price ");
                            Console.WriteLine("Enter 3 for search by purpose ");
                            valueS = Console.ReadLine();
                        } while (!double.TryParse(valueS, out value));
                        switch (value)
                        {
                            case 1:
                                Console.Write("Enter medicine's name: ");
                                name = Console.ReadLine();
                                Stock.SearchByName(name, pharmacyId);
                                break;
                            case 2:
                                double price;
                                string priceS;
                                do
                                {
                                    Console.Write("Enter medicine's price: ");
                                    priceS = Console.ReadLine();
                                } while (!double.TryParse(priceS, out price));
                                Stock.SearchByPrice(price, pharmacyId);
                                break;
                            case 3:
                                int count = 1;
                                int purpose;
                                string purposeS;
                                foreach (var item in Enum.GetNames(typeof(Medicine.Purposes)))
                                {
                                    if (item != "None")
                                    {
                                        Console.WriteLine($"{count} - {item}");
                                        count++;
                                    }
                                }
                                do
                                {
                                    Console.Write("Enter medicine's purpose: ");
                                    purposeS = Console.ReadLine();
                                } while (!int.TryParse(purposeS, out purpose));
                                Stock.SearchByPurpose(purpose, pharmacyId);
                                break;
                            default:
                                Console.WriteLine("Fatal Error! ");
                                break;
                        }
                        break;
                    case 3:
                        int stock;
                        int medicine;
                        string stockS;
                        string medicineS;
                        Console.Clear();
                        int s = Stock.GetStock(pharmacyId);
                        if (s == 0) break;
                        do
                        {
                            Console.Write("Enter stockId: ");
                            stockS = Console.ReadLine();
                        } while (!int.TryParse(stockS, out stock));
                        do
                        {
                            Console.Write("Enter medicine count: ");
                            medicineS = Console.ReadLine();
                        } while (!int.TryParse(medicineS, out medicine));
                        Console.Write("Enter medicine's name: ");
                        name = Console.ReadLine();
                        int co = Stock.SearchByNameAndStock(name, stock, pharmacyId);
                        if (co == 0)
                        {
                            int type;
                            int t;
                            int purpose;
                            double price;
                            string priceS;
                            string prodDateS;
                            string expDateS;
                            DateTime prodDate;
                            DateTime expDate;
                            do
                            {
                                Console.Write("Enter medicine's price: ");
                                priceS = Console.ReadLine();
                            } while (!double.TryParse(priceS, out price));
                            do
                            {
                                Console.Write("Enter medicine's prodDate: ");
                                prodDateS = Console.ReadLine();
                            } while (!DateTime.TryParse(prodDateS, out prodDate));
                            do
                            {
                                Console.Write("Enter medicine's expDate: ");
                                expDateS = Console.ReadLine();
                            } while (!DateTime.TryParse(expDateS, out expDate));
                            do
                            {
                                string typeS;
                                t = Medicine.GetTypes();
                                do
                                {
                                    Console.Write("Enter medicine's type: ");
                                    typeS = Console.ReadLine();
                                } while (!int.TryParse(typeS, out type));
                            } while (!Menu.Check(type, t));
                            do
                            {
                                string purposeS;
                                p = Medicine.GetPurposes();
                                do
                                {
                                    Console.Write("Enter medicine's purpose: ");
                                    purposeS = Console.ReadLine();
                                } while (!int.TryParse(purposeS, out purpose));
                            } while (!Menu.Check(purpose, p));
                            Medicine newMedicine = new Medicine()
                            {
                                Name = name,
                                Price = price,
                                Type = type,
                                Purpose = purpose,
                                ProdDate = prodDate,
                                ExpDate = expDate,
                                StockId = stock,
                                Medicinee = medicine
                            };
                            Medicine.AddMedicine(newMedicine, pharmacyId);
                        }
                        else
                        {
                            Medicine.UpdateMedCount(medicine, name, pharmacyId, stock);
                        }
                        break;
                    case 4:
                        Console.Clear();
                        int i = Menu.Info(pharmacyId);
                        if (i == 0) break;
                        Console.Write("Enter medicine name for buy: ");
                        name = Console.ReadLine();
                        do
                        {
                            Console.Write("Enter medicine's count: ");
                            stockS = Console.ReadLine();
                        } while (!int.TryParse(stockS, out stock));
                        var sell = Medicine.SellMedicine(name, stock, pharmacyId);
                        value = sell.Item1;
                        if (sell.Item2 != 0)
                        {
                            Console.WriteLine($"You will pay {value * stock}$");
                        }
                        break;
                    case 5:
                        Console.Clear();
                        string medicineAnsS;
                        int medicineAns;
                        do
                        {
                            Console.WriteLine("Enter 1 for update medicine: ");
                            Console.WriteLine("Enter 2 for update stock: ");
                            medicineAnsS = Console.ReadLine();
                        } while (!int.TryParse(medicineAnsS, out medicineAns));
                        switch (medicineAns)
                        {
                            case 1:
                                int purpose;
                                int id;
                                int ans;
                                string purposeS;
                                string idS;
                                Console.Clear();
                                i = Menu.Info(pharmacyId);
                                if (i == 0) break;
                                do
                                {
                                    Console.Write("Enter MedicineId for Update: ");
                                    idS = Console.ReadLine();
                                } while (!int.TryParse(idS, out id));
                                ans = Stock.SearchByMedicineId(id, pharmacyId);
                                if (ans == 0) break;
                                Console.Clear();
                                do
                                {
                                    Medicine.GetMedicine();
                                    purposeS = Console.ReadLine();
                                } while (!int.TryParse(purposeS, out purpose));
                                switch (purpose)
                                {
                                    case 1:
                                        Console.Write("Enter medicine's new name: ");
                                        name = Console.ReadLine();
                                        Medicine.UpdateName(id, name);
                                        break;
                                    case 2:
                                        double price;
                                        string priceS;
                                        do
                                        {
                                            Console.Write("Enter medicine's new price: ");
                                            priceS = Console.ReadLine();
                                        } while (!double.TryParse(priceS, out price));
                                        Medicine.UpdatePrice(id, price);
                                        break;
                                    case 3:
                                        int type;
                                        int t = Medicine.GetTypes();
                                        string typeS;
                                        do
                                        {
                                            Console.Write("Enter medicine's new type: ");
                                            typeS = Console.ReadLine();
                                        } while (!int.TryParse(typeS, out type));
                                        Medicine.UpdateType(id, type, t);
                                        break;
                                    case 4:
                                        int v = Medicine.GetPurposes();
                                        do
                                        {
                                            Console.Write("Enter medicine's purpose: ");
                                            purposeS = Console.ReadLine();
                                        } while (!int.TryParse(purposeS, out purpose));
                                        Medicine.UpdatePurpose(id, purpose, v);
                                        break;
                                    case 5:
                                        DateTime expDate;
                                        DateTime prodDate;
                                        string prodDateS;
                                        string expDateS;
                                        do
                                        {
                                            Console.Write("Enter medicine's prodDate: ");
                                            prodDateS = Console.ReadLine();
                                        } while (!DateTime.TryParse(prodDateS, out prodDate));
                                        do
                                        {
                                            Console.Write("Enter medicine's expDate: ");
                                            expDateS = Console.ReadLine();
                                        } while (!DateTime.TryParse(expDateS, out expDate));
                                        Medicine.UpdateDate(id, prodDate, expDate);
                                        break;
                                    default:
                                        break;
                                }

                                break;
                            case 2:
                                int oldId;
                                string oldIdS;
                                Console.Clear();
                                Console.WriteLine("=====Update Medicine Count=====");
                                s = Stock.GetStock(pharmacyId);
                                if (s == 0) break;
                                do
                                {
                                    Console.Write("Enter StockId for update: ");
                                    oldIdS = Console.ReadLine();
                                } while (!int.TryParse(oldIdS, out oldId));
                                do
                                {
                                    Console.Write("Enter medicine count: ");
                                    medicineS = Console.ReadLine();
                                } while (!int.TryParse(medicineS, out medicine));
                                Medicine.UpdateMedicineCount(oldId, medicine, pharmacyId);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 6:
                        Pharmacy.GetPharmacies();
                        do
                        {
                            Console.Write("Enter Pharmacy Name: ");
                            pharma = Console.ReadLine();
                            var ph = Pharmacy.FindPharmacy(pharma);
                            p = ph.Item1;
                            pharmacyId = ph.Item2;
                        } while (p == 0);
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            } while (answer != 0);
        }
    }
}
