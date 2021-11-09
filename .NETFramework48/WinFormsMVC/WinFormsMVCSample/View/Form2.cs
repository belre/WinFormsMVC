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
                IsUndoAndRedoEnable(false, false);
            }

            private void button1_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                controller.Launch<Form3>(this);
            }

            private void button2_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);

                controller.SendStoredMessage( new Command[] {
                    new GenericCommand<Form3, TextItem> {
                        Invoker=this,
                        Validation = ( item) =>
                        {
                            item.Next = textBox1.Text;
                            return true;
                        },
                        Preservation = (item, status, form3) =>
                        {
                            item[form3] = form3.Message;
                        },
                        NextOperation = ( item, status, form3) =>
                        {
                            form3.Message = item.Next;
                        },
                        PrevOperation = ( item, status, form3) =>
                        {
                            form3.Message = item[form3];
                        }
                    },
                    new GenericCommand<Form4, TextItem>() {
                        Invoker = this,
                        Validation = ( item) =>
                        {
                            item.Next = textBox1.Text;
                            return true;
                        },
                        Preservation = (item, status, form4) =>
                        {
                            item[form4] = form4.Message;
                        },
                        NextOperation = ( item, status, form4) =>
                        {
                            if (status.ExecutedCount == 0)
                            {
                                item[form4] = form4.Message;
                            }
                            form4.Message = item.Next;
                        },
                        PrevOperation = ( item, status, form4) =>
                        {
                            form4.Message = item[form4];
                        }
                    }
                }, IsUndoAndRedoEnable);
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                controller.Undo(IsUndoAndRedoEnable);
            }

            private void button4_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                controller.Launch<Form4>(this);
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

                    var controller = FacadeCore.GetController<Form2Controller>(this);
                    controller.SendStoredMessage( new Command[]
                    {
                        new GenericCommand<Form2, ImageItem>()
                        {
                            Invoker = this,
                            IsForSelf = true,
                            Validation = ( item) =>
                            {
                                item.Next = pictureBox1.Image;
                                return true;
                            },
                            Preservation = (item, status, form2) =>
                            {
                                item[form2] = _before_edit_image;
                            },
                            NextOperation = ( item, status, form2) =>
                            {
                                form2.pictureBox1.Image = item.Next;
                            },
                            PrevOperation = ( item, status, form2) =>
                            {
                                form2.pictureBox1.Image = item[form2];
                            }
                        }
                    }, IsUndoAndRedoEnable);
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

            class Test
            {
                public Func<ImageItem, bool> Func
                {
                    get;
                    set;
                }
            }

            private void button5_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);

                controller.SendStoredMessage(new Command[]
                {
                    new GenericCommand<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (item) =>
                        {
                            item.Next = (Image)pictureBox1.Image.Clone();
                            return true;
                        },
                        Preservation = (item, status, form4) =>
                        {
                            item[form4] = (Image)form4.DisplayedImage.Clone();
                        },
                        NextOperation = ( item, status, form4) =>
                        {
                            form4.DisplayedImage = item.Next;
                        },
                        PrevOperation = ( item, status, form4) =>
                        {
                            form4.DisplayedImage = item[form4];
                        }
                    }
                }, IsUndoAndRedoEnable);
            }

            private void button6_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                controller.GetTimeStamp(NotifyTimeStamp);
            }

            private void NotifyTimeStamp(string timestamp)
            {
                label1.Text = string.Format("{0}", timestamp);
            }


            private void Form2_Load(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                button3.Enabled = controller.IsAvailableUndo;
            }

            private void IsUndoAndRedoEnable(bool is_available_undo, bool is_available_redo)
            {
                button3.Enabled = is_available_undo;
                button10.Enabled = is_available_redo;
            }

            private void button7_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);

                controller.SendSimpleMessage(new Command[]
                {
                    new GenericCommand<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = ( item) =>
                        {
                            item.Next = (Image)pictureBox1.Image.Clone();
                            return true;
                        },
                        NextOperation = ( item, status, form4) =>
                        {
                            item[form4] = (Image)form4.DisplayedImage.Clone();
                            form4.DisplayedImage = item.Next;
                        }
                    }
                });
            }

            private void button8_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);

                controller.StampTextIntoImage(BeNotifiedImage, pictureBox1.Image);
            }

            private void BeNotifiedImage(Image image)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                controller.SendSimpleMessage(new Command[]
                {
                    new GenericCommand<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = ( item) =>
                        {
                            item.Next = image;
                            return true;
                        },
                        NextOperation = ( item, status, form4) =>
                        {
                            form4.DisplayedImage = item.Next;
                        }
                    }
                });
            }

            private void button9_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);

                controller.SendStoredMessage(new Command[] {
                    new GenericCommand<Form2, TextItem> {
                        Invoker=this,
                        IsRecursive=true,
                        Validation = ( item) =>
                        {
                            item.Next = textBox1.Text;
                            return true;
                        },
                        Preservation = (item, status, form2) =>
                        {
                            item[form2] = form2.MessageFromClone;
                        },
                        NextOperation = ( item, status, form2) =>
                        {
                            form2.MessageFromClone = item.Next;
                        },
                        PrevOperation = ( item, status, form2) =>
                        {
                            form2.MessageFromClone = item[form2];
                        }
                    }
                }, IsUndoAndRedoEnable);
            }

            private void button10_Click(object sender, EventArgs e)
            {
                var controller = FacadeCore.GetController<Form2Controller>(this);
                controller.Redo(IsUndoAndRedoEnable);
            }

            private void button11_Click(object sender, EventArgs e)
            {
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.DrawString("Dummy Text", new Font("Arial", 16.0F), Brushes.Blue, 20, 100);

                pictureBox1.Invalidate();
            }
        }
    }
}

