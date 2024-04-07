// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace Tonisoft.AspExtensions.Response;

public class ApiResponse : JsonResult
{
    public ApiResponse(int code, object? value) : base(value)
    {
        StatusCode = code;
    }

    public static ApiResponse Success(object? value = null)
    {
        return new ApiResponse(200, value);
    }
}