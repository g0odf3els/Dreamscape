using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Application.Collections.Commands.AutoAppendFileToCollection
{
    public class AutoAppendFileToCollectionCommandValidator : AbstractValidator<AutoAppendFileToCollectionCommand>
    {
        public AutoAppendFileToCollectionCommandValidator()
        {
        }
    }
}
