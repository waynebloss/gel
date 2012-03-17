using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace Gel
{
	[ComVisible(true)]
	public sealed class TimersApi
	{
		internal TimersApi() { }

		Dictionary<int, Timer> _winTimer = new Dictionary<int, Timer>();

		Type _timeoutHandlerType;
		object _timeoutHandler;
		public object timeoutHandler
		{
			get { return _timeoutHandler; }
			set
			{
				_timeoutHandler = value;
				if (value != null)
					_timeoutHandlerType = value.GetType();
				else
					_timeoutHandlerType = null;
			}
		}

		public void close(int id)
		{
			Timer winTimer;
			if (!_winTimer.TryGetValue(id, out winTimer))
				return;
			_winTimer.Remove(id);
			winTimer.Dispose();
		}

		public void start(int id, int timeMs, int repeatTimeMs)
		{
			var winTimer = new Timer();
			winTimer.Interval = timeMs;
			winTimer.Tick += (s, e) =>
			{
				// Callback
				if (_timeoutHandlerType != null)
				{
					// Invoke the Jscript (COM) object's default member (specified by the blank string).
					// For a Jscript function, the default member is the function itself.
					//
					// This is what allows us to do:
					//		process.api('timers').timeoutHandler = function(id) {
					//			...
					//		};
					//
					_timeoutHandlerType.InvokeMember("", System.Reflection.BindingFlags.InvokeMethod, null,
						_timeoutHandler, new object[] { id });
				}

				// Repeat or Stop
				if (repeatTimeMs > 0)
				{
					winTimer.Interval = repeatTimeMs;
				}
				else
				{
					winTimer.Dispose();
					_winTimer.Remove(id);
				}
			};
			_winTimer.Add(id, winTimer);
			winTimer.Start();
		}

		public void stop(int id)
		{
			// The corresponding Node.js binding, timer_wrap method, is never called
			// from the Node.js lib timers.js.
			throw new NotImplementedException("TimersApi.stop() not implemented.");
			// CONSIDER: (_winTimer must be changed to a Dictionary<object, Timer>)
			//Timer winTimer;
			//if (!_winTimer.TryGetValue(timer, out winTimer))
			//    return;
			//winTimer.Dispose();
			//_winTimer.Remove(timer);
		}

		public int getRepeat(int id)
		{
			// The corresponding Node.js binding, timer_wrap method, is never called
			// from the Node.js lib timers.js.
			throw new NotImplementedException("TimersApi.getRepeat() not implemented.");
		}

		public void setRepeat(int id, int value)
		{
			// The corresponding Node.js binding, timer_wrap method, is never called
			// from the Node.js lib timers.js.
			throw new NotImplementedException("TimersApi.setRepeat() not implemented.");
		}

		public void again(int id)
		{
			// The corresponding Node.js binding, timer_wrap method, is never called
			// from the Node.js lib timers.js.
			throw new NotImplementedException("TimersApi.again() not implemented.");
		}
	}
}
