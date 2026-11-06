using Dapper;
using MySql.Data.MySqlClient;

namespace GruppProjekt;

public class UserInterface
{

    public void Start()
    {

        while (true)
        {
            Console.WriteLine("Hej vad vill du göra?");
            Console.WriteLine("1. Hyra en film");
            Console.WriteLine("Hej vad vill du göra?");
            Console.WriteLine("Hej vad vill du göra?");
            Console.WriteLine("7. Avsluta");

            Console.Write("Välj: ");
            string val = Console.ReadLine();

            switch (val)
            {
                case "1":
                    Repository repository1 = new();
                    repository1.RentMovie();
                    break;
                case "2":
                    return;
                case "3":
                    return;
                case "4":
                    return;
                case "5":
                    return;
                case "6":
                    return;
                case "7":
                    System.Console.WriteLine("avslutar");
                    Thread.Sleep(2000);
                    return;
            }
        }

    }
}