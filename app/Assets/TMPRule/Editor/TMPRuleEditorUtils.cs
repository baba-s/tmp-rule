using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace KoganeUnityLib
{
	/// <summary>
	/// TMPRule のエディタに関する汎用機能を管理するクラス
	/// </summary>
	public static class TMPRuleEditorUtils
	{
		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// Unity プロジェクトに存在する TMPRuleSettings を返します
		/// </summary>
		public static TMPRuleSettings GetSettings() =>
			AssetDatabase
				.FindAssets( "t:TMPRuleSettings" )
				.Select( c => AssetDatabase.GUIDToAssetPath( c ) )
				.Select( c => AssetDatabase.LoadAssetAtPath<TMPRuleSettings>( c ) )
				.FirstOrDefault();

		/// <summary>
		/// プレハブのインスタンスの場合 true を返します
		/// </summary>
		private static bool IsPrefabInstance( GameObject go ) =>
			PrefabUtility.GetCorrespondingObjectFromSource( go ) != null &&
			PrefabUtility.GetPrefabInstanceHandle( go ) != null;

		/// <summary>
		/// 現在のシーンのすべての TMPRule の設定を反映します
		/// </summary>
		public static void ApplyAllInScene()
		{
			var list = Resources
			           .FindObjectsOfTypeAll<GameObject>()
			           .Where( c => c.scene.isLoaded )
			           .Where( c => c.hideFlags == HideFlags.None )
			           .Where( c => !IsPrefabInstance( c ) )
			           .Select( c => c.GetComponent<TMPRule>() )
			           .Where( c => c != null )
				;

			var settings = GetSettings();

			foreach ( var n in list )
			{
				Apply( settings, n );
			}
		}

		/// <summary>
		/// 指定された TMPRule を持つオブジェクトに設定を反映します
		/// </summary>
		public static void Apply( TMPRuleSettings settings, TMPRule rule )
		{
			var ruleName = rule.RuleName;

			if ( ruleName == TMPRule.INVALID_RULE_NAME ) return;

			var setting = Array.Find( settings.List, c => c.Name == ruleName );

			if ( setting == null )
			{
				Debug.Log( $"[TMPRule]「{ruleName}」に紐づく TMPRuleParam が見つかりませんでした" );
				return;
			}

			var textMeshPro = rule.GetComponent<TMP_Text>();

			textMeshPro.font         = setting.FontAsset;
			textMeshPro.fontMaterial = setting.Material;
			textMeshPro.fontStyle    = setting.FontStyles;
			textMeshPro.color        = setting.Color;

			if ( setting.IsApplyFontSize )
			{
				textMeshPro.fontSize = setting.FontSize;
			}

			Undo.RecordObject( textMeshPro, "Apply TMP Rule" );
			EditorUtility.SetDirty( textMeshPro );
		}
	}
}