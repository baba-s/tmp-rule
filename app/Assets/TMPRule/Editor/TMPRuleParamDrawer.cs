using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace KoganeUnityLib
{
	/// <summary>
	/// TMPRuleParam の Inspector の表示を変更するエディタ拡張
	/// </summary>
	[CustomPropertyDrawer( typeof( TMPRuleParam ) )]
	public sealed class TMPRuleParamDrawer : PropertyDrawer
	{
		//==============================================================================
		// プロパティ
		//==============================================================================
		public static TMP_FontAsset[] FontAssetList { get; set; }
		public static Material[]      MaterialList  { get; set; }

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// GUI を表示する時に呼び出されます
		/// </summary>
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			using ( new EditorGUI.PropertyScope( position, label, property ) )
			{
				position.height = EditorGUIUtility.singleLineHeight;

				var isLockRect          = new Rect( position );
				var nameRect            = new Rect( position ) { y = isLockRect.yMax + 2, };
				var commentRect         = new Rect( position ) { y = nameRect.yMax + 2, };
				var fontAssetRect       = new Rect( position ) { y = commentRect.yMax + 2, };
				var materialRect        = new Rect( position ) { y = fontAssetRect.yMax + 2, };
				var fontStylesRect      = new Rect( position ) { y = materialRect.yMax + 2, };
				var isApplyFontSizeRect = new Rect( position ) { y = fontStylesRect.yMax + 2, };
				var fontSizeRect        = new Rect( position ) { y = isApplyFontSizeRect.yMax + 2, };
				var colorRect           = new Rect( position ) { y = fontSizeRect.yMax + 2, };

				var isLockProperty          = property.FindPropertyRelative( "m_isLock" );
				var nameProperty            = property.FindPropertyRelative( "m_name" );
				var commentProperty         = property.FindPropertyRelative( "m_comment" );
				var fontAssetProperty       = property.FindPropertyRelative( "m_fontAsset" );
				var materialProperty        = property.FindPropertyRelative( "m_material" );
				var fontStylesProperty      = property.FindPropertyRelative( "m_fontStyles" );
				var isApplyFontSizeProperty = property.FindPropertyRelative( "m_isApplyFontSize" );
				var fontSizeProperty        = property.FindPropertyRelative( "m_fontSize" );
				var colorProperty           = property.FindPropertyRelative( "m_color" );

				PropertyField( "編集不可", isLockRect, isLockProperty );

				var enabled = GUI.enabled;
				GUI.enabled = !isLockProperty.boolValue;

				PropertyField( "ルール名", nameRect, nameProperty );
				PropertyField( "コメント", commentRect, commentProperty );

				DrawFontAsset( fontAssetRect, fontAssetProperty );
				DrawMaterial( materialRect, materialProperty, fontAssetProperty );

				PropertyField( "フォントスタイル", fontStylesRect, fontStylesProperty );
				PropertyField( "フォントサイズ適用", isApplyFontSizeRect, isApplyFontSizeProperty );
				PropertyField( "フォントサイズ", fontSizeRect, fontSizeProperty );
				PropertyField( "文字の色", colorRect, colorProperty );

				GUI.enabled = enabled;
			}
		}

		/// <summary>
		/// PropertyField を実行します
		/// </summary>
		private void PropertyField( string label, Rect position, SerializedProperty property )
		{
			EditorGUI.PropertyField( position, property, new GUIContent( label ) );
		}

		/// <summary>
		/// FontAsset のプルダウンメニューを表示します
		/// </summary>
		private static void DrawFontAsset( Rect rect, SerializedProperty property )
		{
			var fontAssetList = FontAssetList;
			var fontAsset     = property.objectReferenceValue as TMP_FontAsset;
			var index         = Mathf.Max( 0, Array.IndexOf( fontAssetList, fontAsset ) );
			var options       = fontAssetList.Select( c => c.name ).ToArray();

			index                         = EditorGUI.Popup( rect, "Font Asset", index, options );
			property.objectReferenceValue = fontAssetList.ElementAtOrDefault( index );
		}

		/// <summary>
		/// FontAsset のマテリアルのプルダウンメニューを表示します
		/// </summary>
		private static void DrawMaterial
		(
			Rect               rect,
			SerializedProperty property,
			SerializedProperty fontAssetProperty
		)
		{
			var fontAsset    = fontAssetProperty.objectReferenceValue as TMP_FontAsset;
			var materialList = FindAllMaterial( fontAsset );
			var material     = property.objectReferenceValue as Material;
			var index        = Mathf.Max( 0, Array.IndexOf( materialList, material ) );
			var options      = materialList.Select( c => c.name ).ToArray();

			index                         = EditorGUI.Popup( rect, "マテリアル", index, options );
			property.objectReferenceValue = materialList.ElementAtOrDefault( index );
		}

		/// <summary>
		/// Unity プロジェクトに存在するすべての FontAsset のマテリアルを検索します
		/// </summary>
		private static Material[] FindAllMaterial( TMP_FontAsset fontAsset )
		{
			if ( fontAsset == null ) return new Material[0];

			var materials = MaterialList.Where( c => c.name.StartsWith( fontAsset.name ) );

			return new[] { fontAsset.material }
			       .Concat( materials )
			       .ToArray();
		}
	}
}