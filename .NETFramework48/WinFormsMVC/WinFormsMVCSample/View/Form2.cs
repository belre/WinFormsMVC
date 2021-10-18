using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMVC.View;
using WinFormsMVC.Request;
using WinFormsMVC.Request.Item;
using WinFormsMVCSample.Controller;

namespace WinFormsMVCSample
{
    namespace View
    {
        public partial class Form2 : BaseForm
        {
            private Image _before_edit_image = null;
            private bool _is_now_drawing = false;

            public string MessageFromClone
            {
                get
                {
                    return label2.Text;
                }
                set
                {
                    label2.Text = value;
                }
            }

            public Form2()
            {
                InitializeComponent();

                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            }

            private void button1_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.LaunchForm3(this);
            }

            private void button2_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);

                controller.SendMessageWithRecord( new Command[] {
                    new GenericCommand<Form3, TextItem> {
                        Invoker=this,
                        Validation = (command, item) =>
                        {
                            item.Next = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form3) =>
                        {
                            form3.Message = item[form3];
                        },
                        NextOperation = (command, item, form3) =>
                        {
                            item[form3] = form3.Message;
                            form3.Message = item.Next;
                        }
                    },
                    new GenericCommand<Form4, TextItem>() {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.Next = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form4) =>
                        {
                            form4.Message = item[form4];
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            item[form4] = form4.Message;
                            form4.Message = item.Next;
                        }
                    }
                }, IsUndoEnable);
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.Undo(IsUndoEnable);
            }

            private void button4_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.LaunchForm4(this);
            }


            private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
            {
                if (!_is_now_drawing)
                {
                    _is_now_drawing = true;
                    _before_edit_image = (Image)pictureBox1.Image.Clone();
                }
            }

            private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
            {
                if (_is_now_drawing)
                {
                    _is_now_drawing = false;

                    var controller = Facade.GetController<Form2Controller>(this);
                    controller.SendMessageWithRecord( new Command[]
                    {
                        new GenericCommand<Form2, ImageItem>()
                        {
                            Invoker = this,
                            IsForSelf = true,
                            Validation = (command, item) =>
                            {
                                item.Next = (Image)pictureBox1.Image.Clone();
                                return true;
                            },
                            PrevOperation = (command, item, form2) =>
                            {
                                form2.pictureBox1.Image = item[form2];
                            },
                            NextOperation = (command, item, form2) =>
                            {
                                item[form2] = (Image)_before_edit_image.Clone();
                                form2.pictureBox1.Image = item.Next;
                            }
                        }
                    }, IsUndoEnable);
                }
            }

            private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
            {
                if (_is_now_drawing)
                {
                    var image = pictureBox1.Image;
                    var g = Graphics.FromImage(image);
                    g.DrawEllipse(new Pen(Brushes.Red), e.X, e.Y, 5F, 5F);

                    pictureBox1.Invalidate();
                }
            }

            private void button5_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);

                controller.SendMessageWithRecord(new Command[]
                {
                    new GenericCommand<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.Next = (Image)pictureBox1.Image.Clone();
                            return true;
                        },
                        PrevOperation = (command, item, form4) =>
                        {
                            form4.DisplayedImage = item[form4];
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            item[form4] = (Image)form4.DisplayedImage.Clone();
                            form4.DisplayedImage = item.Next;
                        }
                    }
                }, IsUndoEnable);
            }

            private void button6_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2TimestampController>(this);
                controller.TriggerAsyncTimeStamp(NotifyTimeStamp);
            }

            private void NotifyTimeStamp(string timestamp)
            {
                label1.Text = string.Format("{0}", timestamp);
            }


            private void Form2_Load(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                button3.Enabled = controller.IsAvailableUndo;
            }

            private void IsUndoEnable(bool is_available_undo)
            {
                button3.Enabled = is_available_undo;
            }

            private void button7_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);

                controller.SendSimpleMessage(new Command[]
                {
                    new GenericCommand<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.Next = (Image)pictureBox1.Image.Clone();
                            return true;
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            item[form4] = (Image)form4.DisplayedImage.Clone();
                            form4.DisplayedImage = item.Next;
                        }
                    }
                }, IsUndoEnable);
            }

            private void button8_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);

                controller.SendAsyncMessage(new Command[]
                {
                    new GenericCommand<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.Next = (Image)pictureBox1.Image.Clone();
                            System.Threading.Thread.Sleep(5000);
                            return true;
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            item[form4] = (Image)form4.DisplayedImage.Clone();
                            form4.DisplayedImage = item.Next;
                        }
                    }
                }, IsUndoEnable);
            }

            private void button9_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);

                controller.SendMessageWithRecord(new Command[] {
                    new GenericCommand<Form2, TextItem> {
                        Invoker=this,
                        IsRetrieved=true,
                        Validation = (command, item) =>
                        {
                            item.Next = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form2) =>
                        {
                            form2.MessageFromClone = item[form2];
                        },
                        NextOperation = (command, item, form2) =>
                        {
                            item[form2] = form2.MessageFromClone;
                            form2.MessageFromClone = item.Next;
                        }
                    }
                }, IsUndoEnable);
            }
        }
    }
}

