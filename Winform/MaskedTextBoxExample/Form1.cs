using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaskedTextBoxExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //maskedTextBox.CutCopyMaskFormat = MaskFormat.IncludeLiterals;
            //maskedTextBox.TextMaskFormat = MaskFormat.IncludeLiterals;
            ////maskedTextBox.Mask = "";
            ////maskedTextBox.PasswordChar = '#';
            ////maskedTextBox.RejectInputOnFirstFailure = true;


            // copy的格式
            copyMaskedFmtCbx.Items.Add(MaskFormat.IncludeLiterals);
            copyMaskedFmtCbx.Items.Add(MaskFormat.ExcludePromptAndLiterals);
            copyMaskedFmtCbx.Items.Add(MaskFormat.IncludePrompt);
            copyMaskedFmtCbx.Items.Add(MaskFormat.IncludePromptAndLiterals);
            copyMaskedFmtCbx.SelectedIndexChanged += CopyMaskedFmtCbx_SelectedIndexChanged;

            // 输入错误提示音
            beepOnErrorCbx.CheckedChanged += BeepOnErrorCbx_CheckedChanged;

            // 密码字符
            pwdCharTxt.TextChanged += PwdCharTxt_TextChanged;

            // 提示字符
            promptCharTxt.Text = "_";
            promptCharTxt.TextChanged += PromptCharTxt_TextChanged;

            // 一输入失败就禁止继续输入
            rejectOnFailureCbx.CheckedChanged += RejectOnFailureCbx_CheckedChanged;

            // Text属性值
            textMaskFmtCbx.Items.Add(MaskFormat.IncludeLiterals);
            textMaskFmtCbx.Items.Add(MaskFormat.ExcludePromptAndLiterals);
            textMaskFmtCbx.Items.Add(MaskFormat.IncludePrompt);
            textMaskFmtCbx.Items.Add(MaskFormat.IncludePromptAndLiterals);
            textMaskFmtCbx.SelectedIndexChanged += TextMaskFmtCbx_SelectedIndexChanged;

            // masked的类型
            maskTypeCbx.Items.AddRange(new string[]
            {
                "IP地址",
                "身份证",
                "手机号",
                "日期时间",
                "身高",
                
            });
            maskTypeCbx.SelectedIndexChanged += MaskTypeCbx_SelectedIndexChanged;
        }

        private void TextMaskFmtCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox.TextMaskFormat = (MaskFormat)textMaskFmtCbx.SelectedItem;
            MessageBox.Show(maskedTextBox.Text);
        }

        private void RejectOnFailureCbx_CheckedChanged(object sender, EventArgs e)
        {
            maskedTextBox.RejectInputOnFirstFailure = rejectOnFailureCbx.Checked;
        }

        private void PromptCharTxt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(promptCharTxt.Text))
            {
                maskedTextBox.PromptChar = '\0';
            }
            else
            {
                var c = promptCharTxt.Text.Trim().First();
                promptCharTxt.Text = c + "";
                maskedTextBox.PromptChar = c;
            }
        }

        private void PwdCharTxt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pwdCharTxt.Text))
            {
                maskedTextBox.PasswordChar = '\0';
            }
            else
            {
                var c = pwdCharTxt.Text.Trim().First();
                pwdCharTxt.Text = c+ "";
                maskedTextBox.PasswordChar = c;
            }
        }

        private void BeepOnErrorCbx_CheckedChanged(object sender, EventArgs e)
        {
            maskedTextBox.BeepOnError = beepOnErrorCbx.Checked;
        }

        private void CopyMaskedFmtCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox.CutCopyMaskFormat = (MaskFormat)copyMaskedFmtCbx.SelectedItem;
        }

        private void MaskTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strMask = string.Empty;
            switch (maskTypeCbx.Text)
            {
                case "IP地址":
                    strMask = "100-0000-0000";
                    break;
                case "身份证":
                    // 身份证最后一位有时为X
                    // 可以看到单纯使用Mask 不如 正则灵活。可以再结合 TextChanged 事件，判断限制最后一位为X或数字
                    strMask = "000000-00000000-000A";
                    break;
                case "手机号":
                    strMask = "100-0000-0000";
                    break;
                case "日期时间":
                    strMask = "0000-00-00 00:00:00";
                    break;
                case "身高":
                    strMask = "000厘米";
                    break;
                case "人民币":
                    strMask = "￥0000.00";
                    break;
                default:
                    break;
            }

            maskedTextBox.Mask = strMask;
        }
    }
}
