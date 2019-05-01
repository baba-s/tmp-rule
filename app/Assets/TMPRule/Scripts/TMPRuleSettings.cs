using System;
using TMPro;
using UnityEngine;

namespace KoganeUnityLib
{
	/// <summary>
	/// TextMesh Pro の設定のルールをすべて管理するアセット
	/// </summary>
	[CreateAssetMenu( order = 9999 )]
	public sealed class TMPRuleSettings : ScriptableObject
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private TMPRuleParam[] m_list = null;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public TMPRuleParam[] List => m_list;
	}

	/// <summary>
	/// TextMesh Pro の設定の個別のルールを管理するクラス
	/// </summary>
	[Serializable]
	public sealed class TMPRuleParam
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private bool          m_isLock          = false;
		[SerializeField] private string        m_name            = null;
		[SerializeField] private string        m_comment         = null;
		[SerializeField] private TMP_FontAsset m_fontAsset       = null;
		[SerializeField] private Material      m_material        = null;
		[SerializeField] private FontStyles    m_fontStyles      = 0;
		[SerializeField] private bool          m_isApplyFontSize = false;
		[SerializeField] private int           m_fontSize        = 0;
		[SerializeField] private Color         m_color           = Color.white;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public string        Name            => m_name;
		public string        Comment         => string.IsNullOrWhiteSpace( m_comment ) ? m_name : m_comment;
		public TMP_FontAsset FontAsset       => m_fontAsset;
		public Material      Material        => m_material;
		public FontStyles    FontStyles      => m_fontStyles;
		public bool          IsApplyFontSize => m_isApplyFontSize;
		public int           FontSize        => m_fontSize;
		public Color         Color           => m_color;
	}
}