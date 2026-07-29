//
// ApiDefinitions.cs
//
// Hand-maintained .NET for iOS binding for the Salesforce Marketing Cloud
// MobilePush framework (MarketingCloudSDK) 11.0.2.
//
// Ported from the 8.1.3-era binding by diffing the native headers (the ObjC
// MobilePushSDK categories and MarketingCloudSDK-Swift.h). This file is never
// regenerated with Objective Sharpie; update it by hand against the headers.
//
// Types owned by the SFMCSDK core framework (module/subscriber protocols,
// SFMCSdkComponents, the event classes, the delegate protocols, the shared
// enums) are bound by the sibling SFMCSDK.Net.iOS package and referenced via
// the SFMCSDK namespace. AppGroupSDK types are deliberately not bound here.
//
using System;
using CoreLocation;
using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;
using SFMCSDK;

namespace MarketingCloudSDK
{
	// @interface MobilePushSDK : NSObject
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	interface MobilePushSDK
	{
		// +(instancetype _Nonnull)sharedInstance;
		[Static]
		[Export("sharedInstance")]
		MobilePushSDK SharedInstance { get; }

		// -(BOOL)sfmc_configureWithDictionary:(NSDictionary * _Nonnull)configuration error:(NSError * _Nullable * _Nullable)error;
		[Export("sfmc_configureWithDictionary:error:")]
		bool Configure(NSDictionary configuration, [NullAllowed] out NSError error);

		// -(BOOL)sfmc_configureWithDictionary:(NSDictionary * _Nonnull)configuration error:(NSError * _Nullable * _Nullable)error completionHandler:(void (^ _Nullable)(BOOL, NSString * _Nonnull, NSError * _Nonnull))completionHandler;
		[Export("sfmc_configureWithDictionary:error:completionHandler:")]
		bool Configure(NSDictionary configuration, [NullAllowed] out NSError error, [NullAllowed] Action<bool, NSString, NSError> completionHandler);

		// -(void)sfmc_tearDown;
		[Export("sfmc_tearDown")]
		void TearDown();

		// -(BOOL)sfmc_setContactKey:(NSString * _Nonnull)contactKey;
		[Export("sfmc_setContactKey:")]
		bool SetContactKey(string contactKey);

		// -(NSString * _Nullable)sfmc_contactKey;
		[NullAllowed, Export("sfmc_contactKey")]
		string ContactKey { get; }

		// -(BOOL)sfmc_addTag:(NSString * _Nonnull)tag;
		[Export("sfmc_addTag:")]
		bool AddTag(string tag);

		// -(BOOL)sfmc_removeTag:(NSString * _Nonnull)tag;
		[Export("sfmc_removeTag:")]
		bool RemoveTag(string tag);

		// -(NSSet * _Nullable)sfmc_addTags:(NSArray * _Nonnull)tags;
		[Export("sfmc_addTags:")]
		[return: NullAllowed]
		NSSet<NSString> AddTags(string[] tags);

		// -(NSSet * _Nullable)sfmc_removeTags:(NSArray * _Nonnull)tags;
		[Export("sfmc_removeTags:")]
		[return: NullAllowed]
		NSSet<NSString> RemoveTags(string[] tags);

		// -(NSSet * _Nullable)sfmc_tags;
		[NullAllowed, Export("sfmc_tags")]
		NSSet<NSString> Tags { get; }

		// -(BOOL)sfmc_setAttributeNamed:(NSString * _Nonnull)name value:(NSString * _Nonnull)value;
		[Export("sfmc_setAttributeNamed:value:")]
		bool SetAttributeNamed(string name, string value);

		// -(BOOL)sfmc_clearAttributeNamed:(NSString * _Nonnull)name;
		[Export("sfmc_clearAttributeNamed:")]
		bool ClearAttributeNamed(string name);

		// -(NSDictionary * _Nullable)sfmc_attributes;
		[NullAllowed, Export("sfmc_attributes")]
		NSDictionary Attributes { get; }

		// -(NSDictionary * _Nullable)sfmc_setAttributes:(NSArray * _Nonnull)attributes;
		[Export("sfmc_setAttributes:")]
		[return: NullAllowed]
		NSDictionary SetAttributes(NSObject[] attributes);

		// -(NSDictionary * _Nullable)sfmc_clearAttributesNamed:(NSArray * _Nonnull)attributeNames;
		[Export("sfmc_clearAttributesNamed:")]
		[return: NullAllowed]
		NSDictionary ClearAttributesNamed(NSObject[] attributeNames);

		// -(NSString * _Nullable)sfmc_appID;
		[NullAllowed, Export("sfmc_appID")]
		string AppID { get; }

		// -(NSString * _Nullable)sfmc_accessToken;
		[NullAllowed, Export("sfmc_accessToken")]
		string AccessToken { get; }

		// -(NSString * _Nullable)sfmc_deviceIdentifier;
		[NullAllowed, Export("sfmc_deviceIdentifier")]
		string DeviceIdentifier { get; }

		// -(UNNotificationResponse * _Nonnull)sfmc_notificationResponse __attribute__((availability(ios, introduced=10)));
		[Export("sfmc_notificationResponse")]
		UNNotificationResponse NotificationResponse { get; }

		// -(NSDictionary * _Nonnull)sfmc_notificationUserInfo;
		[Export("sfmc_notificationUserInfo")]
		NSDictionary NotificationUserInfo { get; }

		// -(BOOL)sfmc_pushEnabled;
		[Export("sfmc_pushEnabled")]
		bool IsPushEnabled { get; }

		// -(NSString * _Nullable)sfmc_getSDKState __attribute__((swift_name("sfmc_getSDKState()")));
		[NullAllowed, Export("sfmc_getSDKState")]
		string SDKState { get; }

		// -(void)sfmc_setDebugLoggingEnabled:(BOOL)enabled;
		[Export("sfmc_setDebugLoggingEnabled:")]
		void SetDebugLoggingEnabled(bool enabled);

		// -(BOOL)sfmc_getDebugLoggingEnabled;
		[Export("sfmc_getDebugLoggingEnabled")]
		bool IsDebugLoggingEnabled { get; }

		// -(BOOL)sfmc_refreshWithFetchCompletionHandler:(void (^ _Nullable)(UIBackgroundFetchResult))completionHandler;
		[Export("sfmc_refreshWithFetchCompletionHandler:")]
		bool Refresh([NullAllowed] Action<UIBackgroundFetchResult> completionHandler);

		// -(BOOL)sfmc_setSignedString:(NSString * _Nullable)signedString;
		[Export("sfmc_setSignedString:")]
		bool SetSignedString([NullAllowed] string signedString);

		// -(void)sfmc_setRegistrationCallback:(void (^ _Nonnull)(NSDictionary * _Nonnull))registrationCallback;
		[Export("sfmc_setRegistrationCallback:")]
		void SetRegistrationCallback(Action<NSDictionary> registrationCallback);

		// -(void)sfmc_unsetRegistrationCallback;
		[Export("sfmc_unsetRegistrationCallback")]
		void UnsetRegistrationCallback();

		// -(NSString * _Nullable)sfmc_signedString;
		[NullAllowed, Export("sfmc_signedString")]
		string SignedString { get; }

		// -(BOOL)sfmc_isReady;
		[Export("sfmc_isReady")]
		bool IsReady { get; }

		// -(BOOL)sfmc_isInitializing;
		[Export("sfmc_isInitializing")]
		bool IsInitializing { get; }

		// -(BOOL)sfmc_resetDataPolicy;
		[Export("sfmc_resetDataPolicy")]
		bool ResetDataPolicy ();

		// -(void)sfmc_handlePushFeatureEvent:(SFMCSdkPushFeatureEventBase * _Nonnull)pfEventBase;
		[Export("sfmc_handlePushFeatureEvent:")]
		void HandlePushFeatureEvent(SFMCSdkPushFeatureEventBase pfEventBase);

		// -(void)sfmc_handleInAppMessagingFeatureEvent:(SFMCSdkInAppMessagingAnalyticsEvent * _Nonnull)iamEventBase;
		[Export("sfmc_handleInAppMessagingFeatureEvent:")]
		void HandleInAppMessagingFeatureEvent(SFMCSdkInAppMessagingAnalyticsEvent iamEventBase);

		// -(void)sfmc_handleInAppMessagingDataProcessedEvent:(SFMCSdkInAppMessagingDataProcessedEvent * _Nonnull)dataProcessedEvent;
		[Export("sfmc_handleInAppMessagingDataProcessedEvent:")]
		void HandleInAppMessagingDataProcessedEvent(SFMCSdkInAppMessagingDataProcessedEvent dataProcessedEvent);

		// -(void)sfmc_handleTraceEvent:(SFMCSdkTraceEvent * _Nonnull)traceEvent;
		[Export("sfmc_handleTraceEvent:")]
		void HandleTraceEvent(SFMCSdkTraceEvent traceEvent);
	}

	// @interface Intelligence (MobilePushSDK)
	[Category]
	[BaseType(typeof(MobilePushSDK))]
	interface MobilePushSDK_Intelligence
	{
		// -(BOOL)sfmc_setPiIdentifier:(NSString * _Nullable)identifier;
		[Export("sfmc_setPiIdentifier:")]
		bool SetPiIdentifier([NullAllowed] string identifier);

		// -(NSString * _Nullable)sfmc_piIdentifier;
		[NullAllowed, Export("sfmc_piIdentifier")]
		string GetPiIdentifier();

		// -(void)sfmc_trackMessageOpened:(NSDictionary * _Nonnull)inboxMessage;
		[Export("sfmc_trackMessageOpened:")]
		void TrackMessageOpened(NSDictionary<NSString, NSObject> inboxMessage);

		// -(void)sfmc_trackPageViewWithURL:(NSString * _Nonnull)url title:(NSString * _Nullable)title item:(NSString * _Nullable)item search:(NSString * _Nullable)search;
		[Export("sfmc_trackPageViewWithURL:title:item:search:")]
		void TrackPageView(string url, [NullAllowed] string title, [NullAllowed] string item, [NullAllowed] string search);

		// -(void)sfmc_trackCartContents:(NSDictionary * _Nonnull)cartDictionary;
		[Export("sfmc_trackCartContents:")]
		void TrackCartContents(NSDictionary cartDictionary);

		// -(void)sfmc_trackCartConversion:(NSDictionary * _Nonnull)orderDictionary;
		[Export("sfmc_trackCartConversion:")]
		void TrackCartConversion(NSDictionary orderDictionary);

		// -(NSDictionary * _Nullable)sfmc_cartItemDictionaryWithPrice:(NSNumber * _Nonnull)price quantity:(NSNumber * _Nonnull)quantity item:(NSString * _Nonnull)item uniqueId:(NSString * _Nullable)uniqueId;
		[Export("sfmc_cartItemDictionaryWithPrice:quantity:item:uniqueId:")]
		[return: NullAllowed]
		NSDictionary CartItemDictionary(NSNumber price, NSNumber quantity, string item, [NullAllowed] string uniqueId);

		// -(NSDictionary * _Nullable)sfmc_cartDictionaryWithCartItemDictionaryArray:(NSArray * _Nonnull)cartItemDictionaryArray;
		[Export("sfmc_cartDictionaryWithCartItemDictionaryArray:")]
		[return: NullAllowed]
		NSDictionary CartDictionary(NSObject[] cartItemDictionaryArray);

		// -(NSDictionary * _Nullable)sfmc_orderDictionaryWithOrderNumber:(NSString * _Nonnull)orderNumber shipping:(NSNumber * _Nonnull)shipping discount:(NSNumber * _Nonnull)discount cart:(NSDictionary * _Nonnull)cartDictionary;
		[Export("sfmc_orderDictionaryWithOrderNumber:shipping:discount:cart:")]
		[return: NullAllowed]
		NSDictionary OrderDictionary(string orderNumber, NSNumber shipping, NSNumber discount, NSDictionary cartDictionary);
	}

	// @interface InboxMessages (MobilePushSDK)
	[Category]
	[BaseType(typeof(MobilePushSDK))]
	interface MobilePushSDK_InboxMessages
	{
		// -(NSArray * _Nullable)sfmc_getAllMessages;
		[NullAllowed, Export("sfmc_getAllMessages")]
		NSDictionary<NSString, NSObject>[] AllMessages ();

		// -(NSArray * _Nullable)sfmc_getUnreadMessages;
		[NullAllowed, Export("sfmc_getUnreadMessages")]
		NSDictionary<NSString, NSObject>[] UnreadMessages ();

		// -(NSArray * _Nullable)sfmc_getReadMessages;
		[NullAllowed, Export("sfmc_getReadMessages")]
		NSDictionary<NSString, NSObject>[] ReadMessages ();

		// -(NSArray * _Nullable)sfmc_getDeletedMessages;
		[NullAllowed, Export("sfmc_getDeletedMessages")]
		NSDictionary<NSString, NSObject>[] DeletedMessages ();

		// -(NSUInteger)sfmc_getAllMessagesCount;
		[Export("sfmc_getAllMessagesCount")]
		nuint AllMessagesCount ();

		// -(NSUInteger)sfmc_getUnreadMessagesCount;
		[Export("sfmc_getUnreadMessagesCount")]
		nuint UnreadMessagesCount ();

		// -(NSUInteger)sfmc_getReadMessagesCount;
		[Export("sfmc_getReadMessagesCount")]
		nuint ReadMessagesCount ();

		// -(NSUInteger)sfmc_getDeletedMessagesCount;
		[Export("sfmc_getDeletedMessagesCount")]
		nuint DeletedMessagesCount ();

		// -(BOOL)sfmc_markMessageRead:(NSDictionary * _Nonnull)messageDictionary;
		[Export("sfmc_markMessageRead:")]
		bool MarkMessageRead(NSDictionary<NSString, NSObject> messageDictionary);

		// -(BOOL)sfmc_markMessageDeleted:(NSDictionary * _Nonnull)messageDictionary;
		[Export("sfmc_markMessageDeleted:")]
		bool MarkMessageDeleted(NSDictionary<NSString, NSObject> messageDictionary);

		// -(BOOL)sfmc_markMessageWithIdRead:(NSString * _Nonnull)messageId;
		[Export("sfmc_markMessageWithIdRead:")]
		bool MarkMessageRead(string messageId);

		// -(BOOL)sfmc_markMessageWithIdDeleted:(NSString * _Nonnull)messageId;
		[Export("sfmc_markMessageWithIdDeleted:")]
		bool MarkMessageDeleted(string messageId);

		// -(BOOL)sfmc_markAllMessagesRead;
		[Export("sfmc_markAllMessagesRead")]
		bool MarkAllMessagesRead ();

		// -(BOOL)sfmc_markAllMessagesDeleted;
		[Export("sfmc_markAllMessagesDeleted")]
		bool MarkAllMessagesDeleted ();

		// -(BOOL)sfmc_refreshMessages;
		[Export("sfmc_refreshMessages")]
		bool RefreshMessages ();
	}

	interface IMarketingCloudSDKLocationDelegate
	{
	}

	// @protocol MarketingCloudSDKLocationDelegate <NSObject>
	[Protocol, Model]
	[BaseType(typeof(NSObject))]
	interface MarketingCloudSDKLocationDelegate
	{
		// @required -(BOOL)sfmc_shouldShowLocationMessage:(NSDictionary * _Nonnull)message forRegion:(NSDictionary * _Nonnull)region;
		[Abstract]
		[Export("sfmc_shouldShowLocationMessage:forRegion:"), EventArgs("MarketingCloudSDKLocationDelegateShouldShowLocationMessageForRegion")]
		bool ShouldShowLocationMessageForRegion(NSDictionary message, NSDictionary region);
	}

	// @interface Location (MobilePushSDK)
	[Category]
	[BaseType(typeof(MobilePushSDK))]
	interface MobilePushSDK_Location
	{
		// -(void)sfmc_setLocationDelegate:(id<MarketingCloudSDKLocationDelegate> _Nullable)delegate;
		[Export("sfmc_setLocationDelegate:")]
		void SetLocationDelegate([NullAllowed] IMarketingCloudSDKLocationDelegate @delegate);

		// -(void)sfmc_setSFMCSdkLocationDelegate:(id<SFMCSdkLocationDelegate> _Nullable)delegate;
		[Export("sfmc_setSFMCSdkLocationDelegate:")]
		void SetSFMCSdkLocationDelegate([NullAllowed] ISFMCSdkLocationDelegate @delegate);

		// -(CLRegion * _Nullable)sfmc_regionFromDictionary:(NSDictionary * _Nonnull)dictionary;
		[Export("sfmc_regionFromDictionary:")]
		[return: NullAllowed]
		CLRegion RegionFromDictionary(NSDictionary dictionary);

		// -(BOOL)sfmc_locationEnabled;
		[Export("sfmc_locationEnabled")]
		bool GetLocationEnabled();

		// -(void)sfmc_startWatchingLocation;
		[Export("sfmc_startWatchingLocation")]
		void StartWatchingLocation();

		// -(void)sfmc_stopWatchingLocation;
		[Export("sfmc_stopWatchingLocation")]
		void StopWatchingLocation();

		// -(BOOL)sfmc_watchingLocation;
		[Export("sfmc_watchingLocation")]
		bool WatchingLocation ();

		// -(NSDictionary<NSString *,NSString *> * _Nullable)sfmc_lastKnownLocation;
		[NullAllowed, Export("sfmc_lastKnownLocation")]
		NSDictionary<NSString, NSString> GetLastKnownLocation();
	}

	[Static]
	partial interface Constants
	{
		// extern NSString *const _Nonnull MarketingCloudSDKInboxMessageKey;
		[Field("MarketingCloudSDKInboxMessageKey", "__Internal")]
		NSString MarketingCloudSDKInboxMessageKey { get; }

		// extern NSString *const _Nonnull MarketingCloudSDKErrorDomain;
		[Field("MarketingCloudSDKErrorDomain", "__Internal")]
		NSString MarketingCloudSDKErrorDomain { get; }

		// extern NSNotificationName  _Nonnull const SFMCFrameworkDidSetupNotification;
		[Field("SFMCFrameworkDidSetupNotification", "__Internal")]
		NSString SFMCFrameworkDidSetupNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCFrameworkDidTeardownNotification;
		[Field("SFMCFrameworkDidTeardownNotification", "__Internal")]
		NSString SFMCFrameworkDidTeardownNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCFoundationRegistrationResponseSucceededNotification;
		[Field("SFMCFoundationRegistrationResponseSucceededNotification", "__Internal")]
		NSString SFMCFoundationRegistrationResponseSucceededNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCFoundationUNNotificationReceivedNotification;
		[Field("SFMCFoundationUNNotificationReceivedNotification", "__Internal")]
		NSString SFMCFoundationUNNotificationReceivedNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCInboxMessagesRefreshCompleteNotification;
		[Field("SFMCInboxMessagesRefreshCompleteNotification", "__Internal")]
		NSString SFMCInboxMessagesRefreshCompleteNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCInboxMessagesNewInboxMessagesNotification;
		[Field("SFMCInboxMessagesNewInboxMessagesNotification", "__Internal")]
		NSString SFMCInboxMessagesNewInboxMessagesNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCInboxMessagesUpdateStatusCompleteNotification;
		[Field("SFMCInboxMessagesUpdateStatusCompleteNotification", "__Internal")]
		NSString SFMCInboxMessagesUpdateStatusCompleteNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCInboxMessagesNotificationHandledNotification;
		[Field("SFMCInboxMessagesNotificationHandledNotification", "__Internal")]
		NSString SFMCInboxMessagesNotificationHandledNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCOpenDirectMessageNotificationHandledNotification;
		[Field("SFMCOpenDirectMessageNotificationHandledNotification", "__Internal")]
		NSString SFMCOpenDirectMessageNotificationHandledNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCLocationDidFixLocationNotification;
		[Field("SFMCLocationDidFixLocationNotification", "__Internal")]
		NSString SFMCLocationDidFixLocationNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCLocationDidReceiveLocationUpdateNotification;
		[Field("SFMCLocationDidReceiveLocationUpdateNotification", "__Internal")]
		NSString SFMCLocationDidReceiveLocationUpdateNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCLocationGeofenceRefreshCompleteNotification;
		[Field("SFMCLocationGeofenceRefreshCompleteNotification", "__Internal")]
		NSString SFMCLocationGeofenceRefreshCompleteNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCDidEnterLocationRegionMessageNotification;
		[Field("SFMCDidEnterLocationRegionMessageNotification", "__Internal")]
		NSString SFMCDidEnterLocationRegionMessageNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCDidExitLocationRegionMessageNotification;
		[Field("SFMCDidExitLocationRegionMessageNotification", "__Internal")]
		NSString SFMCDidExitLocationRegionMessageNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCDidDisplayLocationMessageNotification;
		[Field("SFMCDidDisplayLocationMessageNotification", "__Internal")]
		NSString SFMCDidDisplayLocationMessageNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCBeaconRefreshCompleteNotification;
		[Field("SFMCBeaconRefreshCompleteNotification", "__Internal")]
		NSString SFMCBeaconRefreshCompleteNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCDidRangeBeaconLocationMessageNotification;
		[Field("SFMCDidRangeBeaconLocationMessageNotification", "__Internal")]
		NSString SFMCDidRangeBeaconLocationMessageNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCLocationDidStartMonitoringForRegionNotification;
		[Field("SFMCLocationDidStartMonitoringForRegionNotification", "__Internal")]
		NSString SFMCLocationDidStartMonitoringForRegionNotification { get; }

		// extern NSNotificationName  _Nonnull const SFMCFrameworkDidBlockNotification;
		[Field("SFMCFrameworkDidBlockNotification", "__Internal")]
		NSString SFMCFrameworkDidBlockNotification { get; }
	}

	// @interface Events (MobilePushSDK)
	[Category]
	[BaseType(typeof(MobilePushSDK))]
	interface MobilePushSDK_Events
	{
		// -(void)sfmc_track:(id _Nullable)events;
		[Export("sfmc_track:")]
		void Track([NullAllowed] NSObject events);

		// -(BOOL)isTrackingEnabledForEvent:(id<SFMCSdkEvent> _Nonnull)event;
		[Export("isTrackingEnabledForEvent:")]
		bool IsTrackingEnabledForEvent(ISFMCSdkEvent @event);

		// -(void)addEventTrackStats:(id<SFMCSdkEvent> _Nonnull)event;
		[Export("addEventTrackStats:")]
		void AddEventTrackStats(ISFMCSdkEvent @event);
	}

	// @interface FeatureToggle (MobilePushSDK)
	[Category]
	[BaseType(typeof(MobilePushSDK))]
	interface MobilePushSDK_FeatureToggle
	{
		// -(void)sfmc_setAnalyticsEnabled:(BOOL)analyticsEnabled;
		[Export("sfmc_setAnalyticsEnabled:")]
		void SetAnalyticsEnabled(bool analyticsEnabled);

		// -(BOOL)sfmc_isAnalyticsEnabled;
		[Export("sfmc_isAnalyticsEnabled")]
		bool IsAnalyticsEnabled ();

		// -(void)sfmc_setPiAnalyticsEnabled:(BOOL)analyticsEnabled;
		[Export("sfmc_setPiAnalyticsEnabled:")]
		void SetPiAnalyticsEnabled(bool analyticsEnabled);

		// -(BOOL)sfmc_isPiAnalyticsEnabled;
		[Export("sfmc_isPiAnalyticsEnabled")]
		bool IsPiAnalyticsEnabled();

		// -(void)sfmc_setLocationEnabled:(BOOL)locationEnabled;
		[Export("sfmc_setLocationEnabled:")]
		void SetLocationEnabled(bool locationEnabled);

		// -(BOOL)sfmc_isLocationEnabled;
		[Export("sfmc_isLocationEnabled")]
		bool IsLocationEnabled();

		// -(void)sfmc_setInboxEnabled:(BOOL)inboxEnabled;
		[Export("sfmc_setInboxEnabled:")]
		void SetInboxEnabled(bool inboxEnabled);

		// -(BOOL)sfmc_isInboxEnabled;
		[Export("sfmc_isInboxEnabled")]
		bool IsInboxEnabled();
	}

	// @interface MarketingCloudSDKConfigBuilder : NSObject
	[BaseType(typeof(NSObject))]
	interface MarketingCloudSDKConfigBuilder
	{
		// -(NSDictionary * _Nullable)sfmc_build;
		[NullAllowed, Export("sfmc_build")]
		NSDictionary Build ();

		// -(instancetype _Nonnull)sfmc_setApplicationId:(NSString * _Nonnull)setApplicationId;
		[Export("sfmc_setApplicationId:")]
		MarketingCloudSDKConfigBuilder SetApplicationId(string identifier);

		// -(instancetype _Nonnull)sfmc_setAccessToken:(NSString * _Nonnull)setAccessToken;
		[Export("sfmc_setAccessToken:")]
		MarketingCloudSDKConfigBuilder SetAccessToken(string setAccessToken);

		// -(instancetype _Nonnull)sfmc_setLocationEnabled:(NSNumber * _Nonnull)setLocationEnabled;
		[Export("sfmc_setLocationEnabled:")]
		MarketingCloudSDKConfigBuilder SetLocationEnabled(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setInboxEnabled:(NSNumber * _Nonnull)setInboxEnabled;
		[Export("sfmc_setInboxEnabled:")]
		MarketingCloudSDKConfigBuilder SetInboxEnabled(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setPiAnalyticsEnabled:(NSNumber * _Nonnull)setPiAnalyticsEnabled;
		[Export("sfmc_setPiAnalyticsEnabled:")]
		MarketingCloudSDKConfigBuilder SetPiAnalyticsEnabled(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setUseLegacyPIIdentifier:(NSNumber * _Nonnull)etUseLegacyPIIdentifier;
		[Export("sfmc_setUseLegacyPIIdentifier:")]
		MarketingCloudSDKConfigBuilder SetUseLegacyPIIdentifier(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setAnalyticsEnabled:(NSNumber * _Nonnull)setAnalyticsEnabled;
		[Export("sfmc_setAnalyticsEnabled:")]
		MarketingCloudSDKConfigBuilder SetAnalyticsEnabled(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setMid:(NSString * _Nonnull)setMid;
		[Export("sfmc_setMid:")]
		MarketingCloudSDKConfigBuilder SetMid(string mid);

		// -(instancetype _Nonnull)sfmc_setMarketingCloudServerUrl:(NSString * _Nonnull)setMarketingCloudServerUrl;
		[Export("sfmc_setMarketingCloudServerUrl:")]
		MarketingCloudSDKConfigBuilder SetMarketingCloudServerUrl(string setMarketingCloudServerUrl);

		// -(instancetype _Nonnull)sfmc_setApplicationControlsBadging:(NSNumber * _Nonnull)setApplicationControlsBadging;
		[Export("sfmc_setApplicationControlsBadging:")]
		MarketingCloudSDKConfigBuilder SetApplicationControlsBadging(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setDelayRegistrationUntilContactKeyIsSet:(NSNumber * _Nonnull)delayRegistrationUntilContactKeyIsSet;
		[Export("sfmc_setDelayRegistrationUntilContactKeyIsSet:")]
		MarketingCloudSDKConfigBuilder SetDelayRegistrationUntilContactKeyIsSet(NSNumber identifier);

		// -(instancetype _Nonnull)sfmc_setMarkMessageReadOnInboxNotificationOpen:(NSNumber * _Nonnull)markMessageReadOnInboxNotificationOpen;
		[Export("sfmc_setMarkMessageReadOnInboxNotificationOpen:")]
		MarketingCloudSDKConfigBuilder SetMarkMessageReadOnInboxNotificationOpen(NSNumber identifier);
	}

	// SWIFT_CLASS("_TtC17MarketingCloudSDK30MarketingCloudSDKSelectorUtils")
	// @interface MarketingCloudSDKSelectorUtils : NSObject
	[BaseType(typeof(NSObject), Name = "_TtC17MarketingCloudSDK30MarketingCloudSDKSelectorUtils")]
	interface MarketingCloudSDKSelectorUtils
	{
		// +(BOOL)isAppDelegateImplementsSelector:(NSString * _Nonnull)selector __attribute__((warn_unused_result("")));
		[Static]
		[Export("isAppDelegateImplementsSelector:")]
		bool IsAppDelegateImplementsSelector(string selector);

		// +(BOOL)isUserNotificationDelegateImplementsSelector:(NSString * _Nonnull)selector __attribute__((warn_unused_result("")));
		[Static]
		[Export("isUserNotificationDelegateImplementsSelector:")]
		bool IsUserNotificationDelegateImplementsSelector(string selector);
	}

	// SWIFT_CLASS_NAMED("MarketingCloudSdkConfig")
	// @interface SFMarketingCloudSdkConfig : NSObject <SFMCSdkModuleConfig>
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	interface SFMarketingCloudSdkConfig : ISFMCSdkModuleConfig
	{
		// @property (nonatomic) BOOL trackScreens;
		[Export("trackScreens")]
		bool TrackScreens { get; set; }

		// @property (readonly, nonatomic) enum SFMCSdkModuleName name;
		[Export("name")]
		SFMCSdkModuleName Name { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull appId;
		[Export("appId")]
		string AppId { get; }
	}

	// SWIFT_CLASS_NAMED("MarketingCloudSdkConfigBuilder")
	// @interface SFMarketingCloudSdkConfigBuilder : NSObject
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	interface SFMarketingCloudSdkConfigBuilder
	{
		// -(instancetype _Nonnull)initWithAppId:(NSString * _Nonnull)appId __attribute__((objc_designated_initializer));
		[Export("initWithAppId:")]
		[DesignatedInitializer]
		NativeHandle Constructor(string appId);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setAccessToken:(NSString * _Nonnull)accessToken;
		[Export("setAccessToken:")]
		SFMarketingCloudSdkConfigBuilder SetAccessToken(string accessToken);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setMarketingCloudServerUrl:(NSURL * _Nonnull)endpoint;
		[Export("setMarketingCloudServerUrl:")]
		SFMarketingCloudSdkConfigBuilder SetMarketingCloudServerUrl(NSUrl endpoint);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setMid:(NSString * _Nonnull)mid;
		[Export("setMid:")]
		SFMarketingCloudSdkConfigBuilder SetMid(string mid);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setLocationEnabled:(BOOL)enabled;
		[Export("setLocationEnabled:")]
		SFMarketingCloudSdkConfigBuilder SetLocationEnabled(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setInboxEnabled:(BOOL)enabled;
		[Export("setInboxEnabled:")]
		SFMarketingCloudSdkConfigBuilder SetInboxEnabled(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setAnalyticsEnabled:(BOOL)enabled;
		[Export("setAnalyticsEnabled:")]
		SFMarketingCloudSdkConfigBuilder SetAnalyticsEnabled(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setPIAnalyticsEnabled:(BOOL)enabled;
		[Export("setPIAnalyticsEnabled:")]
		SFMarketingCloudSdkConfigBuilder SetPIAnalyticsEnabled(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setUseLegacyPIIdentifier:(BOOL)enabled;
		[Export("setUseLegacyPIIdentifier:")]
		SFMarketingCloudSdkConfigBuilder SetUseLegacyPIIdentifier(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setApplicationControlsBadging:(BOOL)enabled;
		[Export("setApplicationControlsBadging:")]
		SFMarketingCloudSdkConfigBuilder SetApplicationControlsBadging(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setDelayRegistrationUntilContactKeyIsSet:(BOOL)enabled;
		[Export("setDelayRegistrationUntilContactKeyIsSet:")]
		SFMarketingCloudSdkConfigBuilder SetDelayRegistrationUntilContactKeyIsSet(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setEnableScreenEntryTracking:(BOOL)enabled;
		[Export("setEnableScreenEntryTracking:")]
		SFMarketingCloudSdkConfigBuilder SetEnableScreenEntryTracking(bool enabled);

		// -(SFMarketingCloudSdkConfigBuilder * _Nonnull)setMarkMessageReadOnInboxNotificationOpen:(BOOL)enabled;
		[Export("setMarkMessageReadOnInboxNotificationOpen:")]
		SFMarketingCloudSdkConfigBuilder SetMarkMessageReadOnInboxNotificationOpen(bool enabled);

		// -(SFMarketingCloudSdkConfig * _Nonnull)build;
		[Export("build")]
		SFMarketingCloudSdkConfig Build ();
	}

	interface IMarketingCloudSdkInterface
	{
	}

	// SWIFT_PROTOCOL("_TtP17MarketingCloudSDK26MarketingCloudSdkInterface_")
	// @protocol MarketingCloudSdkInterface
	[Protocol(Name = "_TtP17MarketingCloudSDK26MarketingCloudSdkInterface_"), Model]
	[BaseType(typeof(NSObject))]
	interface MarketingCloudSdkInterface
	{
		// @required -(id<SFMCSdkModuleIdentity> _Nullable)getIdentity __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("getIdentity")]
		ISFMCSdkModuleIdentity Identity { get; }

		// @required -(NSString * _Nullable)contactKey __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("contactKey")]
		string ContactKey { get; }

		// @required -(BOOL)addTag:(NSString * _Nonnull)tag __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("addTag:")]
		bool AddTag(string tag);

		// @required -(NSSet * _Nullable)addTags:(NSArray * _Nonnull)tags __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("addTags:")]
		[return: NullAllowed]
		NSSet<NSString> AddTags(string[] tags);

		// @required -(BOOL)removeTag:(NSString * _Nonnull)tag __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("removeTag:")]
		bool RemoveTag(string tag);

		// @required -(NSSet * _Nullable)tags __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("tags")]
		NSSet<NSString> Tags { get; }

		// @required -(void)setDebugLoggingEnabled:(BOOL)enabled;
		[Abstract]
		[Export("setDebugLoggingEnabled:")]
		void SetDebugLoggingEnabled(bool enabled);

		// @required -(NSDictionary * _Nullable)attributes __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("attributes")]
		NSDictionary Attributes { get; }

		// @required -(NSString * _Nullable)accessToken __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("accessToken")]
		string AccessToken { get; }

		// @required -(NSString * _Nullable)deviceIdentifier __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("deviceIdentifier")]
		string DeviceIdentifier { get; }

		// @required -(BOOL)refreshWithFetchCompletionHandler:(void (^ _Nullable)(UIBackgroundFetchResult))completionHandler __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("refreshWithFetchCompletionHandler:")]
		bool Refresh([NullAllowed] Action<UIBackgroundFetchResult> completionHandler);

		// @required -(void)setRegistrationCallback:(void (^ _Nonnull)(NSDictionary * _Nonnull))registrationCallback;
		[Abstract]
		[Export("setRegistrationCallback:")]
		void SetRegistrationCallback(Action<NSDictionary> registrationCallback);

		// @required -(void)unsetRegistrationCallback;
		[Abstract]
		[Export("unsetRegistrationCallback")]
		void UnsetRegistrationCallback();

		// @required -(BOOL)setSignedString:(NSString * _Nullable)signedString __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("setSignedString:")]
		bool SetSignedString([NullAllowed] string signedString);

		// @required -(NSString * _Nullable)signedString __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("signedString")]
		string SignedString { get; }

		// @required -(NSArray * _Nullable)getAllMessages __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("getAllMessages")]
		NSDictionary<NSString, NSObject>[] AllMessages();

		// @required -(NSArray * _Nullable)getUnreadMessages __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("getUnreadMessages")]
		NSDictionary<NSString, NSObject>[] UnreadMessages ();

		// @required -(NSArray * _Nullable)getReadMessages __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("getReadMessages")]
		NSDictionary<NSString, NSObject>[] ReadMessages ();

		// @required -(NSArray * _Nullable)getDeletedMessages __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("getDeletedMessages")]
		NSDictionary<NSString, NSObject>[] DeletedMessages ();

		// @required -(NSUInteger)getAllMessagesCount __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("getAllMessagesCount")]
		nuint AllMessagesCount { get; }

		// @required -(NSUInteger)getUnreadMessagesCount __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("getUnreadMessagesCount")]
		nuint UnreadMessagesCount { get; }

		// @required -(NSUInteger)getReadMessagesCount __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("getReadMessagesCount")]
		nuint ReadMessagesCount { get; }

		// @required -(NSUInteger)getDeletedMessagesCount __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("getDeletedMessagesCount")]
		nuint DeletedMessagesCount { get; }

		// @required -(BOOL)markMessageRead:(NSDictionary * _Nonnull)messageDictionary __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("markMessageRead:")]
		bool MarkMessageRead(NSDictionary<NSString, NSObject> messageDictionary);

		// @required -(BOOL)markMessageDeleted:(NSDictionary * _Nonnull)messageDictionary __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("markMessageDeleted:")]
		bool MarkMessageDeleted(NSDictionary<NSString, NSObject> messageDictionary);

		// @required -(BOOL)markMessageWithIdReadWithMessageId:(NSString * _Nonnull)messageId __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("markMessageWithIdReadWithMessageId:")]
		bool MarkMessageRead(string messageId);

		// @required -(BOOL)markMessageWithIdDeletedWithMessageId:(NSString * _Nonnull)messageId __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("markMessageWithIdDeletedWithMessageId:")]
		bool MarkMessageDeleted(string messageId);

		// @required -(BOOL)markAllMessagesRead __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("markAllMessagesRead")]
		bool MarkAllMessagesRead ();

		// @required -(BOOL)markAllMessagesDeleted __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("markAllMessagesDeleted")]
		bool MarkAllMessagesDeleted ();

		// @required -(BOOL)refreshMessages __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("refreshMessages")]
		bool RefreshMessages ();

		// @required -(BOOL)setPiIdentifier:(NSString * _Nullable)identifier __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("setPiIdentifier:")]
		bool SetPiIdentifier([NullAllowed] string identifier);

		// @required -(NSString * _Nullable)piIdentifier __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("piIdentifier")]
		string PiIdentifier { get; }

		// @required -(void)trackMessageOpened:(NSDictionary * _Nonnull)inboxMessage;
		[Abstract]
		[Export("trackMessageOpened:")]
		void TrackMessageOpened(NSDictionary<NSString, NSObject> inboxMessage);

		// @required -(void)trackPageViewWithUrl:(NSString * _Nonnull)url title:(NSString * _Nullable)title item:(NSString * _Nullable)item search:(NSString * _Nullable)search;
		[Abstract]
		[Export("trackPageViewWithUrl:title:item:search:")]
		void TrackPageView(string url, [NullAllowed] string title, [NullAllowed] string item, [NullAllowed] string search);

		// @required -(void)trackCartContents:(NSDictionary * _Nonnull)cartDictionary;
		[Abstract]
		[Export("trackCartContents:")]
		void TrackCartContents(NSDictionary cartDictionary);

		// @required -(void)trackCartConversion:(NSDictionary * _Nonnull)orderDictionary;
		[Abstract]
		[Export("trackCartConversion:")]
		void TrackCartConversion(NSDictionary orderDictionary);

		// @required -(NSDictionary * _Nullable)cartItemDictionaryWithPrice:(NSNumber * _Nonnull)price quantity:(NSNumber * _Nonnull)quantity item:(NSString * _Nonnull)item uniqueId:(NSString * _Nullable)uniqueId __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("cartItemDictionaryWithPrice:quantity:item:uniqueId:")]
		[return: NullAllowed]
		NSDictionary CartItemDictionary(NSNumber price, NSNumber quantity, string item, [NullAllowed] string uniqueId);

		// @required -(NSDictionary * _Nullable)cartDictionaryWithCartItem:(NSArray * _Nonnull)cartItem __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("cartDictionaryWithCartItem:")]
		[return: NullAllowed]
		NSDictionary CartDictionary(NSObject[] cartItem);

		// @required -(NSDictionary * _Nullable)orderDictionaryWithOrderNumber:(NSString * _Nonnull)orderNumber shipping:(NSNumber * _Nonnull)shipping discount:(NSNumber * _Nonnull)discount cart:(NSDictionary * _Nonnull)cart __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("orderDictionaryWithOrderNumber:shipping:discount:cart:")]
		[return: NullAllowed]
		NSDictionary OrderDictionary(string orderNumber, NSNumber shipping, NSNumber discount, NSDictionary cart);

		// @required -(void)setLocationDelegate:(id<SFMCSdkLocationDelegate> _Nullable)delegate;
		[Abstract]
		[Export("setLocationDelegate:")]
		void SetLocationDelegate([NullAllowed] ISFMCSdkLocationDelegate @delegate);

		// @required -(CLRegion * _Nullable)regionFromDictionary:(NSDictionary * _Nonnull)dictionary __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("regionFromDictionary:")]
		[return: NullAllowed]
		CLRegion RegionFromDictionary(NSDictionary dictionary);

		// @required -(BOOL)locationEnabled __attribute__((warn_unused_result("")));
		// @required -(void)setLocationEnabled:(BOOL)geoFenceEnabled;
		[Abstract]
		[Export("locationEnabled")]
		bool LocationEnabled { get; set; }

		// @required -(void)startWatchingLocation;
		[Abstract]
		[Export("startWatchingLocation")]
		void StartWatchingLocation();

		// @required -(void)stopWatchingLocation;
		[Abstract]
		[Export("stopWatchingLocation")]
		void StopWatchingLocation();

		// @required -(BOOL)watchingLocation __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("watchingLocation")]
		bool WatchingLocation { get; }

		// @required -(NSDictionary<NSString *,NSString *> * _Nullable)lastKnownLocation __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export("lastKnownLocation")]
		NSDictionary<NSString, NSString> LastKnownLocation { get; }

		// @required -(BOOL)resetDataPolicy __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("resetDataPolicy")]
		bool ResetDataPolicy();

		// @required -(BOOL)isAnalyticsEnabled __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("isAnalyticsEnabled")]
		bool IsAnalyticsEnabled { get; }

		// @required -(void)setAnalyticsEnabled:(BOOL)analyticsEnabled;
		[Abstract]
		[Export("setAnalyticsEnabled:")]
		void SetAnalyticsEnabled(bool analyticsEnabled);

		// @required -(BOOL)isPiAnalyticsEnabled __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("isPiAnalyticsEnabled")]
		bool IsPiAnalyticsEnabled { get; }

		// @required -(void)setPiAnalyticsEnabled:(BOOL)analyticsEnabled;
		[Abstract]
		[Export("setPiAnalyticsEnabled:")]
		void SetPiAnalyticsEnabled(bool analyticsEnabled);

		// @required -(BOOL)isLocationEnabled __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("isLocationEnabled")]
		bool IsLocationEnabled { get; }

		// @required -(void)setInboxEnabled:(BOOL)inboxEnabled;
		[Abstract]
		[Export("setInboxEnabled:")]
		void SetInboxEnabled(bool inboxEnabled);

		// @required -(BOOL)isInboxEnabled __attribute__((warn_unused_result("")));
		[Abstract]
		[Export("isInboxEnabled")]
		bool IsInboxEnabled { get; }
	}

	// SWIFT_CLASS_NAMED("MarketingCloudSdk")
	// @interface SFMarketingCloudSdk : NSObject <SFMCModule, MarketingCloudSdkInterface, MarketingCloudSdkProtocol, Subscriber>
	[BaseType(typeof(NSObject))]
	[DisableDefaultCtor]
	interface SFMarketingCloudSdk : ISFMCModule, IMarketingCloudSdkInterface, IMarketingCloudSdkProtocol, ISubscriber
	{
		// @property (nonatomic) enum SFMCSdkModuleName name;
		[Export("name", ArgumentSemantic.Assign)]
		SFMCSdkModuleName Name { get; set; }

		// @property (copy, nonatomic, class) NSString * _Nonnull moduleVersion;
		[Static]
		[Export("moduleVersion")]
		string ModuleVersion { get; set; }

		// @property (copy, nonatomic, class) NSDictionary<NSString *,NSString *> * _Nullable stateProperties;
		[Static]
		[NullAllowed, Export("stateProperties", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> StateProperties { get; set; }

		// @property (readonly, nonatomic, strong, class) SFMCSdkModuleLogger * _Nonnull logger;
		[Static]
		[Export("logger", ArgumentSemantic.Strong)]
		SFMCSdkModuleLogger Logger { get; }

		// +(id<SFMCModule> _Nullable)initModuleWithConfig:(id<SFMCSdkModuleConfig> _Nonnull)config components:(SFMCSdkComponents * _Nonnull)components __attribute__((objc_method_family("none"))) __attribute__((warn_unused_result("")));
		[Static]
		[Export("initModuleWithConfig:components:")]
		[return: NullAllowed]
		ISFMCModule InitModule(ISFMCSdkModuleConfig config, SFMCSdkComponents components);

		// +(void)requestSdk:(void (^ _Nonnull)(id<MarketingCloudSdkInterface> _Nullable))callback;
		[Static]
		[Export("requestSdk:")]
		void RequestSdk(Action<IMarketingCloudSdkInterface> callback);

		// +(enum SFMCSdkModuleStatus)_getStatus __attribute__((warn_unused_result("")));
		[Static]
		[Export("_getStatus")]
		SFMCSdkModuleStatus GetStatus();

		// +(void)sendIdentityEventForTags;
		[Static]
		[Export("sendIdentityEventForTags")]
		void SendIdentityEventForTags();

		// +(void)tearDownModule;
		[Static]
		[Export("tearDownModule")]
		void TearDownModule();

		// -(void)receiveWithMessage:(SFMCSdkMessage * _Nonnull)message;
		[Export("receiveWithMessage:")]
		void Receive(SFMCSdkMessage message);

		// -(id<SFMCSdkModuleIdentity> _Nullable)getIdentity __attribute__((warn_unused_result("")));
		[NullAllowed, Export("getIdentity")]
		ISFMCSdkModuleIdentity Identity { get; }

		// -(void)internalTearDown;
		[Export("internalTearDown")]
		void InternalTearDown();

		// +(NSDictionary<NSString *,id> * _Nonnull)metadata __attribute__((warn_unused_result("")));
		[Static]
		[Export("metadata")]
		NSDictionary<NSString, NSObject> Metadata { get; }

		// -(BOOL)refreshWithFetchCompletionHandler:(void (^ _Nullable)(UIBackgroundFetchResult))completionHandler __attribute__((warn_unused_result("")));
		[Export("refreshWithFetchCompletionHandler:")]
		bool Refresh([NullAllowed] Action<UIBackgroundFetchResult> completionHandler);

		// -(BOOL)resetDataPolicy __attribute__((warn_unused_result("")));
		[Export("resetDataPolicy")]
		bool ResetDataPolicy();

		// -(void)setDebugLoggingEnabled:(BOOL)enabled;
		[Export("setDebugLoggingEnabled:")]
		void SetDebugLoggingEnabled(bool enabled);

		// +(SFMCSdkModuleLogger * _Nonnull)getLogger __attribute__((warn_unused_result("")));
		[Static]
		[Export("getLogger")]
		SFMCSdkModuleLogger GetLogger();

		// -(NSString * _Nullable)contactKey __attribute__((warn_unused_result("")));
		[NullAllowed, Export("contactKey")]
		string ContactKey { get; }

		// -(BOOL)addTag:(NSString * _Nonnull)tag __attribute__((warn_unused_result("")));
		[Export("addTag:")]
		bool AddTag(string tag);

		// -(NSSet * _Nullable)addTags:(NSArray * _Nonnull)tags __attribute__((warn_unused_result("")));
		[Export("addTags:")]
		[return: NullAllowed]
		NSSet<NSString> AddTags(string[] tags);

		// -(BOOL)removeTag:(NSString * _Nonnull)tag __attribute__((warn_unused_result("")));
		[Export("removeTag:")]
		bool RemoveTag(string tag);

		// -(NSSet * _Nullable)tags __attribute__((warn_unused_result("")));
		[NullAllowed, Export("tags")]
		NSSet<NSString> Tags { get; }

		// -(NSDictionary * _Nullable)attributes __attribute__((warn_unused_result("")));
		[NullAllowed, Export("attributes")]
		NSDictionary Attributes { get; }

		// -(NSString * _Nullable)accessToken __attribute__((warn_unused_result("")));
		[NullAllowed, Export("accessToken")]
		string AccessToken { get; }

		// -(NSString * _Nullable)deviceIdentifier __attribute__((warn_unused_result("")));
		[NullAllowed, Export("deviceIdentifier")]
		string DeviceIdentifier { get; }

		// -(BOOL)setSignedString:(NSString * _Nullable)signedString __attribute__((warn_unused_result("")));
		[Export("setSignedString:")]
		bool SetSignedString([NullAllowed] string signedString);

		// -(NSString * _Nullable)signedString __attribute__((warn_unused_result("")));
		[NullAllowed, Export("signedString")]
		string SignedString { get; }

		// -(void)setRegistrationCallback:(void (^ _Nonnull)(NSDictionary * _Nonnull))registrationCallback;
		[Export("setRegistrationCallback:")]
		void SetRegistrationCallback(Action<NSDictionary> registrationCallback);

		// -(void)unsetRegistrationCallback;
		[Export("unsetRegistrationCallback")]
		void UnsetRegistrationCallback();

		// -(void)setAnalyticsEnabled:(BOOL)analyticsEnabled;
		[Export("setAnalyticsEnabled:")]
		void SetAnalyticsEnabled(bool analyticsEnabled);

		// -(BOOL)isAnalyticsEnabled __attribute__((warn_unused_result("")));
		[Export("isAnalyticsEnabled")]
		bool IsAnalyticsEnabled { get; }

		// -(void)setPiAnalyticsEnabled:(BOOL)analyticsEnabled;
		[Export("setPiAnalyticsEnabled:")]
		void SetPiAnalyticsEnabled(bool analyticsEnabled);

		// -(BOOL)isPiAnalyticsEnabled __attribute__((warn_unused_result("")));
		[Export("isPiAnalyticsEnabled")]
		bool IsPiAnalyticsEnabled { get; }

		// -(BOOL)isLocationEnabled __attribute__((warn_unused_result("")));
		[Export("isLocationEnabled")]
		bool IsLocationEnabled { get; }

		// -(BOOL)locationEnabled __attribute__((warn_unused_result("")));
		// -(void)setLocationEnabled:(BOOL)locationEnabled;
		[Export("locationEnabled")]
		bool LocationEnabled { get; set; }

		// -(BOOL)isInboxEnabled __attribute__((warn_unused_result("")));
		[Export("isInboxEnabled")]
		bool IsInboxEnabled { get; }

		// -(void)setInboxEnabled:(BOOL)inboxEnabled;
		[Export("setInboxEnabled:")]
		void SetInboxEnabled(bool inboxEnabled);

		// -(NSArray * _Nullable)getAllMessages __attribute__((warn_unused_result("")));
		[NullAllowed, Export("getAllMessages")]
		NSDictionary<NSString, NSObject>[] AllMessages();

		// -(NSArray * _Nullable)getUnreadMessages __attribute__((warn_unused_result("")));
		[NullAllowed, Export("getUnreadMessages")]
		NSDictionary<NSString, NSObject>[] UnreadMessages ();

		// -(NSArray * _Nullable)getReadMessages __attribute__((warn_unused_result("")));
		[NullAllowed, Export("getReadMessages")]
		NSDictionary<NSString, NSObject>[] ReadMessages ();

		// -(NSArray * _Nullable)getDeletedMessages __attribute__((warn_unused_result("")));
		[NullAllowed, Export("getDeletedMessages")]
		NSDictionary<NSString, NSObject>[] DeletedMessages ();

		// -(NSUInteger)getAllMessagesCount __attribute__((warn_unused_result("")));
		[Export("getAllMessagesCount")]
		nuint AllMessagesCount { get; }

		// -(NSUInteger)getUnreadMessagesCount __attribute__((warn_unused_result("")));
		[Export("getUnreadMessagesCount")]
		nuint UnreadMessagesCount { get; }

		// -(NSUInteger)getReadMessagesCount __attribute__((warn_unused_result("")));
		[Export("getReadMessagesCount")]
		nuint ReadMessagesCount { get; }

		// -(NSUInteger)getDeletedMessagesCount __attribute__((warn_unused_result("")));
		[Export("getDeletedMessagesCount")]
		nuint DeletedMessagesCount { get; }

		// -(BOOL)markMessageRead:(NSDictionary * _Nonnull)messageDictionary __attribute__((warn_unused_result("")));
		[Export("markMessageRead:")]
		bool MarkMessageRead(NSDictionary<NSString, NSObject> messageDictionary);

		// -(BOOL)markMessageDeleted:(NSDictionary * _Nonnull)messageDictionary __attribute__((warn_unused_result("")));
		[Export("markMessageDeleted:")]
		bool MarkMessageDeleted(NSDictionary<NSString, NSObject> messageDictionary);

		// -(BOOL)markMessageWithIdReadWithMessageId:(NSString * _Nonnull)messageId __attribute__((warn_unused_result("")));
		[Export("markMessageWithIdReadWithMessageId:")]
		bool MarkMessageRead(string messageId);

		// -(BOOL)markMessageWithIdDeletedWithMessageId:(NSString * _Nonnull)messageId __attribute__((warn_unused_result("")));
		[Export("markMessageWithIdDeletedWithMessageId:")]
		bool MarkMessageDeleted(string messageId);

		// -(BOOL)markAllMessagesRead __attribute__((warn_unused_result("")));
		[Export("markAllMessagesRead")]
		bool MarkAllMessagesRead ();

		// -(BOOL)markAllMessagesDeleted __attribute__((warn_unused_result("")));
		[Export("markAllMessagesDeleted")]
		bool MarkAllMessagesDeleted ();

		// -(BOOL)refreshMessages __attribute__((warn_unused_result("")));
		[Export("refreshMessages")]
		bool RefreshMessages ();

		// -(BOOL)setPiIdentifier:(NSString * _Nullable)identifier __attribute__((warn_unused_result("")));
		[Export("setPiIdentifier:")]
		bool SetPiIdentifier([NullAllowed] string identifier);

		// -(NSString * _Nullable)piIdentifier __attribute__((warn_unused_result("")));
		[NullAllowed, Export("piIdentifier")]
		string PiIdentifier { get; }

		// -(void)trackMessageOpened:(NSDictionary * _Nonnull)inboxMessage;
		[Export("trackMessageOpened:")]
		void TrackMessageOpened(NSDictionary<NSString, NSObject> inboxMessage);

		// -(void)trackPageViewWithUrl:(NSString * _Nonnull)url title:(NSString * _Nullable)title item:(NSString * _Nullable)item search:(NSString * _Nullable)search;
		[Export("trackPageViewWithUrl:title:item:search:")]
		void TrackPageView(string url, [NullAllowed] string title, [NullAllowed] string item, [NullAllowed] string search);

		// -(void)trackCartContents:(NSDictionary * _Nonnull)cartDictionary;
		[Export("trackCartContents:")]
		void TrackCartContents(NSDictionary cartDictionary);

		// -(void)trackCartConversion:(NSDictionary * _Nonnull)orderDictionary;
		[Export("trackCartConversion:")]
		void TrackCartConversion(NSDictionary orderDictionary);

		// -(NSDictionary * _Nullable)cartItemDictionaryWithPrice:(NSNumber * _Nonnull)price quantity:(NSNumber * _Nonnull)quantity item:(NSString * _Nonnull)item uniqueId:(NSString * _Nullable)uniqueId __attribute__((warn_unused_result("")));
		[Export("cartItemDictionaryWithPrice:quantity:item:uniqueId:")]
		[return: NullAllowed]
		NSDictionary CartItemDictionary(NSNumber price, NSNumber quantity, string item, [NullAllowed] string uniqueId);

		// -(NSDictionary * _Nullable)cartDictionaryWithCartItem:(NSArray * _Nonnull)cartItem __attribute__((warn_unused_result("")));
		[Export("cartDictionaryWithCartItem:")]
		[return: NullAllowed]
		NSDictionary CartDictionary(NSObject[] cartItem);

		// -(NSDictionary * _Nullable)orderDictionaryWithOrderNumber:(NSString * _Nonnull)orderNumber shipping:(NSNumber * _Nonnull)shipping discount:(NSNumber * _Nonnull)discount cart:(NSDictionary * _Nonnull)cart __attribute__((warn_unused_result("")));
		[Export("orderDictionaryWithOrderNumber:shipping:discount:cart:")]
		[return: NullAllowed]
		NSDictionary OrderDictionary(string orderNumber, NSNumber shipping, NSNumber discount, NSDictionary cart);

		// -(void)setLocationDelegate:(id<SFMCSdkLocationDelegate> _Nullable)delegate;
		[Export("setLocationDelegate:")]
		void SetLocationDelegate([NullAllowed] ISFMCSdkLocationDelegate @delegate);

		// -(CLRegion * _Nullable)regionFromDictionary:(NSDictionary * _Nonnull)dictionary __attribute__((warn_unused_result("")));
		[Export("regionFromDictionary:")]
		[return: NullAllowed]
		CLRegion RegionFromDictionary(NSDictionary dictionary);

		// -(void)startWatchingLocation;
		[Export("startWatchingLocation")]
		void StartWatchingLocation();

		// -(void)stopWatchingLocation;
		[Export("stopWatchingLocation")]
		void StopWatchingLocation();

		// -(BOOL)watchingLocation __attribute__((warn_unused_result("")));
		[Export("watchingLocation")]
		bool WatchingLocation { get; }

		// -(NSDictionary<NSString *,NSString *> * _Nullable)lastKnownLocation __attribute__((warn_unused_result("")));
		[NullAllowed, Export("lastKnownLocation")]
		NSDictionary<NSString, NSString> LastKnownLocation { get; }
	}
}
