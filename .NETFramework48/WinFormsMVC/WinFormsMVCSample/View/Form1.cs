using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMVC.View;
using WinFormsMVC.Facade;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVCSample.Controller;

namespace WinFormsMVCSample
{
    namespace View
    {
        public partial class Form1 : BaseForm
        {
            public Form1()
            {
                InitializeComponent();
            }


            private void button1_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form1Controller>(this);
                controller.Launch<Form2>(this);
                //controller.LaunchForm2(this);

            }

            private void button2_Click(object sender, EventArgs e)
            {

            }

            private void button2_Click_1(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form1Controller>(this);
                controller.SendStoredMessage( new Command[]
                {
                    new GenericCommand<Form3, TextItem>()
                    {
                        Invoker = this,
                        IsRecursive = true,
                        Validation = ( item, status) =>
                        {
                            item.Next = "Hello World";
                            return true;
                        },
                        PrevOperation = ( item, form3) =>
                        {
                            form3.RootMessage = item[form3];
                        },
                        NextOperation = ( item, form3) =>
                        {
                            item[form3] = form3.RootMessage;
                            form3.RootMessage = item.Next;
                        }
                    }
                }, null);
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form1Controller>(this);
                controller.LaunchWithLock<SPWFolderForm>(this, OnClosedFolderBrowser<SPWFolderForm>, (spw_form) =>
                {
                    spw_form.RootDrive = @"F:\usrdata";
                });
            }
            
            private void OnClosedFolderBrowser<T>(object sender, FormClosedEventArgs e) where T : SPWFolderForm
            {
                var form = (T) sender;

                if (form.DialogResult == DialogResult.OK)
                {
                    var controller = FacadeCore.GetController<Form1Controller>(this);
                    controller.SendStoredMessage(new Command[]
                    {
                        new GenericCommand<Form3, TextItem>()
                        {
                            Invoker = this,
                            IsRecursive = true,
                            Validation = ( item, status) =>
                            {
                                item.Next = form.FilePath;
                                return true;
                            },
                            PrevOperation = ( item, form3) =>
                            {
                                form3.FolderPath = item[form3];
                            },
                            NextOperation = ( item, form3) =>
                            {
                                item[form3] = form3.FolderPath;
                                form3.FolderPath = item.Next;
                            }
                        }
                    }, null);
                }
            }

            private void Form1_Load(object sender, EventArgs e)
            {
                int init_value = 5;
                textBox1.Text = init_value.ToString();
                trackBar1.Value = init_value;
            }


            private void trackBar1_ValueChanged(object sender, EventArgs e)
            {
                textBox1.Text = trackBar1.Value.ToString();
            }

            private bool textbox_and_trackbar_HasError(TextBox textbox, TrackBar trackbar)
            {
                int value;          // 整数変換の結果

                // もしもテキストボックスの値が、
                // 整数に変換できない場合はエラーとする。
                if (!int.TryParse(textbox.Text, out value))
                {
                    return true;
                }
                
                // もしもトラックバーの値の最小値、最大値の範囲に、
                // 整数に変換された値valueが入っていない場合はエラーとする。
                if (value < trackbar.Minimum || value > trackbar.Maximum)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            private void textBox1_TextChanged(object sender, EventArgs e)
            {
                if (textbox_and_trackbar_HasError(textBox1, trackBar1))
                {
                    textBox1.BackColor = Color.Yellow;
                }
                else
                {
                    textBox1.BackColor = SystemColors.Window;
                }
            }

            private void textBox1_Validating(object sender, CancelEventArgs e)
            {
                // 閉じるボタンを押すときにも、textBox1_Validatingが実行される。
                // 従って、if文がないと、閉じるボタンを押したときに閉じれなくなる。
                // → いまフォーカスが当たっているコントロールと、イベントを起こしたコントロールが
                // 同じであれば、現在入力待ちの状態なので、e.Cancel = true;としないようにする。
                if (ActiveControl.Equals(sender))
                {
                    return;
                }

                if (textbox_and_trackbar_HasError(textBox1, trackBar1))
                {
                    // e.Cancelと指定することで、
                    // 以降の処理をすべて中止する。
                    e.Cancel = true;
                }
            }

            private void textBox1_Validated(object sender, EventArgs e)
            {
                if (!textbox_and_trackbar_HasError(textBox1, trackBar1))
                {
                    trackBar1.Value = int.Parse(textBox1.Text);
                }
            }
        }
    }
}
