using System.Collections.Generic;
using UnityEngine;
using System;
namespace BubbleZun.Utils
{
    public static class InputConstants
    {
        // 定义键盘布局
        private static readonly string[] KeyboardLayout = new string[]
        {
            "1234567890-=",
            "QWERTYUIOP[]",
            "ASDFGHJKL;'",
            "ZXCVBNM,./"
        };

        // 存储每个键的位置
        private static readonly Dictionary<KeyCode, (int row, int col)> KeyPositions = new Dictionary<KeyCode, (int row, int col)>();

        // 静态构造函数初始化位置字典
        static InputConstants()
        {
            // 初始化键位置字典
            for (int row = 0; row < KeyboardLayout.Length; row++)
            {
                for (int col = 0; col < KeyboardLayout[row].Length; col++)
                {
                    char c = KeyboardLayout[row][col];
                    KeyCode keyCode = GetKeyCodeFromChar(c);
                    if (keyCode != KeyCode.None)
                    {
                        KeyPositions[keyCode] = (row, col);
                    }
                }
            }
        }

        // 计算两个键之间的距离
        public static int KeyDistance(KeyCode key1, KeyCode key2)
        {
            if (!KeyPositions.ContainsKey(key1) || !KeyPositions.ContainsKey(key2))
            {
                return int.MaxValue;
            }

            var pos1 = KeyPositions[key1];
            var pos2 = KeyPositions[key2];

            // 计算曼哈顿距离
            return Math.Abs(pos1.row - pos2.row) + Math.Abs(pos1.col - pos2.col);
        }

        // 辅助函数：将字符转换为KeyCode
        private static KeyCode GetKeyCodeFromChar(char c)
        {
            if (char.IsDigit(c))
            {
                return (KeyCode)((int)KeyCode.Alpha0 + (c - '0'));
            }
            else if (char.IsLetter(c))
            {
                return (KeyCode)((int)KeyCode.A + (char.ToUpper(c) - 'A'));
            }
            else
            {
                switch (c)
                {
                    case '-': return KeyCode.Minus;
                    case '=': return KeyCode.Equals;
                    case '[': return KeyCode.LeftBracket;
                    case ']': return KeyCode.RightBracket;
                    case ';': return KeyCode.Semicolon;
                    case '\'': return KeyCode.Quote;
                    case ',': return KeyCode.Comma;
                    case '.': return KeyCode.Period;
                    case '/': return KeyCode.Slash;
                    default: return KeyCode.None;
                }
            }
        }

        public static readonly List<KeyCode> AllKeyCodes = new List<KeyCode>
        {
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.I,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.M,
            KeyCode.N,
            KeyCode.O,
            KeyCode.P,
            KeyCode.Q,
            KeyCode.R,
            KeyCode.S,
            KeyCode.T,
            KeyCode.U,
            KeyCode.V,
            KeyCode.W,
            KeyCode.X,
            KeyCode.Y,
            KeyCode.Z,
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Space,
            KeyCode.Return,
            KeyCode.Escape,
            KeyCode.LeftShift,
            KeyCode.RightShift,
            KeyCode.LeftControl,
            KeyCode.RightControl,
            KeyCode.LeftAlt,
            KeyCode.RightAlt,
            KeyCode.Tab,
            KeyCode.Backspace,
            KeyCode.Delete,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };
    }
}