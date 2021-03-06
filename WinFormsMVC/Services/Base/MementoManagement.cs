using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVC.Request;
using WinFormsMVC.View;

namespace WinFormsMVC.Services.Base
{
    /// <summary>
    /// Mementoパターンを使用して、コマンドの履歴を参照します。
    /// </summary>
    public class MementoManagement
    {
        /// <summary>
        /// 最大で記録するコマンド数です。
        /// </summary>
        public readonly int MAX_MEMEMTO_NUMBER = 10;

        /// <summary>
        /// Mementoのコマンドの一覧を表します。
        /// </summary>
        public List<IEnumerable<Request.Command>> Mememtoes { get; }

        /// <summary>
        /// 削除しようとしているMementoのコマンド一覧を表します。
        /// </summary>
        public List<IEnumerable<Request.Command>> RemovingMememtoes { get; }

        public MementoManagement()
        {
            Mememtoes = new List<IEnumerable<Request.Command>>();
            RemovingMememtoes = new List<IEnumerable<Command>>();
        }

        protected void FreeEnumerable(IEnumerable<Request.Command> mememto)
        {
            foreach (var comm in mememto)
            {
                comm.Dispose();
            }
        }

        protected void ClearAllEnumerable(List<IEnumerable<Request.Command>> mememtoes)
        {
            foreach (var mememto in mememtoes)
            {
                foreach (var comm in mememto)
                {
                    comm.Dispose();
                }
            }
            mememtoes.Clear();
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
                FreeEnumerable(Mememtoes.First());
                Mememtoes.RemoveAt(0);
            }

            ClearAllEnumerable(RemovingMememtoes);
        }

        /// <summary>
        /// 削除候補に含まれているコマンドを
        /// 再度コマンドに入れなおします。
        /// </summary>
        public IEnumerable<Request.Command> RestoreCommand()
        {
            if (RemovingMememtoes.Count() != 0)
            {
                var adapt_command = RemovingMememtoes.First();
                RemovingMememtoes.RemoveAt(0);

                Mememtoes.Add(adapt_command);
                if (Mememtoes.Count > MAX_MEMEMTO_NUMBER)
                {
                    FreeEnumerable(RemovingMememtoes.First());
                    Mememtoes.RemoveAt(0);
                }

                return adapt_command;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// コマンドを削除します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Request.Command> PopLatestCommand()
        {
            if (Mememtoes.Count != 0)
            {
                var target = Mememtoes.Last();
                Mememtoes.Remove(target);

                RemovingMememtoes.Insert(0, target);
                if (RemovingMememtoes.Count > MAX_MEMEMTO_NUMBER)
                {
                    FreeEnumerable(RemovingMememtoes.Last());
                    RemovingMememtoes.Remove(RemovingMememtoes.Last());
                }

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

        public bool IsAvailableRedo()
        {
            return RemovingMememtoes.Count != 0;
        }
    }
}