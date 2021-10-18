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

                controller.SendMessageWithRecord( new AbstractCommand[] {
                    new Command<Form3, TextItem> {
                        Invoker=this,
                        Validation = (command, item) =>
                        {
                            item.NextText = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form3) =>
                        {
                            if (item.PrevText != null)
                            {
                                form3.Message = item.PrevText;
                            }
                        },
                        NextOperation = (command, item, form3) =>
                        {
                            if (item.NextText != null)
                            {
                                item.PrevText = form3.Message;
                                form3.Message = item.NextText;
                            }
                        }
                    },
                    new Command<Form4, TextItem>() {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.NextText = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form4) =>
                        {
                            if (item.PrevText != null)
                            {
                                form4.Message = item.PrevText;
                            }
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            if (item.NextText != null)
                            {
                                item.PrevText = form4.Message;
                                form4.Message = item.NextText;
                            }
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
                    controller.SendMessageWithRecord( new AbstractCommand[]
                    {
                        new Command<Form2, ImageItem>()
                        {
                            Invoker = this,
                            IsForSelf = true,
                            Validation = (command, item) =>
                            {
                                item.NextImage = (Image)pictureBox1.Image.Clone();
                                return true;
                            },
                            PrevOperation = (command, item, form2) =>
                            {
                                if (item.PrevImage != null)
                                {
                                    form2.pictureBox1.Image = item.PrevImage;
                                }
                            },
                            NextOperation = (command, item, form2) =>
                            {
                                if (item.NextImage != null)
                                {
                                    item.PrevImage = (Image)_before_edit_image.Clone();
                                    form2.pictureBox1.Image = item.NextImage;
                                }
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

                controller.SendMessageWithRecord(new AbstractCommand[]
                {
                    new Command<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.NextImage = (Image)pictureBox1.Image.Clone();
                            return true;
                        },
                        PrevOperation = (command, item, form4) =>
                        {
                            if (item.PrevImage != null)
                            {
                                form4.DisplayedImage = item.PrevImage;
                            }
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            if (item.NextImage != null)
                            {
                                item.PrevImage = (Image)form4.DisplayedImage.Clone();
                                form4.DisplayedImage = item.NextImage;
                            }
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

                controller.SendSimpleMessage(new AbstractCommand[]
                {
                    new Command<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.NextImage = (Image)pictureBox1.Image.Clone();
                            return true;
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            if (item.NextImage != null)
                            {
                                item.PrevImage = (Image)form4.DisplayedImage.Clone();
                                form4.DisplayedImage = item.NextImage;
                            }
                        }
                    }
                }, IsUndoEnable);
            }

            private void button8_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);

                controller.SendAsyncMessage(new AbstractCommand[]
                {
                    new Command<Form4, ImageItem>()
                    {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.NextImage = (Image)pictureBox1.Image.Clone();
                            System.Threading.Thread.Sleep(5000);
                            return true;
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            if (item.NextImage != null)
                            {
                                item.PrevImage = (Image)form4.DisplayedImage.Clone();
                                form4.DisplayedImage = item.NextImage;
                            }
                        }
                    }
                }, IsUndoEnable);
            }
        }
    }
}

