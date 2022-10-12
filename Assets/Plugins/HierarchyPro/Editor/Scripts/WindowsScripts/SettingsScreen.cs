
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using EMX.HierarchyPlugin.Editor.Settings;

namespace EMX.HierarchyPlugin.Editor
{


	class SettingsScreen : EditorWindow, IRepaint
	{

		const int buttonW = 240;

		public static void Init( Rect? __source, Type t ) // var w = GetWindow<WelcomeScreen>( "Post Presets - Welcome Screen" , true, Params.WindowType,)
		{
			_source = __source;
			//EditorApplication.update += showw;
			showw( t );
		}

		int id;
		public int ID()
		{
			return GetInstanceID() + id;
		}
		const int SHR = 20;
		public int? currentWidth()
		{
			return (int)position.width / 2 - SHR * 2 - 16;
		}

		Type type;
		static Rect? _source;
		static void showw( Type t )
		{
			var pw = Resources.FindObjectsOfTypeAll<SettingsScreen>().FirstOrDefault();
			if ( pw )
			{
				// pw. winScrollPos.Clear();
				pw.type = t;
				Draw.RESET( pw );
				pw.Repaint();
				return;
			}
			//var w = GetWindow<SettingsScreen>(true, "" + Root.PN + " - Settings", true);
			var w = CreateWindow<SettingsScreen>( "" + Root.HierarchyPro + " - Settings");
			if ( !SessionState.GetBool( Folders.PREFS_PATH + "was settings", false ) )
			{
				var d  = Screen.currentResolution.height / 1080f;
				var source = _source ?? new Rect(0, d * 140, Screen.currentResolution.width, Screen.currentResolution.height - d * 280);
				var thisR = new Rect(0, source.y, buttonW + (Screen.currentResolution.width < 1500 ? 430 : Screen.currentResolution.width < 2100 ? 750 : 1000), Math.Max(source.height,
				Math.Min(Screen.currentResolution.height, 1080) - d *280));
				thisR.x = source.x + source.width / 2 - thisR.width / 2;
				thisR.y = source.y + source.height / 2 - thisR.height / 2;
				w.position = thisR;
				SessionState.SetBool( Folders.PREFS_PATH + "was settings", true );
			}

			w.type = t;
			w.GENERATE_RANDOM();
			// w.winScrollPos.Clear();
			// EditorApplication.update -= showw;
		}

		Vector2 mainScrollPos;
		Dictionary<int, Vector2> winScrollPos = new Dictionary<int, Vector2>();
		//int CAT {
		//    get { return SessionState.GetInt( "EMX|HierarchyPRO|Settings|Category", 0 ); }
		//    set { SessionState.SetInt( "EMX|HierarchyPRO|Settings|Category", value ); }
		//}
		string CAT {
			get { return SessionState.GetString( "EMX|HierarchyPRO|Settings|Category", "" ); }
			set { SessionState.SetString( "EMX|HierarchyPRO|Settings|Category", value ); }
		}

		Rect Shrink( Rect r, float v )
		{
			r.x += v;
			r.y += v;
			r.width -= v + v;
			r.height -= v + v;
			return r;
		}



		Type GET_CAT()
		{
			if ( type == null || type == typeof( MainSettingsEnabler_Window ) )
			{
				var t = CAT;
				if ( t == "" ) type = typeof( MainSettingsParams_Window );
				else
				{
					type = GetType().Assembly.GetType( t );
					if ( type == null ) type = typeof( MainSettingsParams_Window );
				}
				GENERATE_RANDOM();
			}
			if ( type.FullName != CAT )
			{
				CAT = type.FullName;
				GENERATE_RANDOM();
			}

			if ( type == typeof( AB_Cache ) ) return typeof( AboutCacheEditor );
			if ( type == typeof( AB_Extensions ) ) return typeof( AboutExtensionsEditor );
			if ( type == typeof( IC_Window ) ) return typeof( IconsforComponentsModSettingsEditor );
			if ( type == typeof( PW_Window ) ) return typeof( ProjectWindowSettingsEditor );
			if ( type == typeof( RM_Window ) ) return typeof( RightHierarchyModsSettingsEditor );
			if ( type == typeof( SA_Window ) ) return typeof( GameObjectSetActiveModSettingsEditor );
			if ( type == typeof( AS_Window ) ) return typeof( AutosaveSceneModSettingsEditor );
			if ( type == typeof( RC_Window ) ) return typeof( RightClickMenuSettingsEditor );
			if ( type == typeof( ST_Window ) ) return typeof( SnapTransformModSettingsEditor );
			if ( type == typeof( TB_Window ) ) return typeof( TopBarsModSettingsEditor );
			if ( type == typeof( MainSettingsEnabler_Window ) ) return typeof( MainSettingsEditor );
			if ( type == typeof( MainSettingsParams_Window ) ) return typeof( MainSettingsParamsEditor );
#if !TRUE
			if ( type == typeof( BB_Window ) ) return typeof( BB_WindowEditor );
			if ( type == typeof( BO_Window ) ) return typeof( BookmarksforGameObjectsSettingsEditor );
			if ( type == typeof( BF_Window ) ) return typeof( BookmarksforProjectviewSettingsEditor );
			if ( type == typeof( LO_Window ) ) return typeof( LastSelectionHistorySettingsEditor );
			if ( type == typeof( HE_Window ) ) return typeof( HierarchyExpandedMemEditor );
			if ( type == typeof( HG_Window ) ) return typeof( HyperGraphSettingsEditor );
			if ( type == typeof( LS_Window ) ) return typeof( LastScenesHistorySettingsEditor );
			if ( type == typeof( HLA_Window ) ) return typeof( HighlighterAutoModSettingsEditor );
			if ( type == typeof( HLM_Window ) ) return typeof( HighlighterManualModSettingsEditor );
			if ( type == typeof( PLA_Window ) ) return typeof( ProjectlighterAutoModSettingsEditor );
			if ( type == typeof( PLM_Window ) ) return typeof( ProjectlighterManualModSettingsEditor );
			if ( type == typeof( PK_Window ) ) return typeof( PlayModeSaverModSettingsEditor );
			if ( type == typeof( PM_Window ) ) return typeof( PresetsManagerModSettingsEditor );
			if ( type == typeof( ProjectSettingsParams_Window ) ) return typeof( ProjectSettingsParamsEditor );
#endif

			throw new Exception( "no window" );
		}


		object[] ob=new object[1];
		Dictionary<int, MethodInfo> _mc = new Dictionary<int, MethodInfo>();
		void DrawGUI( Type t )
		{
			var i = t.FullName.GetHashCode();
			if ( !_mc.ContainsKey( i ) )
			{
				_mc.Add( i, t.GetMethod( "_GUI", (BindingFlags)(~0) ) );
			}
			ob[ 0 ] = this;


			using ( MainRoot.ENABLE( this ).USE( "ENABLE_ALL", 0 ) )
				_mc[ i ].Invoke( null, ob );
		}
		bool changedCat = true;

		void GENERATE_RANDOM()
		{
			start_line = UnityEngine.Random.Range( 0.2f, 0.5f );
			end_line = UnityEngine.Random.Range( 1.7f, 2.1f );
			var RR = 5;
			for ( int i = 0; i < p.Length; i++ )
			{
				rofp[ i ].x = UnityEngine.Random.Range( -RR, RR );
				rofp[ i ].y = UnityEngine.Random.Range( -RR, RR );
				rofp2[ i ].x = UnityEngine.Random.Range( -RR, RR );
				rofp2[ i ].y = UnityEngine.Random.Range( -RR, RR );
			}
			changedCat = true;
		}

		Vector3[] rofp2 = new Vector3[24];
		Vector3[] rofp = new Vector3[24];
		Vector3[] p = new Vector3[24];
		float start_line, end_line;
		Color32 dad =  new Color32( 255, 185, 55 ,255);
		Vector3 tp1, tp2;

		private void OnGUI()
		{
			if ( Root.TemperaryPluginDisabled ) return;



			// if ( EditorApplication.isCompiling ) return;

			//  GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
			// var r1= EditorGUILayout.GetControlRect(GUILayout.Height(1),GUILayout.ExpandWidth(true));
			//var r1= EditorGUILayout.GetControlRect();
			//var r2= EditorGUILayout.GetControlRect();
			//var r3= EditorGUILayout.GetControlRect();
			// GUILayout.EndHorizontal();
			var r1 = position;
			r1.x = 0;
			r1.y = 0;


			// var hr = Shrink( r1, SHR );
			var hr = r1;
			hr.height = 8;
			hr.y += SHR / 2;
			Draw.HRx1( hr );

			hr.y = r1.y + r1.height - SHR / 2;
			Draw.HRx1( hr );
			//if (Event.current.type == EventType.Repaint)
			//Draw.s( "preToolbar" ).Draw( r1, new GUIContent(), false, false, false, false );


			r1.width /= 2;
			r1 = Shrink( r1, SHR );
			r1.width += SHR / 2;

			GUILayout.BeginArea( r1 );
			// GUILayout.Label( "ASD" );
			mainScrollPos = GUILayout.BeginScrollView( mainScrollPos );

			// mainScrollPos = GUILayout.BeginScrollView( mainScrollPos );
			var c = GET_CAT();

			MainSettingsEditor._GUI( this );

			if ( Event.current.type == EventType.Repaint )
				if ( MainSettingsEditor._WritePos.ContainsKey( type ) )
				{
					// var L = 20;
					// var II = L / 2;
					var rect = MainSettingsEditor._WritePos[type];




					p[ 0 ] = new Vector3( rect.x, rect.y, 0 );
					p[ 2 ] = new Vector3( rect.x, rect.y + rect.height, 0 );
					p[ 4 ] = new Vector3( rect.x + rect.width, rect.y + rect.height, 0 );
					p[ 6 ] = new Vector3( rect.x + rect.width, rect.y, 0 );
					for ( int i = 0; i < 4; i++ )
						p[ i * 2 + 1 ] = Vector3.Lerp( p[ i * 2 + 0 ], p[ (i * 2 + 2) % 8 ], 0.5f );
					for ( int i = 8; i < p.Length; i++ ) p[ i ] = p[ i - 8 ];

					for ( int i = 0; i < p.Length; i++ )
					{
						p[ i ].x += rofp[ i ].x;
						p[ i ].y += rofp[ i ].y;
					}


					Root.p[ 0 ].gl.GL_BEGIN();
					tp2 = p[ 0 ];
					/// for ( int i = 0; i < L; i++ )
					/// {
					///     tp1 = tp2;
					///     tp2 = GetBezierPoint( p, i / (L - 1f), i / II, 3 );
					///     var dad =  new Color( 155, 125, 55 );
					///     GL.Color( dad );
					///     ALIAS( tp1, tp2, dad );
					///     GL_VERTEX3( tp1 );
					///     GL_VERTEX3( tp2 );
					/// }
					GL.Color( dad );





					for ( float t = start_line; t <= end_line; t += 0.05f )
					{
						var ind = (int)(t*2)*3;
						if ( ind + 4 >= p.Length ) break;
						if ( tp2 == p[ 0 ] ) tp2 = GetPoint( (t * 2f) % 1f, p.Skip( ind ).ToArray() );
						tp1 = tp2;
						tp2 = GetPoint( (t * 2f) % 1f, p.Skip( ind ).ToArray() );
						ALIAS( tp1, tp2, dad );
						GL_VERTEX3( tp1 );
						GL_VERTEX3( tp2 );
					}

					Root.p[ 0 ].gl.GL_END();
				}

			GUILayout.EndScrollView();

			GUILayout.EndArea();

			var r2=  r1;
			r2.x += r2.width + SHR;
			//r2.width += r2.width + SHR;
			GUILayout.BeginArea( r2 );

			//Debug.Log( EditorGUIUtility.hotControl );

			var id = c.FullName.GetHashCode();
			if ( !winScrollPos.ContainsKey( id ) ) winScrollPos.Add( id, Vector2.zero );
			winScrollPos[ id ] = GUILayout.BeginScrollView( winScrollPos[ id ] );

			DrawGUI( c );
			var s = new Rect( 0, 75, currentWidth() ?? 0 - 50, 2 );
			EditorGUI.DrawRect( s, dad );
			/* {
                 var rect = s;
                 rect.height = 40;
                 rect.y -= rect.height - 10;
                 p[ 0 ] = new Vector3( rect.x, rect.y, 0 );
                 p[ 2 ] = new Vector3( rect.x, rect.y + rect.height, 0 );
                 p[ 4 ] = new Vector3( rect.x + rect.width, rect.y + rect.height, 0 );
                 p[ 6 ] = new Vector3( rect.x + rect.width, rect.y, 0 );
                 for ( int i = 0; i < 4; i++ )
                     p[ i * 2 + 1 ] = Vector3.Lerp( p[ i * 2 + 0 ], p[ (i * 2 + 2) % 8 ], 0.5f );
                 for ( int i = 8; i < p.Length; i++ ) p[ i ] = p[ i - 8 ];
                 for ( int i = 0; i < p.Length; i++ )
                 {
                     p[ i ].x += rofp2[ i ].x;
                     p[ i ].y += rofp2[ i ].y;
                 }

                 Root.p[ 0 ].gl.GL_BEGIN();
                 tp2 = p[ 0 ];
                 GL.Color( dad );
                 for ( float t = start_line; t <= end_line; t += 0.05f )
                 {
                     var ind = (int)(t*2)*3;
                     if ( ind + 4 >= p.Length ) break;
                     if ( tp2 == p[ 0 ] ) tp2 = GetPoint( (t * 2f) % 1f, p.Skip( ind ).ToArray() );
                     tp1 = tp2;
                     tp2 = GetPoint( (t * 2f) % 1f, p.Skip( ind ).ToArray() );
                     ALIAS( tp1, tp2, dad );
                     GL_VERTEX3( tp1 );
                     GL_VERTEX3( tp2 );
                 }
                 Root.p[ 0 ].gl.GL_END();
             }*/

			GUILayout.EndScrollView();

			GUILayout.EndArea();

			var gg = position;
			gg.x = 0;
			gg.y = 0;
			gg = Shrink( gg, SHR );
			gg.x -= SHR;
			gg.width += SHR * 2;
			if ( Event.current.type == EventType.Repaint )
			{
				GUI.BeginClip( gg );
				if ( MainSettingsEditor._WritePos.ContainsKey( type ) )
				{
					bool LINE = true;
#pragma warning disable
					var rect = MainSettingsEditor._WritePos[type];
					var L = LINE ? 3 : 6; //3
					var D = 0f;
					p[ 0 ] = new Vector3( rect.x + rect.width, rect.y, 0 );
					p[ 0 ].x += -mainScrollPos.x + r1.x;
					p[ 0 ].y += -mainScrollPos.y + r1.y - SHR + 4;
					p[ L ].x = s.x + r2.x - winScrollPos[ id ].x;
					p[ L ].y = s.y + r2.y - winScrollPos[ id ].y - SHR + 1;
					for ( int i = 1; i < L; i++ ) p[ i ] = Vector3.Lerp( p[ 0 ], p[ L ], i / (L - D) );


					if ( LINE )
					{
						for ( int i = 1; i < 3; i++ )
						{
							p[ i ].x += rofp[ i ].x * 5;
							p[ i ].y += rofp[ i ].y * 5;
						}
						p[ 2 ].y = Mathf.Lerp( p[ 2 ].y, p[ 3 ].y, 0.95f );
					}
					else
					{
						for ( int i = 1; i < L; i++ )
						{
							p[ i ].x += rofp[ i ].x * 10 * (1 - i / (L - D));
							p[ i ].y += rofp[ i ].y * 1;
							var ler = i/(L-1f);
							if ( ler < 0.5f )
							{
								p[ i ].y = Mathf.Lerp( p[ 0 ].y, p[ i ].y, i / (L - D) * 2 );
								p[ i ].y = Mathf.Lerp( p[ 0 ].y, p[ i ].y, i / (L - D) * 2 );
							}
							else
							{
								p[ i ].y = Mathf.Lerp( p[ i ].y, p[ L ].y, i / (L - D) * 2 - 1 );
								p[ i ].y = Mathf.Lerp( p[ i ].y, p[ L ].y, i / (L - D) * 2 - 1 );
							}
						}
					}



					//p[ 2 ].y = Mathf.Lerp( p[ 2 ].y, p[ 3 ].y, 0.95f );

					Root.p[ 0 ].gl.GL_BEGIN();
					GL.Color( dad );
					//ALIAS( p[ 6 ], p[ 7 ], dad );
					//GL_VERTEX3( p[ 6 ] );
					//GL_VERTEX3( p[ 7 ] );
					tp2 = p[ 0 ];
					for ( float t = 0; t <= 1f; t += 0.05f )
					{
						if ( LINE )
						{
							tp1 = tp2;
							tp2 = GetPoint( (t), p );
						}
						else
						{
							var ind = (int)(t*4);
							if ( ind + 4 >= p.Length ) break;
							tp1 = tp2;
							tp2 = GetPoint( (t) % 1f, p.Skip( ind ).ToArray() );
						}

						// if ( tp2 == p[ 0 ] ) tp2 = GetBezierPoint(p, (t), 0 );

						//tp2 = GetBezierPoint(p, (t), 0 );

						ALIAS( tp1, tp2, dad );
						GL_VERTEX3( tp1 );
						GL_VERTEX3( tp2 );
					}
					tp1 = tp2;
					tp2 = p[ L ];
					ALIAS( tp1, tp2, dad );
					GL_VERTEX3( tp1 );
					GL_VERTEX3( tp2 );
					Root.p[ 0 ].gl.GL_END();
#pragma warning restore
				}
				GUI.EndClip();
			}


			if ( changedCat && Event.current.type == EventType.Repaint )
			{
				Repaint();
				changedCat = false;
			}
		}

		private void OnEnable()
		{
			type = null;
			changedCat = true;
			// GENERATE_RANDOM();
			// Repaint();
		}

		protected void GL_VERTEX3( Vector3 r )
		{
			GL.Vertex3( r.x, r.y, 0 );
		}
		void ALIAS( Vector3 p1, Vector3 p2, Color _c )
		{
			var c = _c;
			c.a *= 0.5f;
			do_al( ref c, 0.3f, ref p1, ref p2 );
			c.a *= 0.5f;
			do_al( ref c, 0.5f, ref p1, ref p2 );
			GL.Color( _c );
		}
		void do_al( ref Color c, float D, ref Vector3 p1, ref Vector3 p2 )
		{
			GL.Color( c );
			p1.x -= D;
			p2.x -= D;
			p1.y -= D;
			p2.y -= D;
			GL_VERTEX3( p1 );
			GL_VERTEX3( p2 );
			p1.x += 2 * D;
			p2.x += 2 * D;
			GL_VERTEX3( p1 );
			GL_VERTEX3( p2 );
			p1.y += 2 * D;
			p2.y += 2 * D;
			GL_VERTEX3( p1 );
			GL_VERTEX3( p2 );
			p1.x -= 2 * D;
			p2.x -= 2 * D;
			GL_VERTEX3( p1 );
			GL_VERTEX3( p2 );
		}
		Vector2 m__tb;
		Vector2 GetBezierPoint( Vector3[] BA, float t, int index, int count = 3 )
		{
			if ( count == 1 )
			{
				return BA[ index ];
			}

			var P0 = GetBezierPoint(BA, t, index, count - 1);
			var P1 = GetBezierPoint(BA, t, index + 1, count - 1);
			m__tb.Set( (1 - t) * P0.x + t * P1.x, (1 - t) * P0.y + t * P1.y );
			return m__tb;
		}
		private Vector2 GetPoint( float t, Vector3[] p )
		{
			float cx = 3 * (p[1].x - p[0].x);
			float cy = 3 * (p[1].y - p[0].y);
			float bx = 3 * (p[2].x- p[1].x) - cx;
			float by = 3 * (p[2].y - p[1].y) - cy;
			float ax = p[3].x - p[0].x - cx - bx;
			float ay = p[3].y- p[0].y - cy - by;
			float Cube = t * t * t;
			float Square = t * t;

			float resX = (ax * Cube) + (bx * Square) + (cx * t) + p[0].x;
			float resY = (ay * Cube) + (by * Square) + (cy * t) + p[0].y;

			return new Vector2( resX, resY );
		}

		// public bool Shown { get { return true; } }
	}


}
