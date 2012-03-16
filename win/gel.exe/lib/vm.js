// DO NOT REDISTRIBUTE WITHOUT COPYRIGHT LICENSE FILE: ../lic/node.txt

var binding = process.binding('evals');

exports.Script = binding.NodeScript;
exports.createScript = function(code, ctx, name) {
  return new exports.Script(code, ctx, name);
};

exports.createContext = binding.NodeScript.createContext;
exports.runInContext = binding.NodeScript.runInContext;
exports.runInThisContext = binding.NodeScript.runInThisContext;
exports.runInNewContext = binding.NodeScript.runInNewContext;
