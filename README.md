# WinFormsMVC

WinForms専用でMVCパターンを実装するときに使用するテンプレートプロジェクトです。
なお、まだまだ改良を重ねたいと考えていますので、ご意見をください。

## WinFormsMVC

- フレームワーク : .NET Framework 4.8
- 開発環境 : Visual Studio 2019 Professional

## WinFormsMVCDotnet6

- フレームワーク ： .NET 6
- 開発環境： Visual Studio 2022 Preview (17.0.0)
- 現在未テスト

## 導入手順

1. WinFormsMVCのプロジェクトをインポート
2. Formsは常にWinFormsMVC.BaseFormを継承する。

例：

```C#:Form1.cs
namespace WinFormsMVC
{
    namespace View
    {
        public partial class Form1 : BaseForm
        {

        }
    }
}
```

3. 画面を操作する場合、Controllerを使用する。

**例１．コントローラを取得し、新たなフォーム(Form2クラス)を作成する場合**

コントローラ側
```C#
using WinFormsMVC.Services;
using WinFormsMVC.Controller;

namespace WinFormsMVCSample.Controller
{
    public class Form1Controller : CommandController
    {

        public Form1Controller(FormsManagement manager)
            : base(manager)
        {
        }

    }
}

```

ビュー側

```C#
// Form1の定義
private void button1_Click(object sender, EventArgs e)
{
    var controller = FacadeCore.GetController<Form1Controller>(this);
    controller.Launch<Form2>(this);
}
```

**例２．コントローラを取得し、Form2クラスに指定されたラベルの文字列を変更する場合**

コントローラ側は例１と同じ

ビュー側(Form1)
```C#
// Form1の定義の途中
private void button2_Click_1(object sender, EventArgs e)
{
    var controller = FacadeCore.GetController<Form1Controller>(this);
    controller.SendStoredMessage( new Command[]
    {
        new GenericCommand<Form2, TextItem>()
        {
          Invoker = this,
          Preservation = (item, status, form2) =>
          {
              item.Next = "Hello World";
              return true;
          },
          PrevOperation = ( item, status, form2) =>
          {
              form3.RootMessage = item[form2];
          },
          NextOperation = ( item, status, form2) =>
          {
              item[form2] = form2.Message;
              form2.Message = item.Next;
          }
        }
    }, null);
}
```

ビュー側(Form2)

```C#
namespace WinFormsMVCSample
{
    namespace View
    {
        public partial class Form2 : BaseForm
        {
            public string Message 
            {
                get
                {
                    return _label1.Text;
                }
                set
                {
                    _label1.Text = value;                
                }
            }
        }
    }
}
```

**例３．「元に戻す」を実行する**

SendStoredMessageメソッドが実行された場合、**元に戻す**(Undo), **やり直し(Redo)** の操作を実行することが出来る。

ビュー側(Form2)

```C#
// Form2の定義の途中
private void button3_Click(object sender, EventArgs e)
{
    var controller = FacadeCore.GetController<Form2Controller>(this);
    controller.Undo(IsUndoEnable);
}
```

