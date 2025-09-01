using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Domain;

public record SiweRequest(string Message, string Signature, string Address);