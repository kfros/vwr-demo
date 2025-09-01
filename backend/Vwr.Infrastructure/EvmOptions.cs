using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Infrastructure;

public class EvmOptions
{
    public string RpcUrl { get; set; } = "http://127.0.0.1:8545";
    public string PrivateKey { get; set; } = string.Empty; // from local Anvil account
    public string ContractAddress { get; set; } = string.Empty;
}