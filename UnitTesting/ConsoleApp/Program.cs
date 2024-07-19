// See https://aka.ms/new-console-template for more information

using ConsoleApp.Units;
using AlgorithmPropertyHash = ConsoleApp.Units.AlgorithmPropertyHash;
using PropertyHash = ConsoleApp.Units.PropertyHash;

var hasher = new PropertyHash();
var hasherAlgorithm = new AlgorithmPropertyHash("SHA256");
var item = new Cache.Item("Url", "Content", DateTime.Now);

var hash = hasher.Hash(item, i => i.Url, i => i.Content);
var hashAlgorithm = hasherAlgorithm.Hash(item, i => i.Url, i => i.Content);
Console.WriteLine(hash);
Console.WriteLine(hashAlgorithm);
