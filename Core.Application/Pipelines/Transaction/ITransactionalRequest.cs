using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Transaction;

// Transactional -> İki işlem yapıldığında ikisinden birinde hata olursa yapılan işlemlerin ikisinide geri almayı sağlar
// Yani herhangi bir hata meydana geldiğinde yapılan tüm işlemleri iptal eder.
public interface ITransactionalRequest
{

}
