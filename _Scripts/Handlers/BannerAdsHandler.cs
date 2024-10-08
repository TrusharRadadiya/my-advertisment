using System;
using UnityEngine;

namespace MyAdvertisement
{
    public static class BannerAdsHandler
    {
        private static BannerAdsController _controller;
        private static bool Initialized => _controller is not null;
        
        public static bool Showing { get; private set; }

        private static bool BannerRemoved
        {
            get => PlayerPrefs.GetInt(nameof(BannerRemoved), 0) is 1;
            set => PlayerPrefs.SetInt(nameof(BannerRemoved), value ? 1 : 0);
        }
        
        public static event Action<AdsProvider> OnAdAvailable;
        
        public static void Initialize(BannerAdsController controller)
        {
            _controller = controller;
            _controller.OnAdAvailable += provider =>
            {
                if (BannerRemoved)
                {
                    _controller.DestroyBannerAd();
                    return;
                }
                
                OnAdAvailable?.Invoke(provider);
            };
        }

        public static void LoadAd()
        {
            if (BannerRemoved || !Initialized) return;
            _controller.LoadBannerAd();
        }

        public static void DestroyAd()
        {
            if (!Initialized) return;
            _controller.DestroyBannerAd();
            Showing = false;
        }

        public static void ShowAd()
        {
            if (BannerRemoved || !Initialized) return;
            _controller.ShowBannerAd();
            Showing = true;
        }

        public static void HideAd()
        {
            if (BannerRemoved || !Initialized) return;
            _controller.HideBannerAd();
            Showing = false;
        }

        public static void RemoveAd()
        {
            BannerRemoved = true;
            DestroyAd();
        }

        public static void RestoreAd() => BannerRemoved = false;
    }
}