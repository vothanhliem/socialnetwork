//##############################################
//Cᴏᴘʏʀɪɢʜᴛ 2020 DᴏᴜɢʜᴏᴜᴢLɪɢʜᴛ Codecanyon Item 19703216
//Elin Doughouz >> https://www.facebook.com/Elindoughouz
//====================================================

//For the accuracy of the icon and logo, please use this website " https://appicon.co " and add images according to size in folders " mipmap " 

using System.Collections.Generic;
using WoWonder.Helpers.Model;
using static WoWonder.Activities.NativePost.Extra.WRecyclerView;

namespace WoWonder
{
    internal static class AppSettings
    {
        /// <summary>
        /// Deep Links To App Content
        /// you should add your website without http in the analytic.xml file >> ../values/analytic.xml .. line 5
        /// <string name="ApplicationUrlWeb">demo.wowonder.com</string>
        /// </summary>
        // public static readonly string TripleDesAppServiceProvider = "aES+smXZ5GHmSvv3nfN/0CHArzg9PsMbD5RAiU1je3dhKuhZo1xmE8usJ77pE9yOhEkCJl3p+FBlXCcSLJO1R7KIgioahtB29OgWZLndX+3XnKy4GqOrnnzR7e+cTZTnakIwqDKYB8OGSdwB1fIDEIDukvk9dQPusCJucjGFygO+LImH51oX4mzlXb34TZELrseUPXf1hmeMvEf4OIcAT2wd41c8WwmewS6gYCVwyTHqJfYxIL8McQ9G8HQgyRv5Wy4uivkCBT3CqpQM9IIrQcC1dthZMPUUNbynvkcNtNeNEo96w4w3PPYFKh4XT+EqZQKBOiNoQYhiMcU5hcHKi2qP0aiPlyHxvQS4RRaWoV0+CF9gii3wGtAO3pw5NOsoAMNNKR+wKZpLSKZc8GP2KQEqYbBbI7NXl1k3Tm8HTrU7ybkjm7MscWlOcSl73BUah+UOkns3jfcuIiVARKRyqUkXe2qGWcAMPapy8qKRcLclpzxWkD3O8Ua1vWRNALATYzXLzXzgO50Wg6X/k6DokIbJDE0ILK54R2V0/O2dC9cvgHF5fNQj2XMqHe/m41wIfW9riHa/o99YWcxRcVekFiDh4ncLBVN236zpaIBjFD2Nw5cvHu1NOD0Mrpp54CSLTF05R0JnyE7EpyjRDwrdFeCQAGITE72fV9HEmcYvpaCVm+0o9kd9KtoLkGB7wHj7GYqQjCwSU8rxUyqinB582iece7BNIHaZc3HzxKOTfypRv7r5KEWUwq/D1S4XcbDKEikhQG5wx+PA3L1bZgGs39EaBG+2m5DwZhVfWqUzuNbKndvn5KV0FbZf/AyH3GrH2QZas/RhqQynrNs4JjL1J5k4VvgZtGdsEpJzgk3Ez4g8ubBA2zhUd6ukN3s+aoynRwqgEDPx8apLSND9TH6wNGDsjKiB1wWT28OspfOVqnoNwsFBt08tQSATlg9ehrGeEeolwubX7hkCjO7sfZ75piyhwXr23emYLSMTg/b9LuU6lzf7NUX0s+Hyb4ZTezaXWHlqD2gcIITOT7rfcJ4mw8iGEsP3gzRe5emNIDFcEEWZF2OIYX+etRJ3MnZAze5szAQoLvey1OmJa4IpU3R6kDobqwsQtyGZXopOHYihKFCXv/2KIwSgCjuPGJLaw+YXPztbbjJH81MkZl/1RCBtJpG5MmS4Si7SPVAE7pb1LaiN4ilN3jCljywLLnukyV3H0rOL68+2aRr5joQ4w9BZlYwjM5SaUGiwgV5CKhRdZXH8VFHtC+JhjgVAL0VFK5fxNLEi1Ydcs4+FNxjMAIVY+SG9PFqojYx50ZrpqccSBjcG2w9Mg6aaMEhdGq8B3wnpPMvVfSycL5+KqMI8wLz/ZKckLogtpaCmupPX4IbiRNNjIMJOwNKRgbt9ABghPcNR6bQbQZvQXX7OmcY3ArphJaeMdWSkIN2kvEfEEIHDRV1x1BUlO1vop4WHkNDsWb6IMxNp+fh3TaCk0GPJAkJ8gO8vUORiEEgL6/3X3uXsn1HyaL3jFRri9XEMAgsErpk7zeRvqzLDT/6Gkib+DPPntm4D085fPq3mXW4tF5pP7OE=";
        public static readonly string TripleDesAppServiceProvider = "aES+smXZ5GHmSvv3nfN/0CHArzg9PsMbD5RAiU1je3eax/YUNjiroyg53f7fSMHHzC5FIN2p5ilY2uk+38WIaboSkuPVYCmttvdP7iKuC2EUTEnCIMRosXjBGKLqf8YgWOJkJa+GHFseV5fduLDsW4Dukvk9dQPuaXSJlvym0hsPYX+eL96GL5z1nTMq5alW6PUpvMu3RXgXkaaxbnYvCH7BktXZVA7tHOBkx4PfBRJRW7PTu7KbXNqlw22KEJH3iNPYz6mdAZVGSCKjqNb53EL7x45Zhf/DSMXZXX2WQMAk03+XW4kREdHk9yGQEMPHlRhk16UuVjARzgL80TDMuj2iOnbABHEtbFLLsBo2i73uskHuKIESgjVkpQEyKOp4dqds9rhVWS6XU56blroDn67O0c2BsqQS94N86h47jq4wXJA9atpxWaHI4jFhwfPLw20XOYiaOkc9CuUu8PId+UdiG9yTBj5SYcEwWxm9O2anG1mc40NuSomMvyzvHLfQ63ebfKnB3OtDRXllq+zSYjJ46qBkM5xmCcCzibQ0BEaNGJhDLWNcKCmZ8UqdmGgRqZW87AFsxlmuvfmiqS+/mWRBRoZ0xGIouZorHjiQp37yLzG7Wsm4kO8e818JcYucD0qFULXzNUk3AQrkpnqO9MmN8scTRmKbFgeHHAfkQ3qX5eSf/d4l1wTkhwmq4NaxBgWsddz9qrLNqtwRHGi8eFc1TxRwPBCs8HXqzx/wkPSIPF7IXj5e5NyFpAS1U8F8xXCGpmmNLgcvCzyPxT/94WCnsFSVCh1IBho6tRxDUGsYCTYdgHcJbnquaruymguwfmzvtiMsWFsXiF1S/7LPxlIrsdE6g0FBUqqMy3POnfZcV9I6m5gcVcGEWkKPW75bOnc5XdnGkkcLImTjCLd0H3A2oBY/2orCNs/woD2zmahX4j4ODbqUHHZi6YZfuT1uiZsIfSyF6fxSguMdexigPryctPDHOG7LURHysR8Qdk9hfEDYs9gkGRmLX0pE79bHZBboWOdY+oLRV4EB1n/pOknGqQ7oW+CmK+Ukd83MxedTtVzjF95psTl6YH+9PH4yyGru5LywS+A6r/Findw7z/YwhRO57FI57Nh0DHrNijPRefumqVG27y5b6HtALAtXEedP8g2mwE4vbC3myS6g4Pi5WH6qhn8oOH/mCQMksA0MCWAfUvXq3bQO51h/WG3covCFJVVAb2d3kX0tNJoujtmWgYQFQ/t23URP1CmMMIs99ZmzDE7NiYXPNasdGJ5nflHuVKrB9BZRT8QWympbynTo+Eyt0HqwcWMguQDcElf2Pemj6Mum8tY3PKXCjwHYG2ue/Hi120eVlEzTzxZkfc76bpk070/JILaoYVHtrheowCOd+WVSpF1uLReaT+zh";
        //Main Settings >>>>>
        //*********************************************************
        public static string Version = "6.1";
        public static readonly string ApplicationName = "Social BSGĐ";
        public static readonly string DatabaseName = "socialbsgd";

        // Friend system = 0 , follow system = 1
        public static readonly int ConnectivitySystem = 1;

        /// <summary>
        /// When you select the application mode type.. some settings will be changed and some features will be deactivated..
        /// ==============================================
        /// AppMode.Default : (Facebook) Mode
        /// AppMode.LinkedIn : (Jobs) Mode
        /// 
        /// AppMode.Instagram >> #Next Version 
        /// ==============================================
        /// </summary>
        public static readonly AppMode AppMode = AppMode.Default;

        //Main Colors >>
        //*********************************************************
        public static readonly string MainColor = "#47e707";

        //Language Settings >> http://www.lingoes.net/en/translator/langcode.htm
        //*********************************************************
        public static bool FlowDirectionRightToLeft = false;
        public static string Lang = ""; //Default language ar

        //Set Language User on site from phone 
        public static readonly bool SetLangUser = false;

        public static readonly Dictionary<string, string> LanguageList = new Dictionary<string, string>()
        {
            {"en", "English"},
            {"vn", "Việt Nam"},
        };

        //Error Report Mode
        //*********************************************************
        public static readonly bool SetApisReportMode = false;

        //Notification Settings >>
        //*********************************************************
        public static bool ShowNotification = true;
        public static string OneSignalAppId = "ca830e35-bb6f-4527-a4cd-8f89443a7c2e";

        // WalkThrough Settings >>
        //*********************************************************
        public static readonly bool ShowWalkTroutPage = true;

        //Main Messenger settings
        //*********************************************************
        public static readonly bool MessengerIntegration = true;
        public static readonly bool ShowDialogAskOpenMessenger = false;
        public static readonly string MessengerPackageName = "vn.bsgdtphcm.social"; //APK name on Google Play

        //AdMob >> Please add the code ad in the Here and analytic.xml 
        //*********************************************************
        public static readonly ShowAds ShowAds = ShowAds.AllUsers;

        public static readonly bool RewardedAdvertisingSystem = true;

        //Three times after entering the ad is displayed
        public static readonly int ShowAdInterstitialCount = 5;
        public static readonly int ShowAdRewardedVideoCount = 5;
        public static readonly int ShowAdNativeCount = 40;
        public static readonly int ShowAdNativeCommentCount = 7;
        public static readonly int ShowAdNativeReelsCount = 4;
        public static readonly int ShowAdAppOpenCount = 3;

        public static readonly bool ShowAdMobBanner = false;
        public static readonly bool ShowAdMobInterstitial = false;
        public static readonly bool ShowAdMobRewardVideo = false;
        public static readonly bool ShowAdMobNative = false;
        public static readonly bool ShowAdMobNativePost = false;
        public static readonly bool ShowAdMobAppOpen = false;
        public static readonly bool ShowAdMobRewardedInterstitial = false;

        public static readonly string AdInterstitialKey = "";
        public static readonly string AdRewardVideoKey = "";
        public static readonly string AdAdMobNativeKey = "";
        public static readonly string AdAdMobAppOpenKey = "";
        public static readonly string AdRewardedInterstitialKey = "";

        //FaceBook Ads >> Please add the code ad in the Here and analytic.xml 
        //*********************************************************
        public static readonly bool ShowFbBannerAds = false;
        public static readonly bool ShowFbInterstitialAds = false;
        public static readonly bool ShowFbRewardVideoAds = false;
        public static readonly bool ShowFbNativeAds = false;

        //YOUR_PLACEMENT_ID
        public static readonly string AdsFbBannerKey = "";
        public static readonly string AdsFbInterstitialKey = "";
        public static readonly string AdsFbRewardVideoKey = "";
        public static readonly string AdsFbNativeKey = "";
         
        //Ads AppLovin >> Please add the code ad in the Here 
        //*********************************************************  
        public static readonly bool ShowAppLovinBannerAds = false;
        public static readonly bool ShowAppLovinInterstitialAds = false;
        public static readonly bool ShowAppLovinRewardAds = false;

        public static readonly string AdsAppLovinBannerId = "";
        public static readonly string AdsAppLovinInterstitialId = "";
        public static readonly string AdsAppLovinRewardedId = "";
        //********************************************************* 

        public static readonly bool EnableRegisterSystem = false;

        public static readonly bool ShowBirthdayInRegister = false;

        /// <summary>
        /// true => Only over 18 years old
        /// false => all 
        /// </summary>
        public static readonly bool IsUserYearsOld = true;
        public static readonly bool AddAllInfoPorfileAfterRegister = true;

        //Set Theme Full Screen App
        //*********************************************************
        public static readonly bool EnableFullScreenApp = false;

        //Code Time Zone (true => Get from Internet , false => Get From #CodeTimeZone )
        //*********************************************************
        public static readonly bool AutoCodeTimeZone = true;
        public static readonly string CodeTimeZone = "UTC";


        //Social Logins >>
        //If you want login with facebook or google you should change id key in the analytic.xml file 
        //Facebook >> ../values/analytic.xml .. line 10-11 
        //Google >> ../values/analytic.xml .. line 15 
        //*********************************************************
        public static readonly bool EnableSmartLockForPasswords = false;

        public static readonly bool ShowFacebookLogin = true;
        public static readonly bool ShowGoogleLogin = true;

        public static readonly string ClientId = "469121631567-8a80pijt72g8l5797bq2hasn3hqiqcvm.apps.googleusercontent.com";

        //########################### 

        //Main Slider settings
        //*********************************************************
        public static readonly PostButtonSystem PostButton = PostButtonSystem.Reaction;
        public static readonly ToastTheme ToastTheme = ToastTheme.Default;

        /// <summary>
        /// None : To disable Reels video on the app 
        /// </summary>
        public static readonly ReelsPosition ReelsPosition = ReelsPosition.Tab;
        public static readonly bool ShowYouTubeReels = true;
        public static readonly bool ShowUsernameReels = false;


        public static readonly bool ShowBottomAddOnTab = true;

        public static readonly long RefreshAppAPiSeconds = 30000;

        public static readonly bool ShowAlbum = true;
        public static bool ShowArticles = true;
        public static bool ShowPokes = true;
        public static bool ShowCommunitiesGroups = true;
        public static bool ShowCommunitiesPages = true;
        public static bool ShowMarket = true;
        public static readonly bool ShowPopularPosts = true;
        /// <summary>
        /// if selected false will remove boost post and get list Boosted Posts
        /// </summary>
        public static readonly bool ShowBoostedPosts = true;
        public static readonly bool ShowBoostedPages = true;
        public static bool ShowMovies = true;
        public static readonly bool ShowNearBy = true;
        public static bool ShowStory = true;
        public static readonly bool ShowSavedPost = true;
        public static readonly bool ShowUserContacts = true;
        public static bool ShowJobs = true;
        public static bool ShowCommonThings = true;
        public static bool ShowFundings = true;
        public static readonly bool ShowMyPhoto = true;
        public static readonly bool ShowMyVideo = true;
        public static bool ShowGames = true;
        public static bool ShowMemories = true;
        public static readonly bool ShowOffers = true;
        public static readonly bool ShowNearbyShops = true;

        public static readonly bool ShowSuggestedPage = true;
        public static readonly bool ShowSuggestedGroup = true;
        public static readonly bool ShowSuggestedUser = true;

        public static readonly bool ShowCommentImage = true;
        public static readonly bool ShowCommentRecordVoice = true;

        //count times after entering the Suggestion is displayed
        public static readonly int ShowSuggestedPageCount = 90;
        public static readonly int ShowSuggestedGroupCount = 70;
        public static readonly int ShowSuggestedUserCount = 50;

        //allow download or not when share
        public static readonly bool AllowDownloadMedia = true;

        public static readonly bool ShowAdvertise = true;

        /// <summary>
        /// https://rapidapi.com/api-sports/api/covid-193
        /// you can get api key and host from here https://prnt.sc/wngxfc 
        /// </summary>
        public static readonly bool ShowInfoCoronaVirus = false;
        public static readonly string KeyCoronaVirus = "164300ef98msh0911b69bed3814bp131c76jsneaca9364e61f";
        public static readonly string HostCoronaVirus = "covid-193.p.rapidapi.com";

        public static readonly bool ShowLive = true;
        public static readonly string AppIdAgoraLive = "619ee4ec26334d2dae20e52d1abbb32e";

        //Events settings
        //*********************************************************  
        public static bool ShowEvents = true;
        public static readonly bool ShowEventGoing = true;
        public static readonly bool ShowEventInvited = true;
        public static readonly bool ShowEventInterested = true;
        public static readonly bool ShowEventPast = true;

        // Story >>
        //*********************************************************
        //Set a story duration >> Sec
        public static readonly long StoryImageDuration = 7;
        public static readonly long StoryVideoDuration = 30;

        /// <summary>
        /// If it is false, it will appear only for the specified time in the value of the StoryVideoDuration
        /// </summary>
        public static readonly bool ShowFullVideo = false;

        public static readonly bool EnableStorySeenList = true;
        public static readonly bool EnableReplyStory = true;

        /// <summary>
        /// https://dashboard.stipop.io/
        /// you can get api key from here https://prnt.sc/26ofmq9
        /// </summary>
        public static readonly string StickersApikey = "94fdd66ca85c184583c4efad6be358cc";

        //*********************************************************

        /// <summary>
        ///  Currency
        /// CurrencyStatic = true : get currency from app not api 
        /// CurrencyStatic = false : get currency from api (default)
        /// </summary>
        public static readonly bool CurrencyStatic = false;
        public static readonly string CurrencyIconStatic = "$";
        public static readonly string CurrencyCodeStatic = "USD";
        public static readonly string CurrencyFundingPriceStatic = "$";

        //Profile settings
        //*********************************************************
        public static readonly bool ShowGift = true;
        public static readonly bool ShowWallet = true;
        public static readonly bool ShowGoPro = true;
        public static readonly bool ShowAddToFamily = true;

        public static readonly bool ShowUserGroup = false;
        public static readonly bool ShowUserPage = false;
        public static readonly bool ShowUserImage = true;
        public static readonly bool ShowUserSocialLinks = false;

        public static readonly CoverImageStyle CoverImageStyle = CoverImageStyle.CenterCrop;

        /// <summary>
        /// The default value comes from the site .. in case it is not available, it is taken from these values
        /// </summary>
        public static readonly string WeeklyPrice = "3";
        public static readonly string MonthlyPrice = "8";
        public static readonly string YearlyPrice = "89";
        public static readonly string LifetimePrice = "259";

        //Native Post settings
        //********************************************************* 
        public static readonly bool ShowTextWithSpace = true;

        public static readonly bool ShowTextShareButton = false;
        public static readonly bool ShowShareButton = true;

        public static readonly int AvatarPostSize = 60;
        public static readonly int ImagePostSize = 200;
        public static readonly string PostApiLimitOnScroll = "8";

        public static readonly string PostApiLimitOnBackground = "10";

        public static readonly bool AutoPlayVideo = true;

        public static readonly bool EmbedDeepSoundPostType = true;
        public static readonly VideoPostTypeSystem EmbedFacebookVideoPostType = VideoPostTypeSystem.EmbedVideo;
        public static readonly VideoPostTypeSystem EmbedVimeoVideoPostType = VideoPostTypeSystem.EmbedVideo;
        public static readonly VideoPostTypeSystem EmbedPlayTubeVideoPostType = VideoPostTypeSystem.EmbedVideo;
        public static readonly VideoPostTypeSystem EmbedTikTokVideoPostType = VideoPostTypeSystem.Link;
        public static readonly VideoPostTypeSystem EmbedTwitterPostType = VideoPostTypeSystem.Link;
        public static readonly bool ShowSearchForPosts = true;
        public static readonly bool EmbedLivePostType = true;

        public static readonly string PlayTubeShortUrlSite = "https://ptub.app/";

        //new posts users have to scroll back to top
        public static readonly bool ShowNewPostOnNewsFeed = true;
        public static readonly bool ShowAddPostOnNewsFeed = false;
        public static readonly bool ShowCountSharePost = true;

        /// <summary>
        /// Post Privacy
        /// ShowPostPrivacyForAllUser = true : all posts user have icon Privacy 
        /// ShowPostPrivacyForAllUser = false : just my posts have icon Privacy (default)
        /// </summary>
        public static readonly bool ShowPostPrivacyForAllUser = false;

        public static readonly bool EnableVideoCompress = true;
        public static readonly bool EnableFitchOgLink = true;

        /// <summary>
        /// On : if the length of the text is more than 50 characters will be text is bigger
        /// Off : all text same size
        /// </summary>
        public static readonly VolumeState TextSizeDescriptionPost = VolumeState.On;

        //Trending page
        //*********************************************************   
        public static readonly bool ShowTrendingPage = true;

        public static readonly bool ShowProUsersMembers = true;
        public static readonly bool ShowPromotedPages = true;
        public static readonly bool ShowTrendingHashTags = true;
        public static readonly bool ShowLastActivities = true;
        public static readonly bool ShowShortcuts = true;
        public static readonly bool ShowFriendsBirthday = true;
        public static readonly bool ShowAnnouncement = true;

        /// <summary>
        /// https://www.weatherapi.com
        /// </summary>
        public static readonly WeatherType WeatherType = WeatherType.Celsius;
        public static readonly bool ShowWeather = true;
        public static readonly string KeyWeatherApi = "a413d0bf31a44369a16140106221804";

        /// <summary>
        /// https://openexchangerates.org
        /// #Currency >> Your currency
        /// #Currencies >> you can use just 3 from those : USD,EUR,DKK,GBP,SEK,NOK,CAD,JPY,TRY,EGP,SAR,JOD,KWD,IQD,BHD,DZD,LYD,AED,QAR,LBP,OMR,AFN,ALL,ARS,AMD,AUD,BYN,BRL,BGN,CLP,CNY,MYR,MAD,ILS,TND,YER
        /// </summary>
        public static readonly bool ShowExchangeCurrency = false;
        public static readonly string KeyCurrencyApi = "644761ef2ba94ea5aa84767109d6cf7b";
        public static readonly string ExCurrency = "USD";
        public static readonly string ExCurrencies = "EUR,GBP,TRY";
        public static readonly List<string> ExCurrenciesIcons = new List<string>() { "€", "£", "₺" };

        //********************************************************* 

        /// <summary>
        /// you can edit video using FFMPEG 
        /// </summary>
        public static readonly bool EnableVideoEditor = true;

        public static readonly bool EnableImageEditor = true; //#New

        /// <summary>
        /// https://developer.deepar.ai/
        /// - You can get api key from here https://prnt.sc/b4MBmwlx-6Bx
        /// - You can download the files of filters from here: https://mega.nz/file/r7ZzzJBC#XTtZDBwnDIrHvrbeDglYCot3yrcGsfkszd2mq0yDY3Q
        /// - Or you can find more filters here: https://developer.deepar.ai/downloads
        /// </summary> 
        public static readonly bool EnableDeepAr = true; //New
        public static readonly string DeepArKey = "4f405f5502459b5f4fc7b66e5b3851cf4828bc432c96fb03e655fd54a079d65d21fb5ed848f26431"; //#New

        //********************************************************* 

        public static readonly bool ShowUserPoint = true;

        //Add Post
        public static readonly AddPostSystem AddPostSystem = AddPostSystem.AllUsers;

        public static readonly bool ShowGalleryImage = true;
        public static readonly bool ShowGalleryVideo = true;
        public static readonly bool ShowMention = true;
        public static readonly bool ShowLocation = true;
        public static readonly bool ShowFeelingActivity = true;
        public static readonly bool ShowFeeling = true;
        public static readonly bool ShowListening = true;
        public static readonly bool ShowPlaying = true;
        public static readonly bool ShowWatching = true;
        public static readonly bool ShowTraveling = true;
        public static readonly bool ShowGif = true;
        public static readonly bool ShowFile = true;
        public static readonly bool ShowMusic = true;
        public static readonly bool ShowPolls = true;
        public static readonly bool ShowColor = true;
        public static readonly bool ShowVoiceRecord = true;

        public static readonly bool ShowAnonymousPrivacyPost = true;

        //Advertising 
        public static readonly bool ShowAdvertisingPost = true;

        //Settings Page >> General Account
        public static readonly bool ShowSettingsGeneralAccount = true;
        public static readonly bool ShowSettingsAccount = true;
        public static readonly bool ShowSettingsSocialLinks = true;
        public static readonly bool ShowSettingsPassword = true;
        public static readonly bool ShowSettingsBlockedUsers = true;
        public static readonly bool ShowSettingsDeleteAccount = true;
        public static readonly bool ShowSettingsTwoFactor = true;
        public static readonly bool ShowSettingsManageSessions = true;
        public static readonly bool ShowSettingsVerification = true;

        public static readonly bool ShowSettingsSocialLinksFacebook = true;
        public static readonly bool ShowSettingsSocialLinksTwitter = true;
        public static readonly bool ShowSettingsSocialLinksGoogle = true;
        public static readonly bool ShowSettingsSocialLinksVkontakte = true;
        public static readonly bool ShowSettingsSocialLinksLinkedin = true;
        public static readonly bool ShowSettingsSocialLinksInstagram = true;
        public static readonly bool ShowSettingsSocialLinksYouTube = true;

        //Settings Page >> Privacy
        public static readonly bool ShowSettingsPrivacy = true;
        public static readonly bool ShowSettingsNotification = true;

        //Settings Page >> Tell a Friends (Earnings)
        public static readonly bool ShowSettingsInviteFriends = true;

        public static readonly bool ShowSettingsShare = true;
        public static readonly bool ShowSettingsMyAffiliates = true;
        public static readonly bool ShowWithdrawals = true;

        /// <summary>
        /// if you want this feature enabled go to Properties -> AndroidManefist.xml and remove comments from below code
        /// Just replace it with this 5 lines of code
        /// <uses-permission android:name="android.permission.READ_CONTACTS" />
        /// <uses-permission android:name="android.permission.READ_PHONE_NUMBERS" />
        /// </summary>
        public static readonly bool InvitationSystem = true;

        //Settings Page >> Help && Support
        public static readonly bool ShowSettingsHelpSupport = true;

        public static readonly bool ShowSettingsHelp = true;
        public static readonly bool ShowSettingsReportProblem = true;
        public static readonly bool ShowSettingsAbout = true;
        public static readonly bool ShowSettingsPrivacyPolicy = true;
        public static readonly bool ShowSettingsTermsOfUse = true;

        public static readonly bool ShowSettingsRateApp = true;
        public static readonly int ShowRateAppCount = 5;

        public static readonly bool ShowSettingsUpdateManagerApp = false;

        public static readonly bool ShowSettingsInvitationLinks = true;
        public static readonly bool ShowSettingsMyInformation = true;

        public static readonly bool ShowSuggestedUsersOnRegister = true;


        public static readonly bool ShowAddress = true;

        //Set Theme Tab
        //*********************************************************
        public static TabTheme SetTabDarkTheme = TabTheme.Light;
        public static readonly MoreTheme MoreTheme = MoreTheme.Grid;

        //Bypass Web Errors  
        //*********************************************************
        public static readonly bool TurnTrustFailureOnWebException = true;
        public static readonly bool TurnSecurityProtocolType3072On = true;

        //Payment System
        //*********************************************************
        /// <summary>
        /// if you want this feature enabled go to Properties -> AndroidManefist.xml and remove comments from below code
        /// <uses-permission android:name="com.android.vending.BILLING" />
        /// </summary>
        public static readonly bool ShowInAppBilling = true;

        /// <summary>
        /// Paypal and google pay using Braintree Gateway https://www.braintreepayments.com/
        /// 
        /// Add info keys in Payment Methods : https://prnt.sc/1z5bffc - https://prnt.sc/1z5b0yj
        /// To find your merchant ID :  https://prnt.sc/1z59dy8
        ///
        /// Tokenization Keys : https://prnt.sc/1z59smv
        /// </summary>
        public static readonly bool ShowPaypal = true;
        public static readonly string MerchantAccountId = "test";

        public static readonly string SandboxTokenizationKey = "sandbox_kt2f6mdh_hf4ccmn4t7s******";
        public static readonly string ProductionTokenizationKey = "production_t2wns2y2_dfy45******";

        public static readonly bool ShowCreditCard = true;
        public static readonly bool ShowBankTransfer = true;

        public static readonly bool ShowCashFree = true;
        /// <summary>
        /// Currencies : http://prntscr.com/u600ok
        /// </summary>
        public static readonly string CashFreeCurrency = "INR";

        /// <summary>
        /// If you want RazorPay you should change id key in the analytic.xml file
        /// razorpay_api_Key >> .. line 28 
        /// </summary>
        public static readonly bool ShowRazorPay = true;
        /// <summary>
        /// Currencies : https://razorpay.com/accept-international-payments
        /// </summary>
        public static readonly string RazorPayCurrency = "INR";

        public static readonly bool ShowAuthorizeNet = true;
        public static readonly bool ShowSecurionPay = true;
        public static readonly bool ShowIyziPay = true;
        public static readonly bool ShowPayStack = true;
        public static readonly bool ShowPaySera = true;
        public static readonly bool ShowAamarPay = true;

        /// <summary>
        /// FlutterWave get Api Keys From https://app.flutterwave.com/dashboard/settings/apis/live
        /// </summary>
        public static readonly bool ShowFlutterWave = true;
        public static readonly string FlutterWaveCurrency = "NGN";
        public static readonly string FlutterWavePublicKey = "FLWPUBK_TEST-9c877b3110438191127e631c8*****";
        public static readonly string FlutterWaveEncryptionKey = "FLWSECK_TEST298f1****";
        //********************************************************* 

    }
}