# doppler-image-analysis-api

## _Tool for image content analysis_ 

This is an application to identify labels detected on Images, and an API to process HTML files and Images list.

## Features

This API contains the Next Enpoints:

- **AnalyzeHtml**. This endpoint will get all Images detected inside HTML and will detect labels per Image
- **AnalyzeImageList**. This Endpoint will process all Images on the list and will detect labels per Image

## Tech

doppler-image-analysis-api uses a number of technologies to work correctly:

- [Aws Rekognition] - AI to detect content inside the images
- [.net 7] - Used to build the API and Unit testing
- [Mediatr] - Supports request/response, commands, queries, notifications, and events, synchronous and async with intelligent dispatching via C# generic variance.

## How to Run

**Important Note:**_To be able to run this application is important to have an Aws account._

We need to have the next features on AWS

- S3 bucket to upload the Images to process. For more information go to [How to create an S3 bucket](https://docs.aws.amazon.com/AmazonS3/latest/userguide/create-bucket-overview.html)
- For Custom Labels, Create a Custom labels project in Rekognition Service. For more information go to [Getting started with Amazon Rekognition Custom Labels](https://docs.aws.amazon.com/rekognition/latest/customlabels-dg/getting-started.html)

   [Aws Rekognition]: <https://aws.amazon.com/rekognition>
   [.net 7]: <https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-7>
   [Mediatr]: <https://github.com/jbogard/MediatR>