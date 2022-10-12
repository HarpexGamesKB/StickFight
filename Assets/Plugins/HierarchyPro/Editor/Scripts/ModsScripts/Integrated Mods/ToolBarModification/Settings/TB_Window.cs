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
	class TB_Window : ScriptableObject
	{


	}


	[CustomEditor( typeof( TB_Window ) )]
	class TopBarsModSettingsEditor : MainRoot
	{

		internal static string set_text =  USE_STR + "TopBar (ToolBar)";
		internal static string set_key = "USE_TOPBAR_MOD";
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
			Draw.RESET( w );

			Draw.BACK_BUTTON( w );
			Draw.TOG_TIT( set_text, set_key );
			Draw.Sp( 10 );
			using ( ENABLE( w ).USE( set_key ) )
			{

				string RIGHT = "Right";
				string LEFT=  "Left";
				if ( p.par_e.TOPBAR_SWAP_LEFT_RIGHT )
				{
					var t = RIGHT;
					RIGHT = LEFT;
					LEFT = t;
				}


				using ( GRO( w ).UP( 0 ) )
				{


					using ( GRO( w ).UP( 0 ) )
					{
						Draw.TOG( "Swap Left and Right areas", "TOPBAR_SWAP_LEFT_RIGHT" );
					}




				Draw.Sp( 10 );
				//  Draw.HRx2();
				using ( GRO( w ).UP( 0 ) )
				{
					Draw.TOG_TIT( "" + RIGHT + " Area:" );

					Draw.FIELD( "" + RIGHT + " area min border offset", "TOPBAR_LEFT_MIN_BORDER_OFFSET", -500, 500 );
					Draw.FIELD( "" + RIGHT + " area max border offset", "TOPBAR_LEFT_MAX_BORDER_OFFSET", -500, 500 );

					Draw.Sp( 5 );
					using ( GRO( w ).UP( 0 ) )
					{
						Draw.TOG( "Draw Layouts Tab", "DRAW_TOPBAR_LAYOUTS_BAR" );
						using ( ENABLE( w ).USE( "DRAW_TOPBAR_LAYOUTS_BAR" ) )
						{
							Draw.TOG( "Draw cross for selected layout", "TOPBAR_LAYOUTS_DRAWCROSS" );
							Draw.FIELD( "Addition y style min adjustment", "TOPBAR_LAYOUTS_MIN_Y_OFFSET", -500, 500 );
							Draw.FIELD( "Addition y style max adjustment", "TOPBAR_LAYOUTS_MAN_Y_OFFSET", -500, 500 );

							Draw.TOG( "AutoSave selected layout", "TOPBAR_LAYOUTS_AUTOSAVE" );
							using ( ENABLE( w ).USE( "TOPBAR_LAYOUTS_AUTOSAVE" ) )
							{
								Draw.TOG( "Disable autosave for internal layout", "TOPBAR_LAYOUTS_SAVE_ONLY_CUSTOM" );
								Draw.HELP( w, "Be careful, because you can rewrite even a default layout, however you can of course reset it" );
							}

							Draw.HELP( w, "You can share your layouts that are stored in the: " +
								Folders.DataGetterByType( Folders.CacheFolderType.SettingsData, Folders.DATA_SETTINGS_PATH_USE_DEFAULT ).GET_PATH_TOSTRING + "/.SavedLayouts/" );

						}
						Draw.Sp( 5 );
					}

					Draw.Sp( 4 );
					Draw.TOG( "Use Custom " + RIGHT + " Side Buttons", "DRAW_TOPBAR_CUSTOM_LEFT" );
					Draw.HELP( w, "You can add your buttons at the top bars, using EMX." + Root.CUST_NS + "" );
					Draw.Sp( 5 );

				}


				Draw.Sp( 10 );
				//Draw.HRx2();
				//GUI.Label( Draw.R, "" + LEFT + " Area:" );
				using ( GRO( w ).UP( 0 ) )
				{
					Draw.TOG_TIT( "" + LEFT + " Area:" );

					Draw.FIELD( "" + LEFT + " area min border offset", "TOPBAR_RIGHT_MIN_BORDER_OFFSET", -500, 500 );
					Draw.FIELD( "" + LEFT + " area max border offset", "TOPBAR_RIGHT_MAX_BORDER_OFFSET", -500, 500 );


					Draw.Sp( 5 );

#if !TRUE
					using ( GRO( w ).UP( 0 ) )
					{
						Draw.TOG( "Draw External Mods HotButtons on TopBar", "DRAW_TOPBAR_HOTBUTTONS" );
						using ( ENABLE( w ).USE( "DRAW_TOPBAR_HOTBUTTONS" ) )
						{
							Draw.FIELD( "TopBar Buttons Size", "TOPBAR_HOTBUTTON_SIZE", 3, 60, "px" );
							p.par_e.DrawHotButtonsArray();
						}
						Draw.Sp( 3 );
					}
					Draw.Sp( 3 );
#endif

					Draw.TOG( "Use Custom " + LEFT + " Side Buttons", "DRAW_TOPBAR_CUSTOM_RIGHT" );
					Draw.HELP( w, "You can add your buttons at the top bars, using EMX." + Root.CUST_NS + "" );
					Draw.Sp( 4 );
				}
					//	Draw.Sp(10);

				}


				Draw.Sp( 10 );
				//Draw.HRx2();
				//GUI.Label( Draw.R, "" + LEFT + " Area:" );
				using ( GRO( w ).UP( 0 ) )
				{

					// Draw.TOG_TIT( "" + LEFT + " Area:" );

					Draw.TOG_TIT( "Quick tips:" );
					Draw.HELP_TEXTURE( w, "HELP_LAYOUT", 0 );
					Draw.HELP( w, "Use the right mouse button to open a special menu for quick access to functions.", drawTog: true );
					Draw.HELP( w, "You can use left mouse button to drag button to change position, or use middle button to remove.", drawTog: true );
					//Draw.HRx2();
					Draw.Sp( 10 );

					Draw.TOG_TIT( "You can add your own buttons on topbar:", EnableRed: false );
					Draw.HELP( w, "Use ExtensionInterface_TopBarOnGUI class, and don't forget enable 'Use Custom Buttons' toggles in the current settings", drawTog: true );
					Draw.Sp( 3 );
					if ( Draw.BUT( "Select Script with Custom Examples" ) ) { Selection.objects = new[] { Root.icons.example_folders[ 3 ] }; }
					Draw.Sp( 20 );

					//HOT BUTTONS

				}


			}
		}
	}
}
