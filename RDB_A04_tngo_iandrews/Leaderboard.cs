/*
    FILE			: MathTest.cs
	PROGRAMMERS		: Isaiah Andrews & Tommy Ngo
    PROJECT			: PROG2111 - Assignment 4
	DESCRIPTION		: This file implements the components for the leaderboard. The methods consists of MySQL queries in order to display information
                      on the page.
*/

// using statements
using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RDB_A04_tngo_iandrews
{
    public partial class Leaderboard : Form
    {
        public Leaderboard()
        {
            InitializeComponent();

            // initialize list box view details
            leaderboardListBox.View = View.Details;
            leaderboardListBox.Columns.Add("Rank", 100);
            leaderboardListBox.Columns.Add("Name", 100);
            leaderboardListBox.Columns.Add("Score", 95);

            // variables retaining towards the MySQL API connections and queries
            string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            StringBuilder cmdSB;
            MySqlDataReader reader = null;

            // ensure connection is being used during this instance while running
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                // create query string
                cmdSB = new StringBuilder("SELECT Name, Score FROM Leaderboard ORDER BY Score DESC;");

                // create connection with query
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                // i stands for the amount of insertions needed for each rank within the leaderboard
                int i = 1;

                // try for successful connection and query handler
                try
                {
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read()) // read each value from selected query and store into listbox
                    {
                        String[] row = { i.ToString(), reader["Name"].ToString(), reader["Score"].ToString() };
                        ListViewItem item = new ListViewItem(row);
                        leaderboardListBox.Items.Add(item);
                        i++;
                    }
                }
                catch (Exception ex) // catch exception if query is not successful
                {
                    MessageBox.Show(ex.Message);
                }
                finally // finally for connection close
                {
                    connection.Close();
                }
            }
        }
    }
}