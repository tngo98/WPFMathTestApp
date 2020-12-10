/*
    FILE			: Program.cs
	PROGRAMMERS		: Isaiah Andrews & Tommy Ngo
    PROJECT			: PROG2111 - Assignment 4
	DESCRIPTION		: This project creates a game that after asking a user for their name, gives them a
                      10 question math test to answer. Depending on how long it takes for the user to
                      answer they will get a score. Finally, their final score will be committed to a
                      leaderboard so they can compare their high score to other players.
*/

using System;
using System.Windows.Forms;

namespace RDB_A04_tngo_iandrews
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Start());
        }
    }
}