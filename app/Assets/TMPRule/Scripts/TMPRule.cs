using UnityEngine;

namespace KoganeUnityLib
{
	/// <summary>
	/// TextMesh Pro の設定をルールに沿って変更するコンポーネント
	/// </summary>
	public sealed class TMPRule : MonoBehaviour
	{
		//==============================================================================
		// 定数
		//==============================================================================
		public const string INVALID_RULE_NAME = "無効";

		//==============================================================================
		// プロパティ(SerializeField)
		//==============================================================================
		[field: SerializeField] public string RuleName { get; set; } = INVALID_RULE_NAME;
	}
}