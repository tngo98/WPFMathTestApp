/*
    FILE			: MathTest.cs
	PROGRAMMERS		: Isaiah Andrews & Tommy Ngo
    PROJECT			: PROG2111 - Assignment 4
	DESCRIPTION		: This file implements the programmatic game logic with in each component of the UI of the actual survey. It contains all the methods
                      in order to program each component and to allow validation of user submission.
*/

// using statements
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RDB_A04_tngo_iandrews
{
    public partial class MathTest : Form
    {
        private User user; // The user object contains session information
        private int[] timesTaken = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //an array of ints holding the score of each of the questions
        private bool[] isCorrect = { false, false, false, false, false, false, false, false, false, false }; //an array of bools holding the 'correct/incorrect' status of each question
        private int questionState = 0;
        private static int hh, mm, ss;
        private static int TimeAllSeconds = 20;

        public MathTest(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        /*
           FUNCTION: MathTest_Load
           PURPOSE: This handler handles the loading of the
           PARAMETER:  object sender, EventArgs e
        */

        private void MathTest_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            comboBox1.Enabled = true;

            string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            StringBuilder cmdSB;
            MySqlDataReader reader = null;
            List<Label> questionLbls = new List<Label>();
            List<ComboBox> questionCmboBxs = new List<ComboBox>();

            // insert all of the labels into a list
            questionLbls.Add(q1lbl);
            questionLbls.Add(q2lbl);
            questionLbls.Add(q3lbl);
            questionLbls.Add(q4lbl);
            questionLbls.Add(q5lbl);
            questionLbls.Add(q6lbl);
            questionLbls.Add(q7lbl);
            questionLbls.Add(q8lbl);
            questionLbls.Add(q9lbl);
            questionLbls.Add(q10lbl);

            // insert all of the combo boxes into a list
            questionCmboBxs.Add(comboBox1);
            questionCmboBxs.Add(comboBox2);
            questionCmboBxs.Add(comboBox3);
            questionCmboBxs.Add(comboBox4);
            questionCmboBxs.Add(comboBox5);
            questionCmboBxs.Add(comboBox6);
            questionCmboBxs.Add(comboBox7);
            questionCmboBxs.Add(comboBox8);
            questionCmboBxs.Add(comboBox9);
            questionCmboBxs.Add(comboBox10);

            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                cmdSB = new StringBuilder("SELECT Question FROM Questions;"); // query the database for all of the questions from the test
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                try
                {
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        questionLbls[i].Text = reader["Question"].ToString();// replace all of the question label with the questions from the database
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                for (int i = 1; i <= 10; i++)
                {
                    cmdSB = new StringBuilder(
                        "INSERT INTO Answers SELECT ActualAnswer FROM ActualAnswer WHERE QuestionID=" + i + " UNION SELECT PotentialAnswer FROM PotentialAnswers WHERE QuestionID=" + i + "; "
                         + "SELECT Answers FROM Answers ORDER BY RAND(); DELETE FROM Answers;"); // query for inserting, selecting, and deleting answers from answer table
                    cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                    try
                    {
                        connection.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            questionCmboBxs[i - 1].Items.Add(reader["Answers"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /*
           FUNCTION: sbmtBtn_Click
           PURPOSE: This handler handles the submitting of the quiz results.
           PARAMETER:  object sender, EventArgs e
        */

        private void sbmtBtn_Click(object sender, EventArgs e)
        {
            string conStr = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            StringBuilder cmdSB;
            int score = 0;

            List<ComboBox> questionCmboBxs = new List<ComboBox>();
            questionCmboBxs.Add(comboBox1);
            questionCmboBxs.Add(comboBox2);
            questionCmboBxs.Add(comboBox3);
            questionCmboBxs.Add(comboBox4);
            questionCmboBxs.Add(comboBox5);
            questionCmboBxs.Add(comboBox6);
            questionCmboBxs.Add(comboBox7);
            questionCmboBxs.Add(comboBox8);
            questionCmboBxs.Add(comboBox9);
            questionCmboBxs.Add(comboBox10);

            if (questionCmboBxs[0].Text == "" || questionCmboBxs[1].Text == "" || questionCmboBxs[2].Text == "" || questionCmboBxs[3].Text == "" || questionCmboBxs[4].Text == "" || questionCmboBxs[5].Text == "" || questionCmboBxs[6].Text == "" || questionCmboBxs[7].Text == "" || questionCmboBxs[8].Text == "" || questionCmboBxs[9].Text == "")
            {
                questionError.Visible = true;
            }
            else
            {
                timer1.Stop(); // stop the timer if every question has been answered
                using (MySqlConnection connection = new MySqlConnection(conStr))
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        cmdSB = new StringBuilder("SELECT ActualAnswer FROM ActualAnswer WHERE QuestionID=" + i + ";");// Query the database for the correct answer
                        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(cmdSB.ToString(), connection);

                        try
                        {
                            connection.Open();
                            int mysqlint = int.Parse(cmd.ExecuteScalar().ToString());

                            if (int.Parse(questionCmboBxs[i - 1].Text) == mysqlint)// compare the acutal answer to the answer given
                            {
                                isCorrect[i - 1] = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (timesTaken[i - 1] != 0 && isCorrect[i - 1] == true)// if the user did not take longer than 20 seconds and their answer was correct, then...
                        {
                            score += timesTaken[i - 1]; // award points; the more time left on the clock the higher the score.
                        }
                        else
                        {
                            score += 0;
                        }
                    }
                }

                user.submitUser(score);// call the submitUser method with the calculated score

                // open the leaderboard form and hide the test window
                Leaderboard form = new Leaderboard();
                form.Show();
                this.Hide();
            }
        }

        /*
           FUNCTION: timer1_Tick
           PURPOSE: This handler handles the timer, and the state control logic.
           PARAMETER:  object sender, EventArgs e
        */

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TimeAllSeconds > 0)
            {
                TimeAllSeconds = TimeAllSeconds - 1;
            }

            TimeSpan time_Span = TimeSpan.FromSeconds(TimeAllSeconds);
            hh = time_Span.Hours;
            mm = time_Span.Minutes;
            ss = time_Span.Seconds;

            timer.Text = ":" + ss;

            switch (questionState)
            {
                case 0: //Question 1
                    comboBox1.Enabled = true;
                    break;

                case 1://Question 2
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = true;
                    break;

                case 2://Question 3
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = true;
                    break;

                case 3://Question 4
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = true;
                    break;

                case 4://Question 5
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = true;
                    break;

                case 5://Question 6
                    comboBox5.Enabled = false;
                    comboBox6.Enabled = true;
                    break;

                case 6://Question 7
                    comboBox6.Enabled = false;
                    comboBox7.Enabled = true;
                    break;

                case 7://Question 8
                    comboBox7.Enabled = false;
                    comboBox8.Enabled = true;
                    break;

                case 8://Question 9
                    comboBox8.Enabled = false;
                    comboBox9.Enabled = true;
                    break;

                case 9://Question 10
                    comboBox9.Enabled = false;
                    comboBox10.Enabled = true;
                    break;

                case 10:// Submit completed quiz state
                    comboBox10.Enabled = false;
                    sbmtBtn.Enabled = true;
                    timer.Visible = false;
                    gameOverlbl.Visible = true;
                    break;
            }
        }

        /*
           FUNCTION: comboBox_SelectedIndexChanged
           PURPOSE: This handler handles the advancing of the game state, and the starting/stopping of the timer.
           PARAMETER:  object sender, EventArgs e
        */

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            timesTaken[questionState] = TimeAllSeconds;
            questionState++;
            this.timer1.Stop();
            TimeAllSeconds = 21;
            this.timer1.Enabled = true;
        }
    }
}