using Android.App;
using Android.OS;
using Android.Views;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WoWonder.Activities.NativePost.Extra;
using WoWonder.Activities.Tabbes;
using WoWonder.Helpers.Ads;
using WoWonder.Helpers.Utils;
using WoWonder.MediaPlayers.Exo;
using WoWonderClient;
using WoWonderClient.Classes.Posts;
using WoWonderClient.Classes.Story;
using WoWonderClient.Requests;
using static WoWonder.Helpers.Model.Classes;
using Exception = System.Exception;
using Math = System.Math;
using Uri = Android.Net.Uri;

namespace WoWonder.Activities.NativePost.Post
{
    public class ApiPostAsync
    {
        private readonly Activity ActivityContext;
        private readonly NativePostAdapter NativeFeedAdapter;
        private readonly WRecyclerView WRecyclerView;
        private static bool ShowFindMoreAlert;
        private static PostModelType LastAdsType = PostModelType.AdMob3; // Uncommented this line
        public static List<PostDataObject> PostCacheList { private set; get; }

        public ApiPostAsync(WRecyclerView recyclerView, NativePostAdapter adapter)
        {
            try
            {
                ActivityContext = adapter.ActivityContext;
                NativeFeedAdapter = adapter;
                WRecyclerView = recyclerView;
                PostCacheList = new List<PostDataObject>();
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #region Api V2

        Random rand = new Random();
        private Task task;

        public async Task<List<PostDataObject>> FetchNewsFeedApiPosts(string offset = "0", string typeRun = "Add", string hash = "")
        {
            try
            {
                int apiStatus;
                dynamic respond;

                Console.WriteLine($"[FetchNewsFeedApiPosts] Start - Offset: {offset}, TypeRun: {typeRun}, Hash: {hash}, NativePostType: {NativeFeedAdapter.NativePostType}");

                switch (NativeFeedAdapter.NativePostType)
                {
                    case NativeFeedType.Global:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_news_feed", NativeFeedAdapter.IdParameter, "", WRecyclerView.GetFilter(), "", WRecyclerView.GetPostType());
                        break;
                    case NativeFeedType.User:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_user_posts", NativeFeedAdapter.IdParameter, "", "", "");
                        break;
                    case NativeFeedType.Group:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_group_posts", NativeFeedAdapter.IdParameter, "", "", "");
                        break;
                    case NativeFeedType.Page:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_page_posts", NativeFeedAdapter.IdParameter, "", "", "");
                        break;
                    case NativeFeedType.Event:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_event_posts", NativeFeedAdapter.IdParameter, "", "", "");
                        break;
                    case NativeFeedType.Saved:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "saved", "", "", "", "");
                        break;
                    case NativeFeedType.HashTag:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "hashtag", "", hash, "", "");
                        break;
                    case NativeFeedType.Video:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost("5", offset, "get_random_videos", "", "", "", "");
                        break;
                    case NativeFeedType.Popular:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetPopularPost(AppSettings.PostApiLimitOnScroll, offset);
                        break;
                    case NativeFeedType.Boosted:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetBoostedPost();
                        break;
                    case NativeFeedType.Live:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetLivePost();
                        break;
                    //case NativeFeedType.Advertise:
                    //    (apiStatus, respond) = await RequestsAsync.Advertise.GetAdvertisePost(AppSettings.PostApiLimitOnScroll, offset);
                    //    break;
                    default:
                        (apiStatus, respond) = (400, null);
                        break;
                }

                Trace.EndSection();

                if (WRecyclerView.SwipeRefreshLayoutView is { Refreshing: true })
                    WRecyclerView.SwipeRefreshLayoutView.Refreshing = false;

                Console.WriteLine($"[FetchFeedPostsApi] API Response - Status: {apiStatus}, Response: {respond}");

                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                {
                    WRecyclerView.MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(ActivityContext, respond);
                    return new List<PostDataObject>(); // Return empty list on error
                }
                else
                {
                    LoadDataApi(apiStatus, respond, offset);
                }

                return result?.Data ?? new List<PostDataObject>(); // Return data or empty list if null
            }
            catch (Exception ex)
            {
                Methods.DisplayReportResultTrack(ex);
                return new List<PostDataObject>(); // Return empty list on exception
            }
        }

        public void ExcuteDataToMainThread(string offset = "0", string typeRun = "Add", string hash = "")
        {
            try
            {
                Console.WriteLine($"[ExcuteDataToMainThread] Start - Offset: {offset}, TypeRun: {typeRun}, Hash: {hash}");

                var beforeList = NativeFeedAdapter.ListDiffer.Count;

                if (beforeList > 150)
                {
                    NativeFeedAdapter.ListDiffer.RemoveRange(10, Math.Min(30, beforeList));
                    NativeFeedAdapter.NotifyItemRangeRemoved(10, Math.Min(30, beforeList));
                }

                task = Task.Run(async () => await FetchNewsFeedApiPosts(offset, typeRun, hash)).ContinueWith(t =>
                {
                    try
                    {
                        var newPostsList = t.Result as List<PostDataObject>; // Ensure proper type casting
                        if (newPostsList == null || newPostsList.Count == 0)
                        {
                            Console.WriteLine("[ExcuteDataToMainThread] No new posts to process.");
                            return;
                        }

                        NativeFeedAdapter.ListDiffer.AddRange(newPostsList.Select(post => new AdapterModelsClass { PostData = post })); // Convert to AdapterModelsClass

                        ActivityContext?.RunOnUiThread(() =>
                        {
                            if (beforeList == 0)
                                NativeFeedAdapter.NotifyDataSetChanged();
                            else
                                NativeFeedAdapter.NotifyItemRangeInserted(beforeList, newPostsList.Count);

                            Console.WriteLine("[ExcuteDataToMainThread] Updated UI with new posts. Total count: " + NativeFeedAdapter.ListDiffer.Count);
                        });

                        WRecyclerView.MainScrollEvent.IsLoading = false;
                        WRecyclerView.Visibility = ViewStates.Visible;
                        WRecyclerView?.ShimmerInflater?.Hide();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[ExcuteDataToMainThread] Exception: {e.Message}");
                        Methods.DisplayReportResultTrack(e);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ExcuteDataToMainThread] Exception: {ex.Message}");
                Methods.DisplayReportResultTrack(ex);
            }
        }

        public async Task FetchFeedPostsApi(string offset = "0", string typeRun = "Add", string hash = "")
        {
            try
            {
                if (task != null && (task.IsCompleted == false || task.Status == TaskStatus.Running))
                    return;

                int apiStatus;
                dynamic respond;
                WRecyclerView.Hash = hash;

                if (WRecyclerView.MainScrollEvent.IsLoading)
                    return;

                var adId = NativeFeedAdapter.ListDiffer.LastOrDefault(a => a.TypeView == PostModelType.AdsPost && a.PostData.PostType == "ad")?.PostData?.Id ?? "";

                Console.WriteLine($"[FetchFeedPostsApi] Start - Offset: {offset}, TypeRun: {typeRun}, Hash: {hash}");
                Trace.BeginSection("API = Started FetchNewsFeedApi " + offset);
                WRecyclerView.MainScrollEvent.IsLoading = true;

                switch (NativeFeedAdapter.NativePostType)
                {
                    case NativeFeedType.Global:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_news_feed", NativeFeedAdapter.IdParameter, "", WRecyclerView.GetFilter(), adId, WRecyclerView.GetPostType());
                        break;
                    case NativeFeedType.User:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_user_posts", NativeFeedAdapter.IdParameter, "", "", adId);
                        break;
                    case NativeFeedType.Group:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_group_posts", NativeFeedAdapter.IdParameter, "", "", adId);
                        break;
                    case NativeFeedType.Page:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_page_posts", NativeFeedAdapter.IdParameter, "", "", adId);
                        break;
                    case NativeFeedType.Event:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_event_posts", NativeFeedAdapter.IdParameter, "", "", adId);
                        break;
                    case NativeFeedType.Saved:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "saved", "", "", "", adId);
                        break;
                    case NativeFeedType.HashTag:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "hashtag", "", hash, "", adId);
                        break;
                    case NativeFeedType.Video:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost("5", offset, "get_random_videos", "", "", "", adId);
                        break;
                    case NativeFeedType.Popular:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetPopularPost(AppSettings.PostApiLimitOnScroll, offset);
                        break;
                    case NativeFeedType.Boosted:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetBoostedPost();
                        break;
                    case NativeFeedType.Live:
                        (apiStatus, respond) = await RequestsAsync.Posts.GetLivePost();
                        break;
                    default:
                        (apiStatus, respond) = (400, null);
                        break;
                }

                Trace.EndSection();

                if (WRecyclerView.SwipeRefreshLayoutView is { Refreshing: true })
                    WRecyclerView.SwipeRefreshLayoutView.Refreshing = false;

                Console.WriteLine($"[FetchFeedPostsApi] API Response - Status: {apiStatus}, Response: {respond}");

                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                {
                    WRecyclerView.MainScrollEvent.IsLoading = false; // Reset IsLoading
                    Methods.DisplayReportResult(ActivityContext, respond);
                    Console.WriteLine("[FetchFeedPostsApi] API call failed or returned no data.");
                    return; // Exit early to avoid further processing
                }
                else
                {
                    LoadDataApi(apiStatus, respond, offset);
                }

                Trace.EndSection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FetchFeedPostsApi] Exception: {ex.Message}");
                Methods.DisplayReportResultTrack(ex);
            }
        }

        public async Task FetchSearchForPosts(string offset, string id, string searchQuery, string type)
        {
            if (!Methods.CheckConnectivity())
                return;

            var (apiStatus, respond) = await RequestsAsync.Posts.SearchForPosts(AppSettings.PostApiLimitOnScroll, offset, id, searchQuery, type);
            if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
            {
                WRecyclerView.MainScrollEvent.IsLoading = false;
                Methods.DisplayReportResult(ActivityContext, respond);
            }
            else LoadDataApi(apiStatus, respond, offset);
        }

        public async Task FetchMostLikedPosts(string offset, string lastTotal, string dt)
        {
            if (!Methods.CheckConnectivity())
                return;

            var (apiStatus, respond) = await RequestsAsync.Posts.MostLikedAsync(AppSettings.PostApiLimitOnScroll, offset, lastTotal, dt);
            if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
            {
                WRecyclerView.MainScrollEvent.IsLoading = false;
                Methods.DisplayReportResult(ActivityContext, respond);
            }
            else LoadDataApi(apiStatus, respond, offset);
        }

        public void LoadDataApi(int apiStatus, dynamic respond, string offset, string typeRun = "Add")
        {
            try
            {
                if (respond is PostObject result)
                {
                    if (WRecyclerView.SwipeRefreshLayoutView is { Refreshing: true })
                        WRecyclerView.SwipeRefreshLayoutView.Refreshing = false;

                    var countList = NativeFeedAdapter.ItemCount;
                    if (result.Data.Count > 0)
                    {
                        result.Data.RemoveAll(a => a.Publisher == null && a.UserData == null);
                        GetAllPostLive(result.Data);

                        if (offset == "0" && countList > 10 && typeRun == "Insert" && NativeFeedAdapter.NativePostType == NativeFeedType.Global)
                        {
                            result.Data.Reverse();
                            bool add = false;

                            foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData?.PostId == post.PostId && a.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                            {
                                add = true;
                                ListUtils.NewPostList.Add(post);
                            }

                            ActivityContext?.RunOnUiThread(() =>
                            {
                                try
                                {
                                    if (add && WRecyclerView.PopupBubbleView != null && WRecyclerView.PopupBubbleView.Visibility != ViewStates.Visible && AppSettings.ShowNewPostOnNewsFeed)
                                        WRecyclerView.PopupBubbleView.Visibility = ViewStates.Visible;
                                }
                                catch (Exception e)
                                {
                                    Methods.DisplayReportResultTrack(e);
                                }
                            });
                        }
                        else
                        {
                            bool add = false;

                            foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                            {
                                add = true;
                                var combiner = new FeedCombiner(null, NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);
                                combiner.CombineDefaultPostSections();
                            }

                            if (add)
                            {
                                ActivityContext?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        NativeFeedAdapter.NotifyItemRangeInserted(countList, NativeFeedAdapter.ListDiffer.Count - countList);
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                });
                            }
                        }
                    }

                    ActivityContext?.RunOnUiThread(() =>
                    {
                        try
                        {
                            WRecyclerView.Visibility = ViewStates.Visible;
                            WRecyclerView?.ShimmerInflater?.Hide();
                        }
                        catch (Exception e)
                        {
                            Methods.DisplayReportResultTrack(e);
                        }
                    });
                }

                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
        }

        public void LoadTopDataApi(List<PostDataObject> list)
        {
            try
            {
                NativeFeedAdapter.ListDiffer.Clear();
                NativeFeedAdapter.NotifyDataSetChanged();

                var combiner = new FeedCombiner(null, NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);
                combiner.AddPostBoxPostView("feed", -1);

                switch (AppSettings.ShowStory)
                {
                    case true:
                        combiner.AddStoryPostView(new List<StoryDataObject>());
                        break;
                }

                switch (list.Count)
                {
                    case > 0:
                        {
                            bool add = false;
                            foreach (var post in from post in list let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                            {
                                add = true;
                                var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);
                                switch (post.PostType)
                                {
                                    case "ad" when AppSettings.ShowAdvertise:
                                        combine.AddAdsPost();
                                        break;
                                    default:
                                        combine.CombineDefaultPostSections();
                                        break;
                                }
                            }

                            switch (PostCacheList?.Count)
                            {
                                case > 0:
                                    LoadBottomDataApi(PostCacheList.Take(30).ToList());
                                    break;
                            }

                            switch (add)
                            {
                                case true:
                                    ActivityContext?.RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            NativeFeedAdapter.NotifyDataSetChanged();
                                            ListUtils.NewPostList.Clear();
                                        }
                                        catch (Exception e)
                                        {
                                            Methods.DisplayReportResultTrack(e);
                                        }
                                    });
                                    break;
                            }

                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void InsertTopDataApi(int apiStatus, dynamic respond)
        {
            try
            {
                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                {
                    WRecyclerView.MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                {
                    result.Data.RemoveAll(a => a.Publisher == null && a.UserData == null);
                    GetAllPostLive(result.Data);
                    result.Data.Reverse();

                    switch (result.Data.Count)
                    {
                        case > 0:
                            {
                                bool add = false;
                                foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                                {
                                    add = true;
                                    var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);
                                    switch (post.PostType)
                                    {
                                        case "ad" when AppSettings.ShowAdvertise:
                                            combine.AddAdsPost("Top");
                                            break;
                                        default:
                                            combine.CombineDefaultPostSections("Top");
                                            break;
                                    }
                                }

                                switch (add)
                                {
                                    case true:
                                        ActivityContext?.RunOnUiThread(() =>
                                        {
                                            try
                                            {
                                                NativeFeedAdapter.NotifyDataSetChanged();
                                                ListUtils.NewPostList.Clear();
                                            }
                                            catch (Exception e)
                                            {
                                                Methods.DisplayReportResultTrack(e);
                                            }
                                        });
                                        break;
                                }

                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public void LoadMemoriesDataApi(int apiStatus, dynamic respond, List<AdapterModelsClass> diffList)
        {
            try
            {
                switch (WRecyclerView.MainScrollEvent.IsLoading)
                {
                    case true:
                        return;
                }

                WRecyclerView.MainScrollEvent.IsLoading = true;

                if (apiStatus != 200 || respond is not FetchMemoriesObject result || result.Data == null)
                {
                    WRecyclerView.MainScrollEvent.IsLoading = false;
                    Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                {
                    if (WRecyclerView.SwipeRefreshLayoutView != null && WRecyclerView.SwipeRefreshLayoutView.Refreshing)
                        WRecyclerView.SwipeRefreshLayoutView.Refreshing = false;

                    var countList = NativeFeedAdapter.ItemCount;
                    switch (result.Data.Posts.Count)
                    {
                        case > 0:
                            {
                                result.Data.Posts.RemoveAll(a => a.Publisher == null && a.UserData == null);
                                result.Data.Posts.Reverse();

                                foreach (var post in from post in result.Data.Posts let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                                {
                                    switch (post.Publisher)
                                    {
                                        case null when post.UserData == null:
                                            continue;
                                        default:
                                            {
                                                var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);
                                                combine.CombineDefaultPostSections();
                                                break;
                                            }
                                    }
                                }

                                ActivityContext?.RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        WRecyclerView.Visibility = ViewStates.Visible;
                                        WRecyclerView?.ShimmerInflater?.Hide();

                                        var d = new Runnable(() => { NativeFeedAdapter.NotifyItemRangeInserted(countList, NativeFeedAdapter.ListDiffer.Count - countList); }); d.Run();
                                        GC.Collect();
                                    }
                                    catch (Exception e)
                                    {
                                        Methods.DisplayReportResultTrack(e);
                                    }
                                });
                                break;
                            }
                    }
                }

                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
        }

        public async Task FetchLoadMoreNewsFeedApiPosts()
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] No internet connection.");
                    return;
                }

                if (NativeFeedAdapter.NativePostType != NativeFeedType.Global)
                {
                    Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] NativePostType is not Global.");
                    return;
                }

                if (PostCacheList?.Count > 40)
                {
                    Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] PostCacheList already contains more than 40 items.");
                    return;
                }

                var diff = NativeFeedAdapter.ListDiffer;
                var list = new List<AdapterModelsClass>(diff);
                if (list.Count <= 20)
                {
                    Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] Not enough items in ListDiffer to fetch more posts.");
                    return;
                }

                var item = list.LastOrDefault();
                string offset;

                switch (item?.TypeView)
                {
                    case PostModelType.Divider:
                    case PostModelType.ViewProgress:
                    case PostModelType.AdMob1:
                    case PostModelType.AdMob2:
                    case PostModelType.AdMob3:
                    case PostModelType.FbAdNative:
                    case PostModelType.AdsPost:
                    case PostModelType.SuggestedPagesBox:
                    case PostModelType.SuggestedGroupsBox:
                    case PostModelType.SuggestedUsersBox:
                    case PostModelType.CommentSection:
                    case PostModelType.AddCommentSection:
                        offset = NativeFeedAdapter.ListDiffer.Count == 0 ? "0" : NativeFeedAdapter.ListDiffer.LastOrDefault()?.PostData?.PostId ?? "0";
                        Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] Offset determined from last item: {offset}");
                        break;
                    default:
                        offset = item?.PostData?.PostId ?? "0";
                        Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] Offset determined from item: {offset}");
                        break;
                }

                Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] Final Offset: {offset}");

                int apiStatus;
                dynamic respond;

                switch (NativeFeedAdapter.NativePostType)
                {
                    case NativeFeedType.Global:
                        var adId = NativeFeedAdapter.ListDiffer.LastOrDefault(a => a.TypeView == PostModelType.AdsPost && a.PostData.PostType == "ad")?.PostData?.Id ?? "";
                        Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] Fetching posts with AdId: {adId}");
                        (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost(AppSettings.PostApiLimitOnScroll, offset, "get_news_feed", NativeFeedAdapter.IdParameter, "", WRecyclerView.GetFilter(), adId, WRecyclerView.GetPostType());
                        break;
                    default:
                        Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] Unsupported NativePostType.");
                        return;
                }

                Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] API Response - Status: {apiStatus}, Response: {respond}");

                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                {
                    Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] API call failed or returned no data.");
                    Methods.DisplayReportResult(ActivityContext, respond);
                }
                else
                {
                    PostCacheList ??= new List<PostDataObject>();

                    var countList = PostCacheList?.Count ?? 0;
                    switch (result.Data?.Count)
                    {
                        case > 0:
                            {
                                result.Data.RemoveAll(a => a.Publisher == null && a.UserData == null);

                                switch (countList)
                                {
                                    case > 0:
                                        {
                                            foreach (var post in from post in result.Data let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                                            {
                                                PostCacheList.Add(post);
                                            }

                                            break;
                                        }
                                    default:
                                        PostCacheList = new List<PostDataObject>(result.Data);
                                        break;
                                }

                                Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] PostCacheList updated. Current count: {PostCacheList.Count}");
                            }
                            break;
                        default:
                            Console.WriteLine("[FetchLoadMoreNewsFeedApiPosts] No new posts returned from API.");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[FetchLoadMoreNewsFeedApiPosts] Exception: {e.Message}");
                Methods.DisplayReportResultTrack(e);
            }
        }

        private bool LoadBottomDataApi(List<PostDataObject> list)
        {
            try
            {
                var countList = NativeFeedAdapter.ItemCount;
                switch (list?.Count)
                {
                    case > 0:
                        {
                            bool add = false;
                            foreach (var post in from post in list let check = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a?.PostData?.PostId == post.PostId && a?.TypeView == PostFunctions.GetAdapterType(post)) where check == null select post)
                            {
                                add = true;
                                var combiner = new FeedCombiner(null, NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);

                                switch (NativeFeedAdapter.NativePostType)
                                {
                                    case NativeFeedType.Global:
                                        {
                                            var check1 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedGroupsBox);
                                            switch (check1)
                                            {
                                                case null when AppSettings.ShowSuggestedGroup && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedGroupCount == 0 && ListUtils.SuggestedGroupList.Count > 0:
                                                    combiner.AddSuggestedBoxPostView(PostModelType.SuggestedGroupsBox);
                                                    break;
                                            }

                                            var check2 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedUsersBox);
                                            switch (check2)
                                            {
                                                case null when AppSettings.ShowSuggestedUser && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedUserCount == 0 && ListUtils.SuggestedUserList.Count > 0:
                                                    combiner.AddSuggestedBoxPostView(PostModelType.SuggestedUsersBox);
                                                    break;
                                            }

                                            var check3 = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.SuggestedPagesBox);
                                            switch (check3)
                                            {
                                                case null when AppSettings.ShowSuggestedPage && NativeFeedAdapter.ListDiffer.Count > 0 && NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowSuggestedPageCount == 0 && ListUtils.SuggestedPageList.Count > 0:
                                                    combiner.AddSuggestedBoxPostView(PostModelType.SuggestedPagesBox);
                                                    break;
                                            }

                                            break;
                                        }
                                }

                                switch (NativeFeedAdapter.ListDiffer.Count % (AppSettings.ShowAdNativeCount * 10))
                                {
                                    case 0 when NativeFeedAdapter.ListDiffer.Count > 0 && AppSettings.ShowAdMobNativePost:
                                        switch (LastAdsType)
                                        {
                                            case PostModelType.AdMob1:
                                                LastAdsType = PostModelType.AdMob2;
                                                combiner.AddAdsPostView(PostModelType.AdMob1);
                                                break;
                                            case PostModelType.AdMob2:
                                                LastAdsType = PostModelType.AdMob3;
                                                combiner.AddAdsPostView(PostModelType.AdMob2);
                                                break;
                                            case PostModelType.AdMob3:
                                                LastAdsType = PostModelType.AdMob1;
                                                combiner.AddAdsPostView(PostModelType.AdMob3);
                                                break;
                                        }

                                        break;
                                }

                                var combine = new FeedCombiner(RegexFilterText(post), NativeFeedAdapter.ListDiffer, ActivityContext, NativeFeedAdapter.NativePostType);
                                switch (post.PostType)
                                {
                                    case "ad" when AppSettings.ShowAdvertise:
                                        combine.AddAdsPost();
                                        break;
                                    default:
                                        {
                                            bool isPromoted = post.IsPostBoosted == "1" || post.SharedInfo.SharedInfoClass != null && post.SharedInfo.SharedInfoClass?.IsPostBoosted == "1";
                                            if (isPromoted)
                                            {
                                                if (NativeFeedAdapter.ListDiffer.Count == 0)
                                                    combine.CombineDefaultPostSections();
                                                else
                                                {
                                                    var p = NativeFeedAdapter.ListDiffer?.FirstOrDefault(a => a.TypeView == PostModelType.PromotePost);
                                                    if (p != null)
                                                        combine.CombineDefaultPostSections();
                                                    else
                                                        combine.CombineDefaultPostSections("Top");
                                                }
                                            }
                                            else
                                            {
                                                combine.CombineDefaultPostSections();
                                            }

                                            break;
                                        }
                                }

                                switch (NativeFeedAdapter.ListDiffer.Count % AppSettings.ShowAdNativeCount)
                                {
                                    case 0 when NativeFeedAdapter.ListDiffer.Count > 0 && AppSettings.ShowFbNativeAds:
                                        combiner.AddAdsPostView(PostModelType.FbAdNative);
                                        break;
                                }
                            }

                            switch (add)
                            {
                                case true:
                                    ActivityContext?.RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            var d = new Runnable(() => { NativeFeedAdapter.NotifyItemRangeInserted(countList, NativeFeedAdapter.ListDiffer.Count - countList); }); d.Run();
                                            GC.Collect();
                                        }
                                        catch (Exception e)
                                        {
                                            Methods.DisplayReportResultTrack(e);
                                        }
                                    });
                                    break;
                            }

                            PostCacheList.RemoveRange(0, list.Count - 1);
                            ActivityContext?.RunOnUiThread(ShowEmptyPage);

                            return add;
                        }
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return false;
            }
        }

        private void ShowEmptyPage()
        {
            try
            {
                NativeFeedAdapter.SetLoaded();
                var viewProgress = NativeFeedAdapter.ListDiffer.FirstOrDefault(anjo => anjo.TypeView == PostModelType.ViewProgress);
                if (viewProgress != null)
                    WRecyclerView.RemoveByRowIndex(viewProgress);

                var emptyStateCheck = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.PostData != null && a.TypeView != PostModelType.AddPostBox /*&& a.TypeView != PostModelType.SearchForPosts*/);
                if (emptyStateCheck != null)
                {
                    var emptyStateChecker = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                    if (emptyStateChecker != null && NativeFeedAdapter.ListDiffer.Count > 1)
                        WRecyclerView.RemoveByRowIndex(emptyStateChecker);
                }
                else
                {
                    var emptyStateChecker = NativeFeedAdapter.ListDiffer.FirstOrDefault(a => a.TypeView == PostModelType.EmptyState);
                    if (emptyStateChecker == null)
                    {
                        var data = new AdapterModelsClass
                        {
                            TypeView = PostModelType.EmptyState,
                            Id = 744747447,
                        };
                        NativeFeedAdapter.ListDiffer.Add(data);
                        NativeFeedAdapter.NotifyDataSetChanged();
                    }
                }

                WRecyclerView.MainScrollEvent.IsLoading = false;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        #endregion

        private void GetAllPostLive(List<PostDataObject> list)
        {
            try
            {
                // Process posts without any RandomId-specific conditions
                foreach (var post in list)
                {
                    // Add your processing logic here if needed
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static async Task GetAllPostVideo()
        {
            try
            {
                if (!Methods.CheckConnectivity())
                    return;

                var (apiStatus, respond) = await RequestsAsync.Posts.GetGlobalPost("10", "0", "get_news_feed", "", "", "0", "0", "video");
                if (apiStatus != 200 || respond is not PostObject result || result.Data == null)
                {
                    // Handle error
                }
                else
                {
                    // Process posts without any RandomId-specific conditions
                    foreach (var post in result.Data)
                    {
                        // Add your processing logic here if needed
                    }
                }
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public static PostDataObject RegexFilterText(PostDataObject item)
        {
            try
            {
                Dictionary<string, string> dataUser = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(item.PostText))
                    return item;

                if (item.PostText.Contains("data-id="))
                {
                    try
                    {
                        //string pattern = @"(data-id=[""'](.*?)[""']|href=[""'](.*?)[""']|'>(.*?)a>)";

                        string pattern = @"(data-id=[""'](.*?)[""']|href=[""'](.*?)[""'])";
                        var aa = Regex.Matches(item.PostText, pattern);
                        switch (aa?.Count)
                        {
                            case > 0:
                                {
                                    for (int i = 0; i < aa.Count; i++)
                                    {
                                        string userid = "";
                                        if (aa.Count > i)
                                            userid = aa[i]?.Value?.Replace("data-id=", "").Replace('"', ' ').Replace(" ", "");

                                        string username = "";
                                        if (aa.Count > i + 1)
                                            username = aa[i + 1]?.Value?.Replace("href=", "").Replace('"', ' ').Replace(" ", "").Replace(InitializeWoWonder.WebsiteUrl, "").Replace("\n", "");

                                        if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(username))
                                            continue;

                                        var data = dataUser.FirstOrDefault(a => a.Key?.ToString() == userid && a.Value?.ToString() == username);
                                        if (data.Key != null)
                                            continue;

                                        i++;

                                        switch (string.IsNullOrWhiteSpace(userid))
                                        {
                                            case false when !string.IsNullOrWhiteSpace(username) && !dataUser.ContainsKey(userid):
                                                dataUser.Add(userid, username);
                                                break;
                                        }
                                    }

                                    item.RegexFilterList = new Dictionary<string, string>(dataUser);
                                    return item;
                                }
                            default:
                                return item;
                        }
                    }
                    catch (Exception e)
                    {
                        Methods.DisplayReportResultTrack(e);
                    }
                }

                return item;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
                return item;
            }
        }
    }
}