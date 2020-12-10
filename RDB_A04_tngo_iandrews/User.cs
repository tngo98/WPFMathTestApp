/*
    FILE			: User.cs
	PROGRAMMERS		: Isaiah Andrews & Tommy Ngo
    PROJECT			: PROG2111 - Assignment 4
	DESCRIPTION		: This file implements the user class. This calls holds information regarding the game session during the game.
*/

// using statements
using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RDB_A04_tngo_iandrews
{
    public class User
    {
        // properties
        private Guid UID;

        private string Name;
        private int Score;
        private bool newUser = true;

        /*
         * CONSTRUCTOR
         * Function: User()
         * Purpose: To construct a user object
         * Parameters: string name: the name of the user
         * Returns: NONE
         */

        public User(string Name)
        {
            // variables retaining towards the MySQL API connections and queries
            string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            // create query string
            StringBuilder cmdSB = new StringBuilder("SELECT COUNT(*) FROM Leaderboard WHERE Name='" + this.Name + "';");

            // assign name
            this.Name = Name;

            // variable declaration for storing query
            int mysqlint = 0;

            // ensure connection is being used during this instance while running
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                // create connection with query
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                // try for opening connection and storing query
                try
                {
                    connection.Open();
                    mysqlint = int.Parse(cmd.ExecuteScalar().ToString());
                }
                catch (Exception e) // catch exception if query is unsuccessful
                {
                    MessageBox.Show(e.Message);
                }
                finally // finally for closing connection
                {
                    connection.Close();
                }

                if (mysqlint == 1) // if there is an existing user
                {
                    // set new uer to false
                    newUser = false;

                    // clear and remake query string
                    cmdSB.Clear();
                    cmdSB.Append("SELECT Score FROM Leaderboard WHERE Name='" + this.Name + "';");

                    // create connection with query
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                    try // try for opening connection and storing query
                    {
                        connection.Open();
                        mysqlint = int.Parse(cmd.ExecuteScalar().ToString());
                        this.Score = mysqlint;
                    }
                    catch (Exception e) // catch exception if query is unsuccessful
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally // finally for closing connection
                    {
                        connection.Close();
                    }

                    // clear and remake query string
                    cmdSB.Clear();
                    cmdSB.Append("SELECT GUID FROM Leaderboard WHERE Name='" + this.Name + "';");

                    // create connection with query
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                    try // try for opening connection and storing query
                    {
                        connection.Open();
                        string mysqlstr = cmd.ExecuteScalar().ToString();
                        this.UID = Guid.Parse(mysqlstr);
                    }
                    catch (Exception e) // catch exception if query is unsuccessful
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally // finally for closing connection
                    {
                        connection.Close();
                    }
                }
                else // if there is a new user
                {
                    // create new user id
                    this.UID = Guid.NewGuid();

                    // set score to 0
                    this.Score = 0;
                }
            }
        }

        /*
         * Function: submitUser()
         * Purpose: To submit user information using MySQL queries
         * Parameters: int score: current score of user
         * Returns: NONE
         */

        public void submitUser(int score)
        {
            // variables retaining towards the MySQL API connections and queries
            string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            StringBuilder cmdSB;

            // if new score is greater than current score, set current score to new score
            if (score > this.Score)
            {
                this.Score = score;
            }

            // ensure connection is being used during this instance while running
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                // switch statement for query string if user is existing
                switch (newUser)
                {
                    case true:
                        cmdSB = new StringBuilder("INSERT INTO Leaderboard (GUID, Name, Score) VALUES ('" + this.UID.ToString() + "', '" + this.Name + "', " + this.Score.ToString() + ");");
                        break;

                    case false:
                        cmdSB = new StringBuilder("UPDATE Leaderboard SET Score=" + this.Score.ToString() + " WHERE Name='" + this.Name + "';");
                        break;

                    default:
                        cmdSB = new StringBuilder("INSERT INTO Leaderboard (GUID, Name, Score) VALUES ('" + this.UID.ToString() + "', '" + this.Name + "', " + this.Score.ToString() + ");");
                        break;
                }

                // create connection with query
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                try // try for opening connection and executing query
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) // catch exception if query is unsuccessful
                {
                    MessageBox.Show(e.Message);
                }
                finally // finally for closing connection
                {
                    connection.Close();
                }
            }
        }
    }
}