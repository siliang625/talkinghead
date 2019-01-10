﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

namespace Microsoft.Teams.Samples.HelloWorld.Web.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        static double score = 0.0;
        static int count = 0;
        Random random = new Random();

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            using (var connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
            {
                if (activity.IsComposeExtensionQuery())
                {
                    var response = MessageExtension.HandleMessageExtensionQuery(connector, activity);
                    return response != null
                        ? Request.CreateResponse<ComposeExtensionResponse>(response)
                        : new HttpResponseMessage(HttpStatusCode.OK);
                }
                else if (activity.GetTextWithoutMentions().ToLower().Trim() == "how happy am i?")
                {
                    await EchoBot.EchoMessage(connector, activity, score/count);
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
                else
                {
                    score += TempSentiment(activity.GetTextWithoutMentions());
                    count++;
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
        }

        public double TempSentiment(string message)
        {
            return random.NextDouble();
        }
    }
}
