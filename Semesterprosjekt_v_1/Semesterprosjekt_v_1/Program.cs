using Npgsql;

namespace Semesterprosjekt_v_1
{
    internal class Program
    {
        // Informasjon for å koble til databasen
        static string databaseID = "h597319";                   // Database & Host
        static string databasePassord = "4le4i4ct4est";         // Password to database
        static string databaseIP = "20.56.240.122";             // IP address of database

        static void Main(string[] args)
        {
        // Tilkoblingsstreng for PostgreSQL over TCP/IP

        string connectionString = string.Format("Host={0};Username={1};Password={2};Database={1};", databaseIP, databaseID, databasePassord);
        using var dataSource = NpgsqlDataSource.Create(connectionString);

            Console.Clear();
            Console.WriteLine("Starter programmet ...");
            Console.WriteLine();
            Console.WriteLine("Mulige handlinger:");
            Console.WriteLine();
            Console.WriteLine("1\t-\tVis alle brukere");
            Console.WriteLine("2\t-\tLegg til bruker");
            Console.WriteLine("3\t-\tEndre bruker");
            Console.WriteLine("4\t-\tSlett bruker");
            Console.WriteLine();
            Console.Write("Velg handling [1/2/3/4]: ");
            int z = Convert.ToInt32(Console.ReadLine());

            // switch-case datastruktur som implementerer forskjellige postgreSQL-funksjoner overfor databasen
            switch (z)
            {
                case 1:
                    // SELECT fornavn, etternavn, epost
                    using (var cmd = dataSource.CreateCommand("SELECT fornavn, etternavn, epost, kortid FROM bruker"))
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Fornavn*****Etternavn*****Epost*****KortID");
                        Console.WriteLine("------------------------------------------");

                        while (reader.Read())
                        {
                            string fornavn = reader.GetString(0);
                            string etternavn = reader.GetString(1);
                            string epost = reader.GetString(2);
                            string kortid = reader.GetString(3);

                            Console.WriteLine($"{fornavn}*****{etternavn}*****{epost}*****{kortid}");
                        }
                    }
                    break;
                case 2:
                    // INSERT some data
                    Console.Write("Skriv inn fornavn: ");
                    string fornavnInput = Console.ReadLine();

                    Console.Write("Skriv inn etternavn: ");
                    string etternavnInput = Console.ReadLine();

                    Console.Write("Skriv inn epost: ");
                    string epostInput = Console.ReadLine();

                    Console.Write("Skriv inn pinkode: ");
                    int pinkodeInput = Convert.ToInt16(Console.ReadLine());

                    using (var cmd = dataSource.CreateCommand("INSERT INTO bruker (fornavn, etternavn, epost, pinkode) VALUES (@fornavn, @etternavn, @epost, @pinkode) RETURNING kortid"))
                    {
                        cmd.Parameters.AddWithValue("fornavn", fornavnInput);
                        cmd.Parameters.AddWithValue("etternavn", etternavnInput);
                        cmd.Parameters.AddWithValue("epost", epostInput);
                        cmd.Parameters.AddWithValue("pinkode", pinkodeInput);

                        // Utfører spørringen og henter kortID generert av databasen
                        int generatedKortID = (int)cmd.ExecuteScalar();
                        Console.WriteLine($"Ny bruker lagt til med kortID: {generatedKortID}");
                    }
                    break;
            }
        }   // End of Main()
    }
}
