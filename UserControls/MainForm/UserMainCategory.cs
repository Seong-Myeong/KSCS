﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using KSCS.Forms;


namespace KSCS
{
    public partial class UserMainCategory : UserControl
    {
        public UserMainCategory()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            flpSubCategory.Visible = !flpSubCategory.Visible;
        }

        private void UserMainCategory_Load(object sender, EventArgs e)
        {
            txtMainCategory.Visible = true;
            lblMainCategory.Visible = false;
            txtMainCategory.Text = lblMainCategory.Text;
            txtMainCategory.Focus();

        }

        private void lblMainCategory_DoubleClick(object sender, EventArgs e)
        {
            txtMainCategory.Visible = true;
            lblMainCategory.Visible = false;
            txtMainCategory.BringToFront();
            txtMainCategory.Focus();
            txtMainCategory.Text = lblMainCategory.Text;
        }

        private void lblMainCategory_Leave(object sender, EventArgs e)
        {
            txtMainCategory.Visible = false;
            lblMainCategory.Visible = true;
            txtMainCategory.Clear();
            if (txtMainCategory.Text.Length == 0)
            {
                ((FlowLayoutPanel)this.Parent).Controls.Remove(this);
            }
        }

        private void txtMainCategory_KeyDown(object sender, KeyEventArgs e)
        {
            //카테고리 이름 변경 사항 저장
            if (e.KeyCode == Keys.Enter)
            {
                if (txtMainCategory.Text.Length > 0)
                {
                    //입력된 내용이 있을 경우
                    if (lblMainCategory.Text.Length > 0)
                    {
                    }
                    else
                    {
                        //신규 카테고리인 경우
                        MainForm.C
                    }
                    this.Name = txtMainCategory.Text;
                    lblMainCategory.Text = txtMainCategory.Text;
                    txtMainCategory.Visible = false;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                //카테고리 이름 변경 사항 취소
                if (lblMainCategory.Text.Length > 0)
                {
                    //기존 카테고리인 경우 원복
                    lblMainCategory.Visible = true;
                    txtMainCategory.Visible = false;
                    txtMainCategory.Clear();
                }
                else
                {
                    //새로 만드는 카테고리인 경우 추가된 카테고리 삭제
                    ((FlowLayoutPanel)this.Parent).Controls.Remove(this);
                }
            }
        }
    }
}
