/*
    FILE			: Start.cs
	PROGRAMMERS		: Isaiah Andrews & Tommy Ngo
    PROJECT			: PROG2111 - Assignment 4
	DESCRIPTION		: This file implements the log-in screen of the project. When a name is entered
                      a new 'User' object will be created and sent along to the next form.
*/

using System;
using System.Windows.Forms;

namespace RDB_A04_tngo_iandrews
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (nameBox.Text == "")
            {
                nameError.Visible = true;
            }
            else
            {
                MathTest form = new MathTest(new User(nameBox.Text));
                form.Show();
                this.Hide();
            }
        }
    }
}