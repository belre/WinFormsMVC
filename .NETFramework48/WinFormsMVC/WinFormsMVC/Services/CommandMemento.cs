using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.View;

namespace WinFormsMVC.Services
{
    /// <summary>
    /// Mementoパターンを使用して、コマンドの履歴を参照します。
    /// </summary>
    public class CommandMemento
    {
        /// <summary>
        /// 最大で記録するコマンド数です。
        /// </summary>
        public readonly int MAX_MEMEMTO_NUMBER = 10;

        /// <summary>
        /// Mementoのコマンドの一覧を表します。
        /// </summary>
        public List<IEnumerable<Request.Command>> Mememtoes { get; }

        public CommandMemento()
        {
            Mememtoes = new List<IEnumerable<Request.Command>>();
        }

        /// <summary>
        /// コマンドを追加します。
        /// </summary>
        /// <param name="abstractCommand"></param>
        public void PushCommand(IEnumerable<Request.Command> abstractCommand)
        {
            Mememtoes.Add(abstractCommand);

            if (Mememtoes.Count > MAX_MEMEMTO_NUMBER)
            {
                Mememtoes.RemoveRange(0, Mememtoes.Count - MAX_MEMEMTO_NUMBER);
            }
        }

        /// <summary>
        /// コマンドを削除します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Request.Command> PopCommand()
        {
            if (Mememtoes.Count != 0)
            {
                var target = Mememtoes.Last();
                Mememtoes.Remove(target);
                return target;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 「元に戻す」が可能かどうかを表します。
        /// </summary>
        /// <returns></returns>
        public bool IsAvalableUndo()
        {
            return Mememtoes.Count != 0;
        }
    }
}