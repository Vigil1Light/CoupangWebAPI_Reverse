using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoupangWeb.Reviews
{
    public partial class ReviewCommentForm : Form
    {
        public string Comment { get; set; }
        public bool ModifyFlags { get; set; }


        public ReviewCommentForm()
        {
            InitializeComponent();
            ModifyFlags = false;
        }

        private void ReviewCommentForm_Load(object sender, EventArgs e)
        {
            txtComment.Text = Comment;
            btnConfirm.Text = ModifyFlags ? "수정 (&U)" : "등록 (&R)";
            Text = ModifyFlags ? "코멘트 수정" : "코멘트 추가";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Comment = txtComment.Text;
            int len = Comment.Length;
            if (len < 1)
            {
                MessageBox.Show((ModifyFlags ? "수정" : "등록") + "하실 댓글을 입력하세요.");
            }
            /*else if (len > 1000)
            {
                MessageBox.Show("댓글은 1000글자미만이어야 합니다.");
            }*/
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
