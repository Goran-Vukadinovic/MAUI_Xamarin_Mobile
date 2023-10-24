
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Immowert4You.Presentation;
using Java.Net;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Presentation.Droid
{
    [Activity(Label = "Immowert 4You", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        protected override void OnStart()
        {
            base.OnStart();
            const int requestLocationId = 1;

            string[] notiPermission =
            {
                Manifest.Permission.PostNotifications
            };

            if ((int)Build.VERSION.SdkInt < 33) 
                return;

            if (this.CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                this.RequestPermissions(notiPermission, requestLocationId);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if(Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Tiramisu)
            {
                var grants = new Permission[permissions.Length];
                for (int i = 0; i < grantResults.Length; i++)
                    grants[i] = Permission.Granted;

                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grants);
            }
            else
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }
    }
}