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

                controller.SendMessage( new AbstractCommand[] {
                    new Command<Form3, TextItem> {
                        Invoker=this,
                        Validation = (command, item) =>
                        {
                            item.NextTemporary = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form3) =>
                        {
                            if (item.PrevTemporary != null)
                            {
                                form3.Message = item.PrevTemporary;
                            }
                        },
                        NextOperation = (command, item, form3) =>
                        {
                            if (item.NextTemporary != null)
                            {
                                item.PrevTemporary = form3.Message;
                                form3.Message = item.NextTemporary;
                            }
                        }
                    },
                    new Command<Form4, TextItem>() {
                        Invoker = this,
                        Validation = (command, item) =>
                        {
                            item.NextTemporary = textBox1.Text;
                            return true;
                        },
                        PrevOperation = (command, item, form4) =>
                        {
                            if (item.PrevTemporary != null)
                            {
                                form4.Message = item.PrevTemporary;
                            }
                        },
                        NextOperation = (command, item, form4) =>
                        {
                            if (item.NextTemporary != null)
                            {
                                item.PrevTemporary = form4.Message;
                                form4.Message = item.NextTemporary;
                            }
                        }
                    }
                });
            }

            private void button3_Click(object sender, EventArgs e)
            {
                var controller = Facade.GetController<Form2Controller>(this);
                controller.Redo();
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
                    controller.SendMessage( new AbstractCommand[]
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
                    });
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

                controller.SendMessage(new AbstractCommand[]
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
                });
            }
        }
    }
}

