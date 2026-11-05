using System;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;



public class Repository
{

    string connectionString =
    """
    Server=comet-direct.usbx.me;
            Port=17815;
            Database=yh_max_Videoteket;
            User=yh_max;
            Password=C7AKQfWB#sha7n;
    """;


    public void CheckDateAndPrice()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    UPDATE Movie
                    SET PricePerDay = CASE
                    WHEN ReleaseDate >= '2000-01-01' THEN 47
                    ELSE 27
                    END;
                    """;
        db.Execute(sql);
    }
}