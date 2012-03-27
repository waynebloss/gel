// COPYRIGHT AND (MIT) LICENSE APPLY. SEE FILE: ../lic/node.txt

var binding = process.binding('os');

exports.hostname = binding.getHostname;
exports.loadavg = binding.getLoadAvg;
exports.uptime = binding.getUptime;
exports.freemem = binding.getFreeMem;
exports.totalmem = binding.getTotalMem;
exports.cpus = binding.getCPUs;
exports.type = binding.getOSType;
exports.release = binding.getOSRelease;
exports.networkInterfaces = binding.getInterfaceAddresses;
exports.arch = function() {
  return process.arch;
};
exports.platform = function() {
  return process.platform;
};

exports.getNetworkInterfaces = function() {
  return exports.networkInterfaces();
};
module.deprecate('getNetworkInterfaces',
                 'It is now called `os.networkInterfaces`.');
