/// <reference path="ref/process.js"/>

process.binding.set('os', (function() {
	var api = process.api('os');
	return {
		getHostname: function() { return api.getHostname(); },
		getLoadAvg: function() { return eval(api.getLoadAvg()); },
		getUptime: function() { return api.getUptime(); },
		getFreeMem: function() { return api.getFreeMem(); },
		getTotalMem: function() { return api.getTotalMem(); },
		getCPUs: function() { return api.getCPUs(); },
		getOSType: function() { return api.getOSType(); },
		getOSRelease: function() { return api.getOSRelease(); },
		getInterfaceAddresses: function() { return api.getInterfaceAddresses(); }
	};
})());
