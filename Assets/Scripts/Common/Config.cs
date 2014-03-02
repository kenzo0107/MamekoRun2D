// デバッグフラグ.
#define DEBUG
// テスト環境.
#define TEST

public static class Config {

	#region public members.

	public static int		PROJECT_VERSION			= 1;
	public static string	AppliVersionForiOS		= "1.0";
	public static string	AppliVersionForAndroid	= "1.0";

	//　デバッグフラグ.
	public const bool IS_DEBUG = 
#if DEBUG
		true;
#else
		false;
#endif
	
	// 環境向先(テスト環境に向いている場合は【True】).
	public const bool IS_TEST = 
#if TEST
		true;
#else
		false;
#endif

	/// <summary>
	/// シーンリスト.
	/// </summary>
	public enum SceneList {
		Top	= 0,
		Menu,
		PlayRunningGame,
		DemoEasyLocalAndroidNotifications,
	}

	/// <summary>
	/// エラーコード.
	/// </summary>
	public enum ERROR_CODE {
		NO_DEVICE_UID_ERROR		= 100,	// 端末UIDエラー.
		NO_SNS_ID_ERROR			= 101,	// SNS_IDエラー.
		POST_DATA_ERROR			= 200,	// アプリ⇒WEBへのPOSTデータエラー.
		DB_ERROR				= 300,	// DB処理に問題発生.
		DB_QUERY_FAILED			= 301,	// DBクエリに問題発生.
		INCONSISTENT_DATA_ERROR	= 500,	// データ不整合エラー.
		SIGNATURE_ERROR			= 600,	// 課金署名確認エラー.
		FORCE_UPDATE_VERSION	= 800,	// 強制アップデート必須状態.
		NO_RETURN_DATA			= 900,	// サーバからの返答情報がないエラー.
		MAINT_MODE				= 999,	//メンテナンスモード.
	}

	#endregion
}