﻿using Alice.Classes;
using EmotionLib;
using EmotionLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alice.Models.Categories
{
    public delegate GlobalActionResponse GlobalTemplateAction(Match match);
    public delegate string Response(Match match, GlobalActionResponse globalActionResponse);

    public class Template : TemplateBase
    {
        private List<Response> _responses = new List<Response>();

        public Template(GlobalTemplateAction globalAction) : base(globalAction)
        {
        }

        public Template() : base() { }

        public void AddResponse(Response response)
        {
            _responses.Add(response);
        }

        public override string GetResponse(Match match)
        {
            GlobalActionResponse globalActionResponse = _globalAction?.Invoke(match);

            Response response = ResponseChooser.Choose(_responses);

            return response.Invoke(match, globalActionResponse);
        }

    }

}