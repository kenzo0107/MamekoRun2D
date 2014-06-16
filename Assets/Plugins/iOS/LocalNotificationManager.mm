//
//  LocalNotificationManager.mm
//

/*!
 @class   :ローカル通知
 @abstract:ローカル通知を管理する
 */

#import "LocalNotificationManager.h"

/*!
 @functiont:通知予約
 @abstract :ローカル通知を設定、予約する
 @param    : message:通知で表示される文言 time:通知をする時間(現在からn分後) tag:ローカル通知の識別タグ
 @result   :無し
 @throws   :無し
 */
void LocalNotificationManager::reserveLocalPush(const char *message, int time, int tag){

    // 通知を作成する
    UILocalNotification *notification = [[UILocalNotification alloc] init];
    // 通知の表示メッセージを設定
    notification.alertBody = [[NSString alloc] initWithUTF8String:message];
    // 通知する時間を設定
    notification.fireDate = [[NSDate date] dateByAddingTimeInterval:REVISE_MINUTE * time];
    // 通知のタグを設定
    NSNumber* tagNumber = [NSNumber numberWithInteger:tag];
    [notification setUserInfo:[NSDictionary  dictionaryWithObject:tagNumber forKey:NOTIFICATION_KEY]];
    // タイムゾーンを設定
    notification.timeZone = [NSTimeZone defaultTimeZone];
    // ダイアログ表示の際の文言設定
    notification.alertAction = DIALOG_OPEN_MESSAGE;
    // 通知音の設定
    notification.soundName = UILocalNotificationDefaultSoundName;
    // バッジの個数を設定
    notification.applicationIconBadgeNumber = ICON_BADGE_NUMBER;
    // 通知を登録する
    [[UIApplication sharedApplication] scheduleLocalNotification:notification];
    // 解放
    [notification release];
}

/*!
 @functiont:通知キャンセル(個別)
 @abstract :ローカル通知の予約を取り消す
 @param    : tag:ローカル通知の識別タグ
 @result   :無し
 @throws   :無し
 */
void LocalNotificationManager::cancelLocalPush( int tag ){
    
    // タグと一致する通知予約をキャンセルする
    for(UILocalNotification *notification in [[UIApplication sharedApplication] scheduledLocalNotifications]) {
        if([[notification.userInfo objectForKey:NOTIFICATION_KEY] integerValue] == tag ) {
            [[UIApplication sharedApplication] cancelLocalNotification:notification];
        }
    }
}

/*!
 @functiont:通知数取得(個別)
 @abstract :ローカル通知予約数を取得する
 @param    :無し
 @result   :無し
 @throws   :無し
 */
int getBadgeCount() {
    
    int badgeCount = 0;
    
    for(UILocalNotification *notification in [[UIApplication sharedApplication] scheduledLocalNotifications]) {
        if ( notification.fireDate > [NSDate date] ) {
            badgeCount  += 1;
        }
    }
    
    return badgeCount;
}




// Interface for Unity.
extern "C" {
    void reserveLocalNotification( const char *message, int time, int tag );
    void cancelLocalNotification( int tag );
    void setBadgeIconNonDisplay( );
    int  getApplicationIconBadgeCountNumber( );
}

/*!
 @functiont:ローカル通知予約
 */
void reserveLocalNotification( const char *message, int time, int tag ) {
    LocalNotificationManager *notification;
    notification->reserveLocalPush( message, time, tag );
}

/*!
 @functiont:ローカル通知予約
 */
void cancelLocalNotification( int tag ) {
    LocalNotificationManager *notification;
    notification->cancelLocalPush( tag );
}

/*!
 @functiont:ローカル通知バッジ非表示処理
 */
void setBadgeIconNonDisplay( ) {
    // 通知を作成する
    UILocalNotification *notification = [[UILocalNotification alloc] init];
    // バッジの個数を設定
    notification.applicationIconBadgeNumber = -1;
    // 通知を登録する
    [[UIApplication sharedApplication] scheduleLocalNotification:notification];
    // 解放
    [notification release];
}

/*!
 @functiont:ローカル通知バッジ数取得
 */
int getApplicationIconBadgeCountNumber( ) {
    return getBadgeCount();
}