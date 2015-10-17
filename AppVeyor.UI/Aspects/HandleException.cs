using System;
using System.Collections.Generic;
using AppVeyor.Common;
using AppVeyor.UI.ViewModel;
using PostSharp.Aspects;

namespace AppVeyor.UI.Aspects
{
    [Serializable]
    public sealed class HandleException : OnMethodBoundaryAspect
    {
        public HandleException(bool isAsync = true)
        {
            ApplyToStateMachine = isAsync;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            var properties = new Dictionary<string,string>()
            {
                {"Method Name", args.Method.Name}
            };
            Telemetry.Instance.TrackException(args.Exception, properties);
            AppVeyorWindowViewModel.Instance.AddAlert(args.Exception, args.Exception.Message);
            base.OnException(args);
        }
    }
}
