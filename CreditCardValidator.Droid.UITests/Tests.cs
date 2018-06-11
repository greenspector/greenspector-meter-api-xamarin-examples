using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;


namespace CreditCardValidator.Droid.UITests
{
	[TestFixture]
	public class Tests
	{
		AndroidApp app;

        [SetUp]
		public void BeforeEachTest()
		{
			app = ConfigureApp.Android.InstalledApp("com.xamarin.example.creditcardvalidator").StartApp();
            //Greenspector Measure Set Up
            app.Invoke("SetUpMeasureBackdoor", "[Put your Greenspector TOKEN]");
        }

        [Test]
        public void CreditCardNumber_TooShort_DisplayErrorMessage()
        {
            //Start of Greenspector measure
            app.Invoke("StartMeasureBackdoor");

            app.WaitForElement(c => c.Marked("action_bar_title").Text("Enter Credit Card Number"));
            app.EnterText(c => c.Marked("creditCardNumberText"), new string('9', 15));
            app.Tap(c => c.Marked("validateButton"));

            app.WaitForElement(c => c.Marked("errorMessagesText").Text("Credit card number is too short."));

            //Stop of Greenspector measure
            app.Invoke("StopMeasureBackdoor","TestBackdoor");
        }

    }
}
