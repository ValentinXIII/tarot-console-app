using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Tarotdatabase // database for storing tarot card information and drawing cards
{
    internal class Program
    {
        static void Main()
        {

            // here we create the new database, tarotdeck.db
            using (var connection = new SqliteConnection("Data Source = tarotdeck.db"))
            {
                // opening connection
                connection.Open();

                // method call for creating the needed tables for database, currently tarotdeck.db with just Cards table
                CreateTables(connection);

                static void CreateTables(SqliteConnection connection)
                {
                    // here we create a database for the cards of the tarot deck:
                    var createTableCmd = connection.CreateCommand();

                    createTableCmd.CommandText = "CREATE TABLE IF NOT EXISTS Cards (id INTEGER PRIMARY KEY, name TEXT NOT NULL, meaning TEXT NOT NULL)";
                    createTableCmd.ExecuteNonQuery();


                    while (true)
                    {
                        Console.WriteLine("What do you wish to do? (1) Draw a card (2) List all cards (3) Quit");
                        string? input = Console.ReadLine();

                        switch (input)
                        {
                            case "1":
                                DrawCard(connection);
                                break;

                            case "2":
                                ListCards(connection).ForEach(Console.WriteLine);
                                break;

                            case "3":
                                Console.WriteLine("Thank you for using the tarot reading app and have a blessed day!");
                                connection.Close();
                                Environment.Exit(0);
                                break;
                        }
                    }

                }


            }

            static void DrawCard(SqliteConnection connection)
            {
                // Query to retrieve a random card from the database
                string query = "SELECT name, meaning FROM Cards ORDER BY RANDOM() LIMIT 1";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader["name"].ToString();
                            string meaning = reader["meaning"].ToString();

                            // Print the random card information
                            Console.WriteLine($"Name: {name}");
                            Console.WriteLine($"Meaning: {meaning}");
                        }
                        else
                        {
                            Console.WriteLine("No cards found in the database.");
                        }
                    }
                }
            }
        }

        static List<string> ListCards(SqliteConnection connection)
        {
            // here we print out the current list of cards in the database
            List<string> cards = new List<string>();
            var selectSql = connection.CreateCommand();
            selectSql.CommandText = "SELECT name FROM Cards";
            var cardList = selectSql.ExecuteReader();

            while (cardList.Read())
            {
                cards.Add(cardList.GetString(0));
            }

            return cards;
        }


    }
}

