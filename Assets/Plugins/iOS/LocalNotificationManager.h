//
//  LocalNotificationManager.h
//

// 通知がダイアログで表示された際の、アプリを開く挙動をするボタンの表示文言
#define DIALOG_OPEN_MESSAGE @"開く"
// 通知された際に、アイコンにつくバッジの数字
#define ICON_BADGE_NUMBER   1


// 通知予約時間を分刻みにするために、60をかける(基本変えない)
// ken-tanaka 2014.04.17 秒単位で通知する為変更.
#define REVISE_MINUTE       1
// 通知の識別キー(基本変えない)
#define NOTIFICATION_KEY    @"ID"

class LocalNotificationManager {

public:
    void reserveLocalPush(const char *message, int time, int tag);
    void cancelLocalPush( int tag);
};


