# Facial Recognition In Xamarin
Created by: Michael Watson

This application is a Xamarin.Forms application containing an iOS and Android application. The mobile application communicates with an Azure Mobile App as the backend.

The Azure backend processes the image using Cognitive Services Computer Vision API to identify users faces. The backend also creates a database to save additional information to the identified user by their Cognitive Services Id.

## The Mobile App

The mobile application utilizes the [Xamarin Media Plugin]() to take images. The images are then resized to 400x400 pixels using the DependencyService with native implementations on [iOS](https://github.com/michael-watson/Facial-Recognition/blob/master/FacialRecognition.Mobile/IdentifyMobi/iOS/Resize_iOS.cs#L14) and [Android](https://github.com/michael-watson/Facial-Recognition/blob/master/FacialRecognition.Mobile/IdentifyMobi/Droid/Resizer_Droid.cs#L23). This resizing is called in our [IdentityService](https://github.com/michael-watson/Facial-Recognition/blob/master/FacialRecognition.Mobile/IdentifyMobi/IdentifyMobi/Services/IdentityService.cs#L35).

After the image has been resized in the native implementations, the associated `byte[]` is returned. This is converted to a Base64 string and uploaded to our Azure Mobile Backend to be processed. Our backend will respond with the identity and any associated information, or with a simple error string as the response.

## The Azure Mobile App

The Azure Mobile App is our backend that processes the images and returns information. The benefit of moving our Identity service to the cloud is enhanced security and more power. The Azure Mobile App will be able to process information quickly and correlate the identity from Cognitive Services with other services we might have.

If we did our Identity Service from just the Xamarin mobile app, we would potentially have to send multiple HttpClient web requests to multiple services creating a slower user experience. Our backend is much more powerful than our phones.

We also get security in the sense that our [Identity Service Client Secret](https://github.com/michael-watson/Facial-Recognition/blob/master/FacialRecognition.Backend/FacialRecognition.Backend/FaceAnalyzer.cs#L16) is actually a ClientSecret because it only lives in the cloud!

The backend uses an [ApiController](https://github.com/michael-watson/Facial-Recognition/blob/master/FacialRecognition.Backend/FacialRecognition.Backend/Controllers/IdentityController.cs) to correlate the IdentityData with our [Database reference](https://github.com/michael-watson/Facial-Recognition/blob/master/FacialRecognition.Backend/FacialRecognition.Backend/Models/MobileServiceContext.cs#L29).