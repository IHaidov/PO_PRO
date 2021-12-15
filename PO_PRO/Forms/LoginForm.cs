﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using PO_PRO.Forms;

namespace PO_PRO
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Paint += new PaintEventHandler(set_background);
            
        }

        private void set_background(Object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            //the rectangle, the same size as our Form
            Rectangle gradient_rectangle = new Rectangle(0, 0, Width, Height);

            //define gradient's properties
            Brush b = new LinearGradientBrush(gradient_rectangle, ColorTranslator.FromHtml("#2D3436"), ColorTranslator.FromHtml("#000000"), 75f);

            //apply gradient         
            graphics.FillRectangle(b, gradient_rectangle);
        }

        private void onClick_LogIn(object sender, EventArgs e)
        {
            Login();
        }

        private void onClick_Register(object sender, EventArgs e)
        {
            MessageBox.Show("Register");
        }
        private void onClick_ClearFields(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void onClick_Exit(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void KeyDown_Form(object sender, KeyEventArgs e)
        {
            //ESC - exit an application
            if (e.KeyData == Keys.Escape)
            {
                ExitApplication();
            }
        }
        private void KeyDown_Text(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Login();
            }
            if (e.KeyData == Keys.Escape)
            {
                ExitApplication();
            }
        }

        private void ExitApplication()
        {
            Application.Exit();
        }

        private void Login()
        {
            if (txtPassword.Text == "1234" && txtUsername.Text == "user")
            {
                new UserForm().Show();
                this.Hide();
                //MessageBox.Show("User");
            }

            else if (txtPassword.Text == "1234" && txtUsername.Text == "admin")
            {
                new AdminForm().Show();
                this.Hide();
                //MessageBox.Show("Admin");
            }

            else
            {
                MessageBox.Show("The password or username is not correct");
                //txtUsername.Focus();
            }
            ClearFields();
        }

        private void ClearFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
