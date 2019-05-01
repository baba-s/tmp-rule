using System;
using System.Linq;
using UnityEditor;

namespace KoganeUnityLib
{
	/// <summary>
	/// TMPRule の Inspector の表示を変更するエディタ拡張
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor( typeof( TMPRule ) )]
	public sealed class TMPRuleInspector : Editor
	{
		//==============================================================================
		// 変数
		//==============================================================================
		private TMPRuleSettings m_settings;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 有効になった時に呼び出されます
		/// </summary>
		private void OnEnable()
		{
			m_settings = TMPRuleEditorUtils.GetSettings();

			// 複数選択されている場合に、選択されている
			// すべてのオブジェクトのパラメータを更新するために targets を参照
			foreach ( var n in targets.OfType<TMPRule>() )
			{
				TMPRuleEditorUtils.Apply( m_settings, n );
			}
		}

		/// <summary>
		/// GUI を表示する時に呼び出されます
		/// </summary>
		public override void OnInspectorGUI()
		{
			if ( m_settings == null ) return;

			var rule = target as TMPRule;

			if ( rule == null ) return;

			var list  = m_settings.List;
			var index = Array.FindIndex( list, c => c.Name == rule.RuleName ) + 1;

			// プルダウンメニューの先頭に「無効」を追加
			var invalidOption = new[] { TMPRule.INVALID_RULE_NAME };
			var options       = invalidOption.Concat( list.Select( c => c.Comment ) ).ToArray();

			EditorGUI.BeginChangeCheck();

			index = EditorGUILayout.Popup( "ルール名", index, options );

			if ( !EditorGUI.EndChangeCheck() ) return;

			// 複数選択されている場合に、選択されている
			// すべてのオブジェクトのパラメータを更新するために targets を参照
			var ruleName = index == -1
					? TMPRule.INVALID_RULE_NAME
					: list[ index - 1 ].Name
				;

			foreach ( var n in targets.OfType<TMPRule>() )
			{
				n.RuleName = ruleName;
				TMPRuleEditorUtils.Apply( m_settings, n );
			}
		}
	}
}