var speechSdk = window.SpeechSDK;
var recognizer;
var keywordRecognized = false;

function startRecognition(subscriptionKey, serviceRegion) {
    var speechConfig = speechSdk.SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);
    var audioConfig = speechSdk.AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new speechSdk.SpeechRecognizer(speechConfig, audioConfig);
    
    recognizer.recognized = (s, e) => {
        if (e.result.reason == speechSdk.ResultReason.RecognizedSpeech) {
            //console.log(`RECOGNIZED: Text=${e.result.text}`);
            //console.log(e.result.text.replace(/[,. ]/g, '').replace('i', 'y').toLowerCase());
            const formattedPhrase = e.result.text.replace(/[,. ]/g, '').replace('i', 'y').toLowerCase();
            if (!keywordRecognized && formattedPhrase === "okhandycook") {
                console.log("keyword", formattedPhrase);
                keywordRecognized = true;
                DotNet.invokeMethodAsync('HandyCook.Application', 'OnKeywordRecognized', null, null);
            }
            else if (keywordRecognized) {
                console.log("Phrase recognizes", e.result.text);
                keywordRecognized = false;
                DotNet.invokeMethodAsync('HandyCook.Application', 'OnSpeechRecognized', null, e.result.text);
            }
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
