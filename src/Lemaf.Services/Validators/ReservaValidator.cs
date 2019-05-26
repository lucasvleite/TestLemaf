using FluentValidation;
using Lemaf.Entities;
using Lemaf.Services.Properties;
using System;

namespace Lemaf.Services.Validators
{
    public class ReservaValidator : AbstractValidator<Reserva>
    {
        public ReservaValidator()
        {
            RuleFor(e => e.DataInicio.Date)
                .NotEmpty().WithMessage(Resources.ErroDataInicio)
                .LessThan(e => e.DataFinal).WithMessage(Resources.ErroDataInicioMaiorQueFinal)
                .GreaterThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage(Resources.ErroMinimoUmDiaAntecedencia)
                .LessThanOrEqualTo(DateTime.Now.AddDays(40)).WithMessage(Resources.ErroMaximoQuarentaDiasAntecedencia)
                .When(x => x.DataInicio.DayOfWeek
                    .Equals(DayOfWeek.Monday)
                    .Equals(DayOfWeek.Tuesday)
                    .Equals(DayOfWeek.Wednesday)
                    .Equals(DayOfWeek.Thursday)
                    .Equals(DayOfWeek.Friday))
                .WithMessage(Resources.ErroDiaUtil);

            RuleFor(e => e.DataFinal)
                .GreaterThan(e => e.DataInicio).WithMessage(Resources.ErroDataInicioMaiorQueFinal)
                .NotEmpty().WithMessage(Resources.ErroDataFinal);

            RuleFor(e => e.DataFinal.Subtract(e.DataInicio))
                .LessThanOrEqualTo(new TimeSpan(8, 0, 0))
                .WithMessage(Resources.ErroDuracao);

            RuleFor(e => e.QuantidadePessoas)
                .LessThanOrEqualTo(20).WithMessage(Resources.ErroQuantidadePessoasInsuficiente)
                .GreaterThanOrEqualTo(1).WithMessage(Resources.ErroQuantidadePessoasMaisQueSuficiente)
                .NotEmpty().WithMessage(Resources.ErroQuantidadePessoas);
        }
    }
}
