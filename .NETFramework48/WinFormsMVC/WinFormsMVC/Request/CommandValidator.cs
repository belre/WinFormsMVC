﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVC.Request
{
    public abstract class CommandValidator<Item> : Command where Item : CommandItem
    {
        /// <summary>
        /// データ検証を行うときに実行される処理です。
        /// </summary>
        public Func<Item, bool> Validation { get; set; }

        /// <summary>
        /// 「元に戻す」の後に行なわれる処理です。
        /// </summary>
        public Action<Item> FinalOperation { get; set; }

        /// <summary>
        /// データ検証に失敗したときに実行される処理です。
        /// </summary>
        public Action<Item> ErrorOperation { get; set; }


    }
}