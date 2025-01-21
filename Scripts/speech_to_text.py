from google.cloud import speech
import io

def convert_audio_to_text(audio_file_path):
    client = speech.SpeechClient()

    # Load the audio file
    with io.open(audio_file_path, "rb") as audio_file:
        content = audio_file.read()

    audio = speech.RecognitionAudio(content=content)
    config = speech.RecognitionConfig(
        encoding=speech.RecognitionConfig.AudioEncoding.LINEAR16,
        sample_rate_hertz=16000,
        language_code="en-US",
    )

    # Transcribe the audio
    response = client.recognize(config=config, audio=audio)

    # Combine and return transcriptions
    transcription = " ".join(result.alternatives[0].transcript for result in response.results)
    return transcription
