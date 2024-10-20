﻿using System;
using System.Collections.Generic;

namespace ArsonistMod.Modules
{
    internal static class Helpers
    {
        internal const string startPrefix = "<style=cIsUtility>Agile. Heat. </style>";
        internal const string heatPrefix = "<style=cIsUtility>Heat. </style>";
        internal const string criticalPrefix = "<style=cIsUtility>Supercritical. </style>";

        internal static string ScepterDescription(string desc)
        {
            return "\n<color=#d299ff>SCEPTER: " + desc + "</color>";
        }

        internal static string ImportantDesc(string desc) 
        {
            return $"<color=#FF9402>{desc}</color>";
        }

        internal static string Healing(string desc)
        {
            return $"<color=#9FFF02>{desc}</color>";
        }

        internal static string Downside(string desc)
        {
            return $"<color=#E40000>{desc}</color>";
        }

        public static T[] Append<T>(ref T[] array, List<T> list)
        {
            var orig = array.Length;
            var added = list.Count;
            Array.Resize<T>(ref array, orig + added);
            list.CopyTo(array, orig);
            return array;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);

        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementNameToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementDescriptionToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}