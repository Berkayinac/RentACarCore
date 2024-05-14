using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Authorization;

// içerisinde rolleri tutan bir interface
public interface ISecuredRequest
{
    public string[] Roles { get; }
}

// behavior'ın içerisinde -> Bir command veya query request işleminde bulunmuşsa ve request edilen yer bu interface'i implement ediyorsa rol bazlı yapıya gir.