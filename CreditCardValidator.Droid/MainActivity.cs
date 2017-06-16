using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CreditCardValidation.Common;
using System.Threading;

/*Needed references (need to be integrate in References*/
using Com.Greenspector.Probe.Android.Interfaces;
using Java.Interop;

namespace CreditCardValidator.Droid
{

	[Activity(Label = "@string/activity_main", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : Activity
	{

        /*Greenspector Backdoor method to call on Xamarin.UITests*/
        Api gspt;

        [Export("SetUpMeasureBackdoor")]
        public void SetUpMeasureBackdoor(String token)
        {
            gspt = new Api();
            // CHANGE PARAMETERS
            ThreadPool.QueueUserWorkItem(o => gspt.SetUpMeasure(ApplicationContext, new String[] { "com.xamarin.example.creditcardvalidator" }, null, "XamarinCreditCard", "1", "https://app.greenspector.com/api", token, 180, 200, true));
        }

        [Export("StartMeasureBackdoor")]
        public void StartMeasureBackdoor()
        {
            gspt.StartMeasure();
        }

        [Export("StopMeasureBackdoor")]
        public void StopMeasureBackdoor(String TestName)
        {
            ThreadPool.QueueUserWorkItem(o => gspt.StopMeasure(TestName));
        }
        /*End of Greenspector Backdoor*/


        static readonly ICreditCardValidator _validator = new SimpleCreditCardValidator();

		EditText _creditCardTextField;
		TextView _errorMessagesTextField;
		Button _validateButton;

        protected override void OnCreate(Bundle bundle)
		{
                      

            base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);
					
			_errorMessagesTextField = FindViewById<TextView>(Resource.Id.errorMessagesText);
			_creditCardTextField = FindViewById<EditText>(Resource.Id.creditCardNumberText);
			_validateButton = FindViewById<Button>(Resource.Id.validateButton);
			_validateButton.Click += (sender, e) =>{
				_errorMessagesTextField.Text = String.Empty;
				string errMessage;

				if (_validator.IsCCValid(_creditCardTextField.Text, out errMessage))
				{
					var i = new Intent(this, typeof(CreditCardValidationSuccess));
					StartActivity(i);
                }
				else
				{
					RunOnUiThread(() => { _errorMessagesTextField.Text = errMessage; });
                }
			};
		}
	}
}


