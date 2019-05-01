using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace KoganeUnityLib
{
	/// <summary>
	/// TMPRuleSettings の Inspector の表示を変更するエディタ拡張
	/// </summary>
	[CustomEditor( typeof( TMPRuleSettings ) )]
	public sealed class TMPRuleSettingsInspector : Editor
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private SerializedProperty m_property;
		private ReorderableList    m_reorderableList;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			TMPRuleParamDrawer.FontAssetList = FindAllFontAsset();
			TMPRuleParamDrawer.MaterialList  = FindAllMaterial();

			m_property = serializedObject.FindProperty( "m_list" );

			m_reorderableList = new ReorderableList( serializedObject, m_property )
			{
				elementHeight       = 168,
				drawElementCallback = OnDrawElement
			};
		}

		/// <summary>
		/// 無効になった時に呼び出されます
		/// </summary>
		private void OnDisable()
		{
			TMPRuleParamDrawer.FontAssetList = null;
			TMPRuleParamDrawer.MaterialList  = null;

			m_property        = null;
			m_reorderableList = null;
		}

		/// <summary>
		/// リストの要素を描画する時に呼び出されます
		/// </summary>
		private void OnDrawElement( Rect rect, int index, bool isActive, bool isFocused )
		{
			var element = m_property.GetArrayElementAtIndex( index );
			rect.height -= 4;
			rect.y      += 2;
			EditorGUI.PropertyField( rect, element );
		}

		/// <summary>
		/// GUI を表示する時に呼び出されます
		/// </summary>
		public override void OnInspectorGUI()
		{
			if ( GUILayout.Button( "現在のシーンのすべてのオブジェクトに反映" ) )
			{
				TMPRuleEditorUtils.ApplyAllInScene();
			}

			serializedObject.Update();
			m_reorderableList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Unity プロジェクトに存在するすべての FontAsset を検索します
		/// </summary>
		private static TMP_FontAsset[] FindAllFontAsset() =>
			AssetDatabase
				.FindAssets( "t:TMP_FontAsset" )
				.Select( c => AssetDatabase.GUIDToAssetPath( c ) )
				.Select( c => AssetDatabase.LoadAssetAtPath<TMP_FontAsset>( c ) )
				.ToArray();

		/// <summary>
		/// Unity プロジェクトに存在するすべてのマテリアルを検索します
		/// </summary>
		private static Material[] FindAllMaterial() =>
			AssetDatabase
				.FindAssets( "t:material" )
				.Select( c => AssetDatabase.GUIDToAssetPath( c ) )
				.Select( c => AssetDatabase.LoadAssetAtPath<Material>( c ) )
				.Where( c => c != null )
				.ToArray();
	}
}