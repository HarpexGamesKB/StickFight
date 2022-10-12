using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EMX.HierarchyPlugin.Editor.Settings
{
    class MainRoot : UnityEditor.Editor, IRepaint
    {

        internal const string USE_STR = ""; //"Use 

        //internal PluginInstance p { get { return Root.p[ 0 ]; } }

        public int ID()
        {
            return GetInstanceID();
        }

        public int? currentWidth()
        {
            return null;
        }

        protected static PluginInstance p { get { return Root.p[ 0 ]; } }
        static Dictionary<int, CLASS_GROUP>  __GROUP = new Dictionary<int, CLASS_GROUP>();
        internal static CLASS_GROUP GRO( IRepaint w )
        {
            if ( !__GROUP.ContainsKey( w.ID() ) ) __GROUP.Add( w.ID(), new CLASS_GROUP() { A = p, ir = w } );
            return __GROUP[ w.ID() ];
        }
        static Dictionary<int, CLASS_ENALBE>  __ENABLE = new Dictionary<int, CLASS_ENALBE>();
        internal static CLASS_ENALBE ENABLE( IRepaint w )
        {
            if ( !__ENABLE.ContainsKey( w.ID() ) ) __ENABLE.Add( w.ID(), new CLASS_ENALBE() { A = p, ir = w } );
            return __ENABLE[ w.ID() ];
            //get { return __ENABLE ?? (__ENABLE = new CLASS_ENALBE() { A = p }); }
        }

        //CLASS_GROUP __GROUP;
        //
        //internal CLASS_GROUP GRO
        //{
        //    get { return __GROUP ?? (__GROUP = new CLASS_GROUP() { A = p }); }
        //}
        //
        //CLASS_ENALBE __ENABLE;
        //
        //internal CLASS_ENALBE ENABLE
        //{
        //    get { return __ENABLE ?? (__ENABLE = new CLASS_ENALBE() { A = p }); }
        //}

    }
}
