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
    class SA_Window : ScriptableObject
    {
    }

    [CustomEditor( typeof( SA_Window ) )]
    class GameObjectSetActiveModSettingsEditor : MainRoot
    {

        internal static string set_text =  USE_STR + "GameObject SetActive Mod (Hierarchy Window)";
        internal static string set_key = "USE_SETACTIVE_MOD";

        public override VisualElement CreateInspectorGUI()
        {
            return base.CreateInspectorGUI();
        }
        public override void OnInspectorGUI()
        {
            _GUI( (IRepaint)this );
        }
        public static void _GUI( IRepaint w )
        {
            Draw.RESET(w);

            //   GUI.Button( Draw.R2, "xxx", Draw.s( "preToolbar" ) );
            // GUI.Button( Draw.R, "Common Settings", Draw.s( "insertionMarker" ) );
            Draw.BACK_BUTTON( w );
            Draw.TOG_TIT( set_text, set_key );
            Draw.Sp( 10 );

            using ( ENABLE(w).USE( set_key ) )
            {
                using ( GRO(w).UP(0) )
                {
            Draw.Sp( 4 );
                    Draw.TOG( "Change button cursor for Hierarchy window", "SET_ACTIVE_CHANGE_BUTTON_CURSOR" );
                Draw.Sp( 10 );

                Draw.TOG( "Replace prefab button", "SET_ACTIVE_PREFAB_BUTTON_OFFSET" );
                Draw.TOG( "Small style", "SET_ACTIVE_SMALL_BOOL" );
                Draw.TOG( "Smoothed camera movement to objects on RBM", "SET_ACTIVE_SMOOTH_FRAME" );
                Draw.HELP(w, "Use the Right-Click to move scene camera to GameObject ('F' or double click like)" );
            Draw.Sp( 10 );
                }

            }



            Draw.Sp( 10 );
            //Draw.HRx2();
            //GUI.Label( Draw.R, "" + LEFT + " Area:" );
            using ( GRO(w).UP(0) )
            {
                // Draw.TOG_TIT( "" + LEFT + " Area:" );
                Draw.TOG_TIT( "Quick tips:" );
                Draw.HELP(w, "Drag mouse to Enable/Disable several GameObjects.", drawTog: true );
                Draw.HELP_TEXTURE(w, "HELP_SETACTIVE", 0 );
            }
        }
    }
}
