

# 🎻ViolinMaestro: Digital Music Analysis Parallelisation Project

Welcome to Violin Maestro! This project was originally developed by students at Queensland University of Technology (QUT) as a sequential application to assist violin players, particularly beginners, by providing real-time feedback on their musical performances. The aim is to help musicians further develop and refine their violin skills. The application analyses audio data and compares it against a digital version of the sheet music, offering detailed feedback on aspects such as pitch accuracy and note duration.

**In this project, I have parallelised the application to enhance performance and responsiveness.**


## Features

-   **Audio Input and Preprocessing:** Reads WAV format audio files and converts them into digital samples.
    
-   **Fourier Transform Analysis:** Utilises Short-Term Fourier Transform (STFT) to analyse audio data in the frequency domain.
    
-   **Onset Detection:** Detects the start and end times of notes in the audio signal.
    
-   **Sheet Music Parsing:** Reads XML format sheet music and extracts note information.
    
-   **Comparison and Feedback Generation:** Compares detected notes against the sheet music and provides feedback on pitch accuracy and timing.
    


## Usage

1.  Run the application and select the audio file (WAV format) and sheet music file (XML format) you wish to analyse (test and example files are included)
    
2.  The application will process the audio and provide visual feedback on pitch accuracy, timing errors, and comments for improvement.
    

## Performance Improvements

The application was optimised for parallel execution to enhance performance and responsiveness. Key improvements include:

-   Parallelising the STFT and onset detection functions using the .NET Task Parallel Library (TPL).
    
-   Localising variables within parallel loops to avoid race conditions.
    
-   Utilising thread-safe collections for result storage.
    

## Tools and Techniques Used

-   **Task Parallel Library (TPL):** Used for efficient loop parallelisation.
    
-   **Visual Studio Performance Profiler:** Utilised to identify computational bottlenecks and guide the parallelisation strategy.
    
-   **Thread-Safe Data Structures:** Ensured thread safety and data consistency during parallel execution.
    

## Reflections and Lessons Learned

During the development of Violin Maestro, several key lessons were learned:

-   Not all parts of an application benefit from parallelisation due to overhead costs and data dependencies.
    
-   Effective parallelisation requires careful analysis of the codebase and a deep understanding of the underlying algorithms.
    
-   Ensuring thread safety and data consistency is crucial for accurate results in parallel applications.
    
