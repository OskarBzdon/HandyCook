var speechSdk = window.SpeechSDK;
var recognizer;

function startRecognition(subscriptionKey, serviceRegion) {
    var speechConfig = speechSdk.SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);
    var audioConfig = speechSdk.AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new speechSdk.SpeechRecognizer(speechConfig, audioConfig);
    
    recognizer.recognized = (s, e) => {
        if (e.result.reason == speechSdk.ResultReason.RecognizedSpeech) {
            console.log(`RECOGNIZED: Text=${e.result.text}`);
        }
    };

    recognizer.startContinuousRecognitionAsync(
        () => console.log("Recognition started"),
        err => console.error(`ERROR: ${err}`)
    );
}

function stopRecognition() {
    recognizer.stopContinuousRecognitionAsync(
        () => console.log("Recognition stopped"),
        err => console.error(`ERROR: ${err}`)
    );
}
