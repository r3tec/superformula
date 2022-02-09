using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public class CustomErrorResponse
    {
        [JsonProperty("errors")]
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        [JsonProperty("status")]
        public int Status { get; set; } = StatusCodes.Status400BadRequest;

        public CustomErrorResponse(ActionContext context)
        {
            ConstructErrorMessages(context);
        }

        private void ConstructErrorMessages(ActionContext context)
        {
            Errors = context.ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors?.Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? "Invalid input." : e.ErrorMessage)?.ToArray() ?? Array.Empty<string>()
                );
        }

    }
}
