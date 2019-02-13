using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotBaseLuis.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotBaseLuis.Services
{
    public class BotLuisServices : IBot
    {
        public Resultados resultados = new Resultados();
        public static readonly string LuisKey = "PepeLuisFutbol";
        private const string WelcomeText = "This bot will introduce you to natural language processing with LUIS. Type an utterance to get started";

        // Services configured from the ".bot" file.
        private readonly BotServices _services;

        // Initializes a new instance of the LuisBot class.
        public BotLuisServices(BotServices services)
        {
            _services = services ?? throw new System.ArgumentNullException(nameof(services));
            if (!_services.LuisServices.ContainsKey(LuisKey))
            {
                throw new System.ArgumentException($"Invalid configuration....");
            }
        }
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))

        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                string equipoLocal = "";
                string equipoVisita = "";
                string fecha = "";

                var respuesta = "";
                // Check LUIS model
                RecognizerResult recognizerResult = await _services.LuisServices[LuisKey].RecognizeAsync(turnContext, cancellationToken);
                //JObject o = JObject.Parse(JObject.Parse(recognizerResult.Entities.ToString())["$instance"].ToString());
                JObject o = JObject.Parse(recognizerResult.Entities.ToString());
                foreach (KeyValuePair<string, JToken> keyValuePair in o)
                {
                    if ("equipos" == keyValuePair.Key)
                    {
                        if (keyValuePair.Value.Count() == 2)
                        {
                            equipoLocal = keyValuePair.Value[0].First.ToString();
                            equipoVisita = keyValuePair.Value[1].First.ToString();
                            

                        }
                    }
                    else if ("datetime" == keyValuePair.Key)
                    {
                        fecha = keyValuePair.Value[0].Last.First.First.ToString();
                    }
                }
                if(equipoVisita != null && equipoLocal != "" && fecha == "")
                {
                    respuesta = resultados.GetResultados(equipoLocal, equipoVisita);
                }
                else if(equipoVisita != null && equipoLocal != "" && fecha != "")
                {
                    respuesta = resultados.GetResultados(equipoLocal, equipoVisita, Int32.Parse(fecha));
                }
                //var asda = JsonConvert.DeserializeObject(recognizerResult.Entities.ToString());
                var topIntent = recognizerResult?.GetTopScoringIntent();
                if (topIntent != null && topIntent.HasValue && topIntent.Value.intent != "None")
                {
                    //await turnContext.SendActivityAsync($"==>LUIS Top Scoring Intent: {topIntent.Value.intent}, Score: {topIntent.Value.score}\n");
                    await turnContext.SendActivityAsync(respuesta);
                }
                else
                {
                    var msg = @"No LUIS intents were found.
                        This sample is about identifying two user intents:
                        'Calendar.Add'
                        'Calendar.Find'
                        Try typing 'Add Event' or 'Show me tomorrow'.";
                    await turnContext.SendActivityAsync(msg);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }
        }
    }
}
