#if UNITY_EDITOR

using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace Examples.HierarchyPlugin
{

    //Don't forget enable 'Use Custom Buttons' toggles in the top bar settings
    public class CustomTopBarOnGUI
    {


        [InitializeOnLoadMethod]
		static void CreateEvents() //should be a static
		{
            //Don't forget enable 'Use Custom Buttons' toggles in the top bar settings
            EMX.CustomizationHierarchy.ExtensionInterface_TopBarOnGUI.onLeftLayoutGUI += OnLeftLayoutGUI;
			EMX.CustomizationHierarchy.ExtensionInterface_TopBarOnGUI.onRightLayoutGUI += OnRightLayoutGUI;
		}

        static void OnLeftLayoutGUI( Rect rect )
        {
            //Don't forget enable 'Use Custom Buttons' toggles in the top bar settings
            if ( GUI.Button( rect, "GO MY 1 LAYOUT" ) )
            {
                Debug.Log( "Hello Unity!" );
            }
        }

        static void OnRightLayoutGUI( Rect rect )
        {
            //Don't forget enable 'Use Custom Buttons' toggles in the top bar settings
            if ( GUI.Button( rect, "GO MY 2 LAYOUT" ) )
            {
                Debug.Log( "Hello Unity!" );
            }
        }
	}

}
#endif
