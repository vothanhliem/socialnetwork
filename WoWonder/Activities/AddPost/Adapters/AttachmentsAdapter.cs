﻿using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Request;
using Bumptech.Glide.Request.Target;
using Bumptech.Glide.Request.Transition;
using Java.IO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WoWonder.Helpers.CacheLoaders;
using WoWonder.Helpers.Utils;
using WoWonderClient.Classes.Posts;
using Object = Java.Lang.Object;

namespace WoWonder.Activities.AddPost.Adapters
{
    public class AttachmentsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<AttachmentsAdapterClickEventArgs> ItemEditClick;
        public event EventHandler<AttachmentsAdapterClickEventArgs> DeleteItemClick;
        public event EventHandler<AttachmentsAdapterClickEventArgs> ItemClick;
        public event EventHandler<AttachmentsAdapterClickEventArgs> ItemLongClick;

        private readonly Activity ActivityContext;
        public ObservableCollection<Attachments> AttachmentList = new ObservableCollection<Attachments>();
        public AttachmentsAdapter(Activity context)
        {
            try
            {
                ActivityContext = context;
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        public override int ItemCount => AttachmentList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Attachment_View
                var itemView = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.Style_AttachmentView, parent, false);
                var vh = new AttachmentsAdapterViewHolder(itemView, DeleteClick, Click, LongClick, OnEditClick);
                return vh;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return null!;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                switch (viewHolder)
                {
                    case AttachmentsAdapterViewHolder holder:
                        {
                            var item = AttachmentList[position];
                            if (item != null)
                            {
                                if (item.FileSimple.Contains("http") || item.FileSimple.Contains("EmbedVideo_File"))
                                {
                                    GlideImageLoader.LoadImage(ActivityContext, item.FileSimple, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                                }
                                else
                                {
                                    if (item.FileSimple.Contains("http"))
                                    {
                                        GlideImageLoader.LoadImage(ActivityContext, item.FileSimple, holder.Image, ImageStyle.CenterCrop, ImagePlaceholders.Drawable);
                                    }
                                    else if (item.FileSimple == "Image_File")
                                    {
                                        holder.Image.SetImageResource(Resource.Drawable.ic_preview_file);
                                    }
                                    else if (item.FileSimple == "Audio_File")
                                    {
                                        holder.Image.SetImageResource(Resource.Drawable.ic_preview_music);
                                    }
                                    else if (item.FileSimple == "Record_File")
                                    {
                                        holder.Image.SetImageResource(Resource.Drawable.ic_preview_record);
                                    }
                                    else if (item.TypeAttachment == "postVideo" && !string.IsNullOrEmpty(item.Thumb?.FileUrl) && !new File(item.Thumb.FileUrl).Exists())
                                    {
                                        var file2 = new File(item.FileUrl);
                                        //var photoUri = FileProvider.GetUriForFile(ActivityContext, ActivityContext.PackageName + ".fileprovider", file2);

                                        Glide.With(ActivityContext?.BaseContext)
                                            .AsBitmap()
                                            .Apply(GlideImageLoader.GetOptions(ImageStyle.CenterCrop, ImagePlaceholders.Drawable))
                                            .Load(file2) // or URI/path
                                            .Into(new MySimpleTarget(this, holder, position));  //image view to set thumbnail to
                                    }
                                    else
                                    {
                                        Glide.With(ActivityContext?.BaseContext).Load(new File(item.FileUrl)).Apply(GlideImageLoader.GetOptions(ImageStyle.CenterCrop, ImagePlaceholders.Drawable)).Into(holder.Image);
                                    }
                                }

                                holder.IconPlay.Visibility = item.TypeAttachment == "postVideo" ? ViewStates.Visible : ViewStates.Gone;

                                switch (item.TypeAttachment)
                                {
                                    case "postPhotos[]":
                                        holder.OptionLayout.Visibility = ViewStates.Visible;
                                        holder.IconEdit.Visibility = item.FileSimple.Contains("gif") ? ViewStates.Gone : ViewStates.Visible;
                                        break;
                                    case "postVideo":
                                        holder.OptionLayout.Visibility = ViewStates.Visible;
                                        break;
                                    case "postFile":
                                    case "postMusic":
                                        holder.OptionLayout.Visibility = ViewStates.Visible;
                                        holder.IconEdit.Visibility = ViewStates.Gone;
                                        break;
                                    default:
                                        holder.OptionLayout.Visibility = ViewStates.Gone;
                                        break;
                                }
                            }

                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        private class MySimpleTarget : CustomTarget
        {
            private readonly AttachmentsAdapter MAdapter;
            private readonly AttachmentsAdapterViewHolder ViewHolder;
            private readonly int Position;
            public MySimpleTarget(AttachmentsAdapter adapter, AttachmentsAdapterViewHolder viewHolder, int position)
            {
                try
                {
                    MAdapter = adapter;
                    ViewHolder = viewHolder;
                    Position = position;
                }
                catch (Exception e)
                {
                    Methods.DisplayReportResultTrack(e);
                }
            }

            public override void OnResourceReady(Object resource, ITransition transition)
            {
                try
                {
                    switch (MAdapter.AttachmentList?.Count)
                    {
                        case > 0:
                            {
                                var item = MAdapter.AttachmentList[Position];
                                if (item != null && string.IsNullOrEmpty(item.Thumb?.FileUrl))
                                {
                                    var fileName = item.FileUrl.Split('/').Last();
                                    var fileNameWithoutExtension = fileName.Split('.').First();

                                    var pathImage = Methods.Path.FolderDcimImage + "/" + fileNameWithoutExtension + ".png";

                                    var videoImage = Methods.MultiMedia.GetMediaFrom_Gallery(Methods.Path.FolderDcimImage, fileNameWithoutExtension + ".png");
                                    switch (videoImage)
                                    {
                                        case "File Dont Exists":
                                            {
                                                switch (resource)
                                                {
                                                    case Bitmap bitmap:
                                                        {
                                                            Methods.MultiMedia.Export_Bitmap_As_Image(bitmap, fileNameWithoutExtension, Methods.Path.FolderDcimImage);

                                                            File file2 = new File(pathImage);
                                                            var photoUri = FileProvider.GetUriForFile(MAdapter.ActivityContext, MAdapter.ActivityContext.PackageName + ".fileprovider", file2);

                                                            Glide.With(MAdapter.ActivityContext?.BaseContext).Load(photoUri).Apply(new RequestOptions()).Into(ViewHolder.Image);

                                                            item.Thumb = new Attachments.VideoThumb
                                                            {
                                                                FileUrl = photoUri.ToString()
                                                            };
                                                            break;
                                                        }
                                                }

                                                break;
                                            }
                                        default:
                                            {
                                                File file2 = new File(pathImage);
                                                var photoUri = FileProvider.GetUriForFile(MAdapter.ActivityContext, MAdapter.ActivityContext.PackageName + ".fileprovider", file2);

                                                Glide.With(MAdapter.ActivityContext?.BaseContext).Load(photoUri).Apply(new RequestOptions()).Into(ViewHolder.Image);

                                                item.Thumb = new Attachments.VideoThumb
                                                {
                                                    FileUrl = photoUri.ToString()
                                                };
                                                break;
                                            }
                                    }
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

            public override void OnLoadCleared(Drawable p0) { }
        }

        public override void OnViewRecycled(Object holder)
        {
            try
            {
                if (ActivityContext?.IsDestroyed != false)
                    return;

                switch (holder)
                {
                    case AttachmentsAdapterViewHolder viewHolder:
                        Glide.With(ActivityContext?.BaseContext).Clear(viewHolder.Image);
                        break;
                }
                base.OnViewRecycled(holder);
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }

        // Function 
        public void Add(Attachments item)
        {
            try
            {
                AttachmentList.Add(item);
                NotifyItemInserted(AttachmentList.IndexOf(AttachmentList.Last()));
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public void Remove(Attachments item)
        {
            try
            {
                var index = AttachmentList.IndexOf(AttachmentList.FirstOrDefault(a => a.Id == item.Id));
                if (index != -1)
                {
                    AttachmentList.Remove(item);
                    NotifyItemRemoved(index);
                }
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }


        public void RemoveAll()
        {
            try
            {
                AttachmentList.Clear();
                NotifyDataSetChanged();
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
            }
        }

        public Attachments GetItem(int position)
        {
            return AttachmentList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception exception)
            {
                Methods.DisplayReportResultTrack(exception);
                return 0;
            }
        }


        private void OnEditClick(AttachmentsAdapterClickEventArgs args)
        {
            ItemEditClick?.Invoke(this, args);
        }
        private void DeleteClick(AttachmentsAdapterClickEventArgs args)
        {
            DeleteItemClick?.Invoke(this, args);
        }

        private void Click(AttachmentsAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }

        private void LongClick(AttachmentsAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }
    }

    public class AttachmentsAdapterViewHolder : RecyclerView.ViewHolder
    {
        #region Variables Basic

        public View MainView { get; private set; }

        public LinearLayout OptionLayout { get; private set; }
        public ImageView Image { get; private set; }
        public ImageView IconDelete { get; private set; }
        public ImageView IconEdit { get; private set; }
        public ImageView IconPlay { get; private set; }

        #endregion

        public AttachmentsAdapterViewHolder(View itemView, Action<AttachmentsAdapterClickEventArgs> clickDeleteListener, Action<AttachmentsAdapterClickEventArgs> clickListener, Action<AttachmentsAdapterClickEventArgs> longClickListener, Action<AttachmentsAdapterClickEventArgs> editClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                //Get values          
                Image = (ImageView)itemView.FindViewById(Resource.Id.Image);

                OptionLayout = itemView.FindViewById<LinearLayout>(Resource.Id.OptionLayout);
                IconPlay = itemView.FindViewById<ImageView>(Resource.Id.ivPlay);
                IconDelete = itemView.FindViewById<ImageView>(Resource.Id.IconDelete);
                IconEdit = itemView.FindViewById<ImageView>(Resource.Id.IconEdit);

                //Create an Event
                IconDelete.Click += (sender, e) => clickDeleteListener(new AttachmentsAdapterClickEventArgs { View = itemView, Position = BindingAdapterPosition });
                IconEdit.Click += (sender, e) => editClickListener(new AttachmentsAdapterClickEventArgs { Position = BindingAdapterPosition, View = itemView });
                itemView.Click += (sender, e) => clickListener(new AttachmentsAdapterClickEventArgs { View = itemView, Position = BindingAdapterPosition });
                itemView.LongClick += (sender, e) => longClickListener(new AttachmentsAdapterClickEventArgs { View = itemView, Position = BindingAdapterPosition });
            }
            catch (Exception e)
            {
                Methods.DisplayReportResultTrack(e);
            }
        }
    }

    public class AttachmentsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}