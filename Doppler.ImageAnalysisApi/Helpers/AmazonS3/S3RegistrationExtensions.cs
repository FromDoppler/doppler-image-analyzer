﻿using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Doppler.ImageAnalysisApi.Configurations.Amazon;
using Doppler.ImageAnalysisApi.Helpers.AmazonS3.Interfaces;

namespace Doppler.ImageAnalysisApi.Helpers.AmazonS3;

public static class S3RegistrationExtensions
{
    public static IServiceCollection AddAmazonS3(this IServiceCollection services, AmazonConfiguration config)
    {
        services.AddTransient<IS3Client, S3Client>();
        services.AddTransient<IAmazonS3, AmazonS3Client>((_) =>
        {
            return new AmazonS3Client(new BasicAWSCredentials(config.AwsAccessKey, config.AwsAccessSecret), RegionEndpoint.GetBySystemName(config.Region));
        });

        return services;
    }
}