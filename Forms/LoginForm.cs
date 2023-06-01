﻿using Guna.UI2.WinForms;
using KSCS.Class;
using KSCS.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Net.Mime.MediaTypeNames;


namespace KSCS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        //호버 되었을 때 마우스 모양 변경-------------------------------
        private void btnCursorHand_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnCursorHand_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor= Cursors.Default;
        }
        //--------------------------------------------------------------


        private void tbStdNum_KeyDown(object sendder,KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblMsg.Text))
                lblMsg.Text = "";
        }
        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblMsg.Text))
                lblMsg.Text = "";
;            // PW 에 Enter Key가 입력되면 btnLogin_Click 을 호출
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin_Click(sender, e);
                btnLogin.Select();
            }
        }

        private bool EmptyCheck()
        {
            if (String.IsNullOrEmpty(tbStdNum.Text))
            {
                lblMsg.Text = "학번을 입력해 주세요";
                tbStdNum.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(tbPassword.Text))
            {
                lblMsg.Text = "비밀번호를 입력해 주세요";
                tbPassword.Focus();
                return false;
            }

            return true;
        }

        //로그인------------------------------------------------------------
        //호출 후, 로그인 성공 시, MainForm Load 실패 시, 유지
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string ID = tbStdNum.Text;
            string PW = tbPassword.Text;
            if (EmptyCheck())
            {
                LoadingForm loadingForm = new LoadingForm();
                loadingForm.Show();

                bool login = await Task.Run(() => KLAS.LoginKLAS(ID, PW));

                loadingForm.Invoke((MethodInvoker)delegate
                {
                    if (login) //KLAS 로그인
                    {
                        Database.CreateData();

                        this.Cursor = Cursors.Default;
                        this.DialogResult = DialogResult.OK;
                        loadingForm.Close();
                        this.Close();
                    }
                    else
                    {
                        lblMsg.Text = "죄송합니다. 로그인할 수 없습니다.";
                        tbPassword.Focus();
                        loadingForm.Close();
                    }
                });
            }
        }

        private void tbStdNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 500);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
               (Screen.PrimaryScreen.Bounds.Width - this.Size.Width) / 2,
               (Screen.PrimaryScreen.Bounds.Height - this.Size.Height) / 2
            );

            guna2PictureBox2.Visible = false;
            guna2PictureBox3.Visible = false;
            guna2PictureBox4.Visible = false;

            Random random = new Random();
            int randomNumber = random.Next(1, 5); // 1부터 4 사이의 난수 생성

            switch (randomNumber)
            {
                case 1:
                    guna2PictureBox1.Image = guna2PictureBox1.Image;
                    break;
                case 2:
                    guna2PictureBox1.Image = guna2PictureBox2.Image;
                    break;
                case 3:
                    guna2PictureBox1.Image = guna2PictureBox3.Image;
                    break;
                case 4:
                    guna2PictureBox1.Image = guna2PictureBox4.Image;
                    break;
            }
        }
    }
}
